using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de domínio relacionado a nota fiscal.
    /// </summary>
    public class NotaFiscalService : DomainServiceBase<INotaFiscalGateway>, INotaFiscalService
    {
        #region Fields
        private readonly INotaFiscalItemGateway m_notaFiscalItemGateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotaFiscalService"/>.
        /// </summary>
        /// <param name="notaFiscalGateway">O table data gateway para nota fiscal.</param>
        /// <param name="notaFiscalItemGateway">O table data gateway para item de nota fiscal.</param>
        public NotaFiscalService(INotaFiscalGateway notaFiscalGateway, INotaFiscalItemGateway notaFiscalItemGateway)
            : base(notaFiscalGateway)
        {
            m_notaFiscalItemGateway = notaFiscalItemGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém uma nota fiscal pelo seu id e retorna a nota fiscal com informações das entidades associadas.
        /// </summary>
        /// <param name="idNotaFiscal">O id de nota fiscal.</param>
        /// <returns>A NotaFiscal com informações de Loja, Fornecedor e Bandeira.</returns>
        public NotaFiscal ObterEstruturadoPorId(int idNotaFiscal)
        {
            return this.MainGateway.ObterEstruturadoPorId(idNotaFiscal);
        }

        /// <summary>
        /// Pesquisa detalhe de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<NotaFiscal> PesquisarPorFiltros(NotaFiscalFiltro filtro, Paging paging)
        {
            Assert(new { dtRecebimento = filtro.DtRecebimento }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorFiltros(filtro, paging);
        }

        /// <summary>
        /// Pesquisa de custos de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens</returns>
        public IEnumerable<CustoNotaFiscal> PesquisarCustosPorFiltros(NotaFiscalFiltro filtro, Paging paging)
        {
            Assert(new { dtRecebimento = filtro.DtRecebimento }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarCustosPorFiltros(filtro, paging);
        }

        /// <summary>
        /// Obtém o último item de nota fiscal recebido na loja para o item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>O item na última nota fiscal da loja.</returns>
        public NotaFiscalItem ObterItemNaUltimaNotaRecebidaDaLoja(int idItemDetalhe, int idLoja)
        {
            return MainGateway.ObterItemNaUltimaNotaRecebidaDaLoja(idItemDetalhe, idLoja);
        }

        /// <summary>
        /// Obtém os itens da nota fiscal com o id informado.
        /// </summary>
        /// <param name="idNotaFiscal">O id da nota fiscal.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens da nota fiscal.</returns>        
        public IEnumerable<NotaFiscalItem> ObterItensDaNotaFiscal(int idNotaFiscal, Paging paging)
        {
            return MainGateway.ObterItensDaNotaFiscal(idNotaFiscal, paging);
        }

        /// <summary>
        /// Pesquisa ultimas entradas de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="idItemDetalhe">O ID do item</param>
        /// <param name="idLoja">O ID da loja</param>        
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As entradas</returns>
        public IEnumerable<NotaFiscalConsolidado> PesquisarUltimasEntradasPorFiltros(long idItemDetalhe, int idLoja, DateTime dtSolicitacao, Paging paging)
        {
            Assert(new { dtSolicitacao = dtSolicitacao }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarUltimasEntradasPorFiltro(idItemDetalhe, idLoja, dtSolicitacao, paging);
        }

        /// <summary>
        /// Pesquisa os custos do item.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja</param>
        /// <param name="cdItem">O codigo do item</param>
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <returns>Os custos do item.</returns>
        public NotaFiscalItemCustosConsolidado ObterCustosPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao)
        {
            return this.MainGateway.ObterCustosPorItem(cdLoja, cdItem, dtSolicitacao);
        }

        /// <summary>
        /// Verifica se existe notas pendentes para o item informado
        /// </summary>
        /// <param name="cdLoja">O codigo da loja</param>
        /// <param name="cdItem">O codigo do item</param>
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <returns>Verdadeiro ou falso</returns>
        public bool ExisteNotasPendentesPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao)
        {
            return this.MainGateway.ExisteNotasPendentesPorItem(cdLoja, cdItem, dtSolicitacao);
        }

        /// <summary>
        /// Corrige os custos de notas fiscais.
        /// </summary>
        /// <param name="custos">Os custos.</param>
        public void CorrigirCustos(IEnumerable<CustoNotaFiscal> custos)
        {
            var custosLiberados = custos.Where(o => o.blLiberar).ToList();

            foreach (var custo in custosLiberados)
            {
                var notaFiscalItem = m_notaFiscalItemGateway.FindById(custo.IDNotaFiscalItem);

                AtualizarCorrecaoCustoNotaFiscalItem(custo, notaFiscalItem);

                m_notaFiscalItemGateway.Update(notaFiscalItem);
                m_notaFiscalItemGateway.SalvarLogCorrecaoCusto(notaFiscalItem);
                m_notaFiscalItemGateway.LiberarItemNotaFiscalDivergente(notaFiscalItem, custo.IDBandeira);
            }
        }

        private static void AtualizarCorrecaoCustoNotaFiscalItem(CustoNotaFiscal custo, NotaFiscalItem notaFiscalItem)
        {
            notaFiscalItem.dtLiberacao = DateTime.UtcNow;
            notaFiscalItem.qtItemAnterior = notaFiscalItem.qtItem;
            notaFiscalItem.blDivergente = false;

            AlterarStatusNotaFiscalItem(custo, notaFiscalItem);

            notaFiscalItem.qtItem = custo.qtAjustada;

            if (custo.qtAjustada != 0)
            {
                notaFiscalItem.vlCusto = notaFiscalItem.vlMercadoria / custo.qtAjustada;
            }
        }

        private static void AlterarStatusNotaFiscalItem(CustoNotaFiscal custo, NotaFiscalItem notaFiscalItem)
        {
            if (notaFiscalItem.IdNotaFiscalItemStatus != NotaFiscalItemStatus.IdConforme && custo.qtAjustada != notaFiscalItem.qtItem)
            {
                notaFiscalItem.IdNotaFiscalItemStatus = NotaFiscalItemStatus.IdAlterado;
            }
            else if (notaFiscalItem.IdNotaFiscalItemStatus != NotaFiscalItemStatus.IdConforme && custo.qtAjustada == notaFiscalItem.qtItem)
            {
                notaFiscalItem.IdNotaFiscalItemStatus = NotaFiscalItemStatus.IdRevisado;
            }
        }
        #endregion
    }
}
