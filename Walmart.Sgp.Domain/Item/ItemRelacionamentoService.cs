using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Serviço de domínio relacionado a relacionamentos entre itens.
    /// </summary>
    public class ItemRelacionamentoService : EntityDomainServiceBase<RelacionamentoItemPrincipal, IItemRelacionamentoGateway>, IItemRelacionamentoService
    {
        #region Fields
        private readonly IMultisourcingGateway m_multisourcingGateway;
        private readonly IRelacionamentoItemPrincipalHistGateway m_principalHistGateway;
        private readonly IRelacionamentoItemSecundarioHistGateway m_secundarioHistGateway;
        private readonly IItemDetalheService m_itemDetalheService;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemRelacionamentoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para relacionamento de item.</param>
        /// <param name="principalHistGateway">O table data gateway para histórico de relacionamento de item principal.</param>
        /// <param name="secundarioHistGateway">O table data gateway para histórico de relacionamento de item secundário.</param>
        /// <param name="itemDetalheService">O serviço de item detalhe.</param>
        /// <param name="multisourcingGateway">O table data gateway para multisourcing.</param>
        public ItemRelacionamentoService(
            IItemRelacionamentoGateway mainGateway,
            IRelacionamentoItemPrincipalHistGateway principalHistGateway,
            IRelacionamentoItemSecundarioHistGateway secundarioHistGateway,
            IItemDetalheService itemDetalheService, 
            IMultisourcingGateway multisourcingGateway)
            : base(mainGateway)
        {
            m_principalHistGateway = principalHistGateway;
            m_secundarioHistGateway = secundarioHistGateway;
            m_itemDetalheService = itemDetalheService;
            m_multisourcingGateway = multisourcingGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Pesquisa relacionamentos por informações dos itens, departamento e sistema.
        /// </summary>
        /// <param name="tipoRelacionamento">O tipo de relacionamento.</param>
        /// <param name="dsItem">Descrição do item.</param>
        /// <param name="cdItem">O código do item (cdItem).</param>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os relacionamentos (item principal e itens secundários).</returns>
        public IEnumerable<RelacionamentoItemPrincipal> PesquisarPorTipoRelacionamento(TipoRelacionamento tipoRelacionamento, string dsItem, int? cdItem, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int cdSistema, int? idRegiaoCompra, Paging paging)
        {
            if (cdItem.HasValue && cdItem.Value == 0)
            {
                cdItem = null;
            }

            Assert(new { tipoRelacionamento, cdSistema }, new AllMustBeInformedSpec());
            Assert(new { cdDepartamento, cdCategoria, cdSubcategoria, cdFineLine, cdItem, dsItem, PurchaseArea = idRegiaoCompra }, new AtLeastOneMustBeInformedSpec(true));

            return this.MainGateway.PesquisarPorTipoRelacionamento(tipoRelacionamento, dsItem, cdItem, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idRegiaoCompra, paging);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public override void Salvar(RelacionamentoItemPrincipal entidade)
        {
            ValidarItemRelacionamento(entidade);
            AtribuirDadosBasicosItemDetalhe(entidade);
            AtualizarTipoRelacionamento(entidade);

            var secundarios = entidade.RelacionamentoSecundario.Select(s => s.ItemDetalhe);

            AtualizarTipoRelacionamento(secundarios);

            var novosSecundarios = entidade.RelacionamentoSecundario.Where(t => t.IsNew).Select(s => s.ItemDetalhe);
            var tipoAcao = entidade.IsNew ? TipoAcao.Insercao : TipoAcao.Alteracao;

            ValidarItemDetalheESecundarios(entidade, secundarios, novosSecundarios);

            entidade.MarcarTipoItemEntrada();
            entidade.MarcarParaReprocessar();

            IEnumerable<RelacionamentoItemSecundario> secundariosRemovidos;

            secundariosRemovidos = SalvarRelacionamento(entidade, tipoAcao);

            var principalHist = CriarHistoricosParaPrincipal(entidade, tipoAcao);
            CriarHistoricosParaSecundarios(entidade.RelacionamentoSecundario, principalHist, tipoAcao);
            CriarHistoricosParaSecundarios(secundariosRemovidos, principalHist, TipoAcao.Exclusao);

            // Marca o item princial e os secundarios como sendo usados pelo tipo de relacionamento e desmarca os itens secundarios que foram removidos.
            var strategy = ItemFactory.CreateTipoRelacionamentoStrategy(entidade, m_itemDetalheService);
            strategy.MarcarItemDetalheComoPrincipal(entidade.ItemDetalhe.Id);
            MarcarItensSecundarios(strategy, entidade, entidade.RelacionamentoSecundario);
            DesmarcarItensSecundarios(strategy, entidade, secundariosRemovidos);
        }

        /// <summary>
        /// Remove a entidade com o id informado.
        /// </summary>
        /// <param name="id">O id da entidade a ser removida.</param>
        public override void Remover(int id)
        {
            var entidade = MainGateway.FindById(id);

            Assert(entidade, new RelacionamentoItemPodeSerRemovidoSpec(m_multisourcingGateway.VerificaRelacionamentoExistente));

            // Desmarca o item princial e os secundarios como sendo usados pelo tipo de relacionamento.
            var strategy = ItemFactory.CreateTipoRelacionamentoStrategy(entidade, m_itemDetalheService);
            strategy.DesmarcarItemDetalheComoPrincipal(entidade.ItemDetalhe.Id);
            DesmarcarItensSecundarios(strategy, entidade, entidade.RelacionamentoSecundario);

            var principalHist = CriarHistoricosParaPrincipal(entidade, TipoAcao.Exclusao);
            CriarHistoricosParaSecundarios(entidade.RelacionamentoSecundario, principalHist, TipoAcao.Exclusao);
            MainGateway.Delete(id);
        }

        /// <summary>
        /// Determina o percentual de rendimento transformado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemPrincipal.</remarks>
        public decimal? ObterPercentualRendimentoTransformado(int idItemDetalhe, byte cdSistema)
        {
            Assert(new { ItemCode = idItemDetalhe, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPercentualRendimentoTransformado(idItemDetalhe, cdSistema);
        }

        /// <summary>
        /// Determina o percentual de rendimento derivado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemSecundario.</remarks>
        public decimal? ObterPercentualRendimentoDerivado(int idItemDetalhe, byte cdSistema)
        {
            Assert(new { ItemCode = idItemDetalhe, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPercentualRendimentoDerivado(idItemDetalhe, cdSistema);
        }

        /// <summary>
        /// Conta o número de vezes que um item detalhe foi utilizado como saída em outros relacionamentos.
        /// </summary>
        /// <param name="idRelacionamentoItemPrincipalCorrente">O id do relacionamento item principal corrente.</param>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>        
        /// <returns>A contagem.</returns>
        public int ContarItemDetalheComoSaidaEmOutrosRelacionamentos(int idRelacionamentoItemPrincipalCorrente, int idItemDetalhe)
        {
            return MainGateway.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(idRelacionamentoItemPrincipalCorrente, idItemDetalhe);
        }

        /// <summary>
        /// Pesquisa informações sobre itens relacionados ao item informado.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Informações sobre itens relacionados.</returns>
        public ItensRelacionadosResponse ObterItensRelacionados(int cdItem, int? idLoja)
        {
            return this.MainGateway.ObterItensRelacionados(cdItem, idLoja);
        }

        private static void AtribuirDadosBasicosItemDetalhe(RelacionamentoItemPrincipal entidade)
        {
            var itemDetalhe = entidade.ItemDetalhe;
            entidade.IDItemDetalhe = itemDetalhe.Id;
            entidade.cdSistema = itemDetalhe.CdSistema;
            entidade.IDDepartamento = itemDetalhe.IDDepartamento;
            entidade.IDCategoria = itemDetalhe.IDCategoria;
        }

        private static void ValidarItemRelacionamento(RelacionamentoItemPrincipal entidade)
        {
            Assert(entidade, new RelacionamentoItemPodeSerSalvoSpec());
            Assert(entidade, new RelacionamentoItemReceituarioPesoPodeSerSalvoSpec());
        }

        private static void MarcarItensSecundarios(ITipoRelacionamentoStrategy strategy, RelacionamentoItemPrincipal principal, IEnumerable<RelacionamentoItemSecundario> secundarios)
        {
            foreach (var secundario in secundarios)
            {
                // Bug 4699:Receituario: Ao Vincular um Item Transformado como Insumo, está setando tpReceituario = I
                if (principal.TipoRelacionamento == TipoRelacionamento.Receituario
                && secundario.ItemDetalhe.TpReceituario == TipoReceituario.Transformado)
                {
                    continue;
                }

                strategy.MarcarItemDetalheComoSecundario(secundario.IDItemDetalhe.Value);
            }
        }

        private void DesmarcarItensSecundarios(ITipoRelacionamentoStrategy strategy, RelacionamentoItemPrincipal principal, IEnumerable<RelacionamentoItemSecundario> secundariosRemovidos)
        {
            // Desmarca os itens secundarios que foram removidos do relacionamento.
            foreach (var secundario in secundariosRemovidos)
            {
                var idItemDetalhe = secundario.IDItemDetalhe.Value;

                // Bug 4638:Sistema não permite relacionar o item.
                if (principal.TipoRelacionamento == TipoRelacionamento.Manipulado
                 || MainGateway.ContarItemDetalheEmOutrosRelacionamentos(principal.IDRelacionamentoItemPrincipal, idItemDetalhe, principal.TipoRelacionamento) == 0)
                {
                    strategy.DesmarcarItemDetalheComoSecundario(idItemDetalhe);
                }
            }
        }

        private void AtualizarTipoRelacionamento(RelacionamentoItemPrincipal principal)
        {
            var principalItemDetalhe = m_itemDetalheService.ObterPorId(principal.IDItemDetalhe);

            principal.ItemDetalhe.TpManipulado = principalItemDetalhe.TpManipulado;
            principal.ItemDetalhe.TpVinculado = principalItemDetalhe.TpVinculado;
            principal.ItemDetalhe.TpReceituario = principalItemDetalhe.TpReceituario;
        }

        private void AtualizarTipoRelacionamento(IEnumerable<ItemDetalhe> secundarios)
        {
            foreach (var secundario in secundarios)
            {
                var secundarioItemDetalhe = m_itemDetalheService.ObterPorId(secundario.IDItemDetalhe);

                secundario.TpManipulado = secundarioItemDetalhe.TpManipulado;
                secundario.TpVinculado = secundarioItemDetalhe.TpVinculado;
                secundario.TpReceituario = secundarioItemDetalhe.TpReceituario;
            }
        }

        private IEnumerable<RelacionamentoItemSecundario> SalvarRelacionamento(RelacionamentoItemPrincipal entidade, TipoAcao tipoAcao)
        {
            IEnumerable<RelacionamentoItemSecundario> secundariosRemovidos;

            if (tipoAcao == TipoAcao.Insercao)
            {
                entidade.DhCadastro = DateTime.Now;
                MainGateway.Insert(entidade);
                secundariosRemovidos = new RelacionamentoItemSecundario[0];
            }
            else
            {
                var entidadeExistente = MainGateway.FindById(entidade.Id);
                entidade.DhCadastro = entidadeExistente.DhCadastro;
                entidade.DhAlteracao = DateTime.Now;
                MainGateway.Update(entidade);
                secundariosRemovidos = entidadeExistente.RelacionamentoSecundario.Where(e => !entidade.RelacionamentoSecundario.Any(n => e.IDItemDetalhe == n.IDItemDetalhe));
            }

            return secundariosRemovidos;
        }

        private RelacionamentoItemPrincipalHist CriarHistoricosParaPrincipal(RelacionamentoItemPrincipal entidade, TipoAcao tipoAcao)
        {
            var principalHist = RelacionamentoItemPrincipalHist.Create(entidade, tipoAcao);
            m_principalHistGateway.Insert(principalHist);

            return principalHist;
        }

        private void CriarHistoricosParaSecundarios(IEnumerable<RelacionamentoItemSecundario> secundarios, RelacionamentoItemPrincipalHist principalHist, TipoAcao tipoAcao)
        {
            foreach (var secundario in secundarios)
            {
                m_secundarioHistGateway.Insert(RelacionamentoItemSecundarioHist.Create(principalHist, secundario, tipoAcao));
            }
        }

        private void ValidarItemDetalheESecundarios(RelacionamentoItemPrincipal entidade, IEnumerable<ItemDetalhe> secundarios, IEnumerable<ItemDetalhe> novosSecundarios)
        {
            var principalEhSaida = entidade.EhSaida();

            // Bug 4681:Bug - Relac. Manipulado - Dá mensagem inválida ao salvar.
            if (entidade.IsNew)
            {
                Assert(entidade.ItemDetalhe, new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(this, entidade, principalEhSaida));
            }

            Assert(novosSecundarios, new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(entidade));
            Assert(secundarios, new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(this, entidade, !principalEhSaida));
        }

        #endregion
    }
}
