using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de domínio relacionado a estoque.
    /// </summary>
    public class EstoqueService : DomainServiceBase<IEstoqueGateway>, IEstoqueService
    {
        #region Fields
        private readonly IItemRelacionamentoGateway m_itemRelacionamentoGateway;
        private readonly IRelacionamentoItemSecundarioGateway m_relacionamentoItemSecundarioGateway;
        private readonly INotaFiscalService m_notaFiscalService;
        private readonly IItemDetalheService m_itemDetalheService;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EstoqueService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para main data.</param>
        /// <param name="itemRelacionamentoGateway">O table data gateway para item relacionamento.</param>
        /// <param name="relacionamentoItemSecundarioGateway">O table data gateway para item relacionamento secundario.</param>
        /// <param name="notaFiscalService">O serviço de nota fiscal.</param>
        /// <param name="itemDetalheService">O serviço de item detalhe.</param>
        public EstoqueService(
            IEstoqueGateway mainGateway,
            IItemRelacionamentoGateway itemRelacionamentoGateway,
            IRelacionamentoItemSecundarioGateway relacionamentoItemSecundarioGateway,
            INotaFiscalService notaFiscalService,
            IItemDetalheService itemDetalheService)
            : base(mainGateway)
        {
            m_itemRelacionamentoGateway = itemRelacionamentoGateway;
            m_relacionamentoItemSecundarioGateway = relacionamentoItemSecundarioGateway;
            m_notaFiscalService = notaFiscalService;
            m_itemDetalheService = itemDetalheService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtém a informação mais recente sobre custo de item do estoque das lojas.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A informação mais recente de custos. (conforme Estoque.dtRecebimento)</returns>
        public IEnumerable<Estoque> PesquisarUltimoCustoDoItemPorLoja(int idItemDetalhe, int? idLoja, Paging paging)
        {
            Assert(new { ItemCode = idItemDetalhe }, new AllMustBeInformedSpec());

            int idUsuario = RuntimeContext.Current.User.Id;
            var tipoPermissao = RuntimeContext.Current.User.TipoPermissao;

            return this.MainGateway.PesquisarUltimoCustoDoItemPorLoja(idItemDetalhe, idLoja, idUsuario, tipoPermissao, paging);
        }

        /// <summary>
        /// Obtém a lista de ids de item detalhe que são itens de entrada de um determinado item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>A lista de ids de itens de entrada.</returns>
        /// <remarks>Consulta recursiva no banco, está com OPTION MAXRECURSION 10 no momento.</remarks>
        public IEnumerable<int> ObterOrigemItem(int idItemDetalhe)
        {
            return this.MainGateway.ObterOrigemItem(idItemDetalhe);
        }

        /// <summary>
        /// Obtém os últimos 5 recebimentos do item ou de suas entradas.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 recebimentos.</returns>
        /// <remarks>Como a consulta utiliza também os itens de entrada do item informado, é possível que uma NotaFiscal possua mais de um NotaFiscalItem. O conjunto de todos NotaFiscalItem deve ter os 5 itens.</remarks>
        public IEnumerable<NotaFiscal> ObterOsCincoUltimosRecebimentosDoItemPorLoja(int idItemDetalhe, int idLoja)
        {
            Assert(new { idItemDetalhe, idLoja }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterOsCincoUltimosRecebimentosDoItemPorLoja(idItemDetalhe, idLoja);
        }

        /// <summary>
        /// Obtém os últimos 5 custo do item na loja
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 custos.</returns>
        public IEnumerable<CustoMaisRecente> ObterOsCincoUltimosCustosDoItemPorLoja(int cdItem, int idLoja)
        {
            Assert(new { cdItem, idLoja }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterOsCincoUltimosCustosDoItemPorLoja(cdItem, idLoja);
        }

        /// <summary>
        /// Obtém os custos mais recentes de itens relacionados a um item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>Os custos.</returns>
        public IEnumerable<CustoItemRelacionadoResponse> ObterUltimoCustoDeItensRelacionadosNaLoja(int idItemDetalhe, int idLoja)
        {
            var principais = m_itemRelacionamentoGateway.ObterPrincipaisPorItem(idItemDetalhe);
            var secundarios = m_relacionamentoItemSecundarioGateway.ObterSecundariosPorItem(idItemDetalhe);

            Dictionary<int, CustoMaisRecente> custos = new Dictionary<int, CustoMaisRecente>();
            custos[0] = new CustoMaisRecente(null, null);

            var result = principais
                .SelectMany(p => p.RelacionamentoSecundario.Select(s => new CustoItemRelacionadoResponse(true, p, s, null)))
                .Union(secundarios.SelectMany(p => p.RelacionamentoSecundario.Select(s => new CustoItemRelacionadoResponse(false, p, s, null)))).ToList();

            var idItemSecundarios = ObterIdItemSecundarios(result);

            foreach (var idItemSecundario in idItemSecundarios)
            {
                var custo = this.MainGateway.ObterUltimoCustoDoItemNaLoja(idItemSecundario, idLoja);
                custos[idItemSecundario] = custo;                
            }

            result.ForEach(x => x.CustoMaisRecente = custos[x.IsPrincipal ? x.RelacionamentoSecundario.IDItemDetalhe ?? 0 : x.RelacionamentoPrincipal.IDItemDetalhe]);

            return result;
        }     

        /// <summary>
        /// Obtém o custo contábil mais recente de um item em uma loja.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>O custo contábil mais recente.</returns>
        public decimal ObterUltimoCustoContabilItem(int idItemDetalhe, int idLoja)
        {
            Assert(new { idItemDetalhe, idLoja }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterUltimoCustoContabilItem(idItemDetalhe, idLoja);
        }

        /// <summary>
        /// Realiza o ajuste do estoque.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajudado.</param>
        public void Ajustar(Estoque estoque)
        {
            if (estoque.MotivoAjuste.Id == MotivoMovimentacao.IDErroDeQuebraPDV)
            {
                Assert(new { estoque.dhAtualizacao }, new AllMustBeInformedSpec());
            }
            else
            {
                estoque.dhAtualizacao = DateTime.Today;
            }
            
            Assert(estoque, new EstoquePodeSerAjustadoSpec(m_notaFiscalService));

            var item = m_itemDetalheService.ObterPorId(estoque.IDItemDetalhe);

            if (item.TpManipulado != TipoManipulado.NaoDefinido)
            {
                MainGateway.AjusteEstoqueManipulado(estoque);
            }
            else if (item.TpReceituario != TipoReceituario.NaoDefinido)
            {
                MainGateway.AjustarEstoqueReceituario(estoque);
            }
            else
            {
                MainGateway.AjustarEstoqueDireto(estoque);
            }
        }

        /// <summary>
        /// Realiza a movimentação do tipo MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">A movimentação do tipo MTR.</param>
        public void RealizarMtr(MovimentacaoMtr movimentacaoMtr)
        {
            Assert(movimentacaoMtr, new AllMustBeInformedSpec());
            Assert(movimentacaoMtr, new MtrPodeSerRealizadaSpec(m_itemDetalheService));

            var itemOrigem = m_itemDetalheService.ObterPorId(movimentacaoMtr.IdItemOrigem);

            if (itemOrigem.TpReceituario == TipoReceituario.Transformado)
            {
                MainGateway.AjustarEstoqueReceituario(movimentacaoMtr);
            }
            else if (itemOrigem.TpManipulado == TipoManipulado.Derivado)
            {
                MainGateway.AjusteEstoqueManipulado(movimentacaoMtr);
            }
            else
            {
                MainGateway.AjustarEstoqueDireto(movimentacaoMtr);
            }
        }

        private static IEnumerable<int> ObterIdItemSecundarios(List<CustoItemRelacionadoResponse> result)
        {
            return result
                .Where(x => x.IsPrincipal ? x.RelacionamentoSecundario.IDItemDetalhe.HasValue : 0 != x.RelacionamentoPrincipal.IDItemDetalhe)
                .Select(x => x.IsPrincipal ? x.RelacionamentoSecundario.IDItemDetalhe.Value : x.RelacionamentoPrincipal.IDItemDetalhe)
                .Distinct();
        }
        #endregion
    }
}
