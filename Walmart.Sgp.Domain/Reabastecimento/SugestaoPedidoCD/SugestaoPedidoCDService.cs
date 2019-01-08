using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a SugestaoPedidoCD.
    /// </summary>
    public class SugestaoPedidoCDService : EntityDomainServiceBase<SugestaoPedidoCD, ISugestaoPedidoCDGateway>, ISugestaoPedidoCDService
    {
        #region Fields
        private static String[] s_auditProperties = new String[] { "dtEnvioPedido", "dtCancelamentoPedido", "qtdPackCompra", "blFinalizado" };
        private readonly IAuditService m_auditService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoCDService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para SugestaoPedidoCD.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public SugestaoPedidoCDService(ISugestaoPedidoCDGateway mainGateway, IAuditService auditService)
            : base(mainGateway)
        {
            m_auditService = auditService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém um SugestaoPedidoCD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade SugestaoPedidoCD.</returns>
        public SugestaoPedidoCD ObterPorIdEstruturado(long id)
        {
            return this.MainGateway.ObterPorIdEstruturado(id);
        }

        /// <summary>
        /// Pesquisar SugestaoPedidoCD com base nos filtros
        /// </summary>
        /// <param name="filtro">Filtro da pesquisa.</param>
        /// <param name="paging">Parametro de paginacao.</param>
        /// <returns>Retorna lista de SugestaoPedidoCD.</returns>
        public IEnumerable<SugestaoPedidoCD> Pesquisar(SugestaoPedidoCDFiltro filtro, Paging paging)
        {
            Assert(new { requestDate = filtro.DtSolicitacao, IdDepartamento = filtro.IdDepartamento, cd = filtro.IdCD }, new AllMustBeInformedSpec());

            Assert(filtro.DtSolicitacao, new DataValidaSpec());

            var result = this.MainGateway.Pesquisar(filtro, paging) ?? new SugestaoPedidoCD[0];
            foreach (var r in result)
            {
                PopularDatasVM(r);
            }

            return result;
        }

        /// <summary>
        /// Realiza a finalização dos pedidos de sugestão CD.
        /// </summary>
        /// <param name="sugestoesPedidoCD">A lista contendo os pedidos que serão finalizados.</param>
        public void FinalizarPedidos(IEnumerable<SugestaoPedidoCD> sugestoesPedidoCD)
        {
            foreach (var sugestao in sugestoesPedidoCD)
            {
                PopularDatas(sugestao);
            }

            Assert(new { SugestoesPedidoCD = sugestoesPedidoCD }, new AllMustBeInformedSpec());
            Assert(sugestoesPedidoCD, new SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec(MainGateway.ExisteSugestoesFinalizadasMesmoItemDetalheSaida));

            sugestoesPedidoCD
                .ToList()
                .ForEach(spcd => spcd.blFinalizado = true);

            SalvarVarios(sugestoesPedidoCD);
        }

        /// <summary>
        /// Valida DataCancelamento de um SugestaoPedidoCD
        /// </summary>
        /// <param name="sugestaoPedidoCD">Sugestão Pedido CD</param>
        /// <returns>SpecResult contendo o resultado.</returns>
        public SpecResult ValidarDataCancelamento(SugestaoPedidoCD sugestaoPedidoCD)
        {
            try
            {
                PopularDatas(sugestaoPedidoCD);
            }
            catch (FormatException ex)
            {
                return new SpecResult(false, ex.Message);
            }

            return new SugestaoPedidoCDDataCancelValidaSpec().IsSatisfiedBy(sugestaoPedidoCD);
        }

        /// <summary>
        /// Valida DataEnvio de um SugestaoPedidoCD
        /// </summary>
        /// <param name="sugestaoPedidoCD">Sugestão Pedido CD</param>
        /// <returns>SpecResult contendo o resultado.</returns>
        public SpecResult ValidarDataEnvio(SugestaoPedidoCD sugestaoPedidoCD)
        {
            try
            {
                PopularDatas(sugestaoPedidoCD);
            }
            catch (FormatException ex)
            {
                return new SpecResult(false, ex.Message);
            }

            return new SugestaoPedidoCDDataEnvioValidaSpec().IsSatisfiedBy(sugestaoPedidoCD);
        }

        /// <summary>
        /// Salvar um SugestaoPedidoCD.
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public override void Salvar(SugestaoPedidoCD entidade)
        {
            PopularDatas(entidade);

            Assert(entidade, new SugestaoPedidoCDDataEnvioValidaSpec());
            Assert(entidade, new SugestaoPedidoCDDataCancelValidaSpec());
            Assert(entidade, new SugestaoPedidoCDQtdPackCompraValidoSpec());

            if (entidade.qtdPackCompra == null)
            {
                entidade.qtdPackCompra = 0;
            }

            this.MainGateway.Update("qtdPackCompra=@qtdPackCompra,dtEnvioPedido=@dtEnvioPedido,dtCancelamentoPedido=@dtCancelamentoPedido,blFinalizado=@blFinalizado", entidade);
            m_auditService.LogUpdate(entidade, s_auditProperties);
        }

        /// <summary>
        /// Salvar uma lista de SugestaoPedidoCD.
        /// </summary>
        /// <param name="sugestoesPedidoCD">A lista contendo os pedidos que serão salvos.</param>
        public void SalvarVarios(IEnumerable<SugestaoPedidoCD> sugestoesPedidoCD)
        {
            foreach (var sugestao in sugestoesPedidoCD)
            {
                this.Salvar(sugestao);
            }
        }

        private static void PopularDatasVM(SugestaoPedidoCD entidade)
        {
            // Code-review: questões de tela devem ser resolvidas nas devidas camadas, ViewModels não devem estar no domínio.
            entidade.dtEnvioPedidoSerialized = entidade.dtEnvioPedido == null ? null : entidade.dtEnvioPedido.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture);

            entidade.dtCancelamentoPedidoSerialized = entidade.dtCancelamentoPedido == null ? null : entidade.dtCancelamentoPedido.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture);
        }

        private static void PopularDatas(SugestaoPedidoCD entidade)
        {
            if (!string.IsNullOrWhiteSpace(entidade.dtCancelamentoPedidoSerialized))
            {
                try
                {
                    // Code-review: questões de tela devem ser resolvidas nas devidas camadas, ViewModels não devem estar no domínio.
                    entidade.dtCancelamentoPedido = DateTime.Parse(entidade.dtCancelamentoPedidoSerialized, RuntimeContext.Current.Culture);
                }
                catch
                {
                    throw new FormatException(Texts.InvalidDateFormatException);
                }
            }

            if (!string.IsNullOrWhiteSpace(entidade.dtEnvioPedidoSerialized))
            {
                try
                {
                    // Code-review: questões de tela devem ser resolvidas nas devidas camadas, ViewModels não devem estar no domínio.
                    entidade.dtEnvioPedido = DateTime.Parse(entidade.dtEnvioPedidoSerialized, RuntimeContext.Current.Culture);
                }
                catch
                {
                    throw new FormatException(Texts.InvalidDateFormatException);
                }
            }
        }
        #endregion
    }
}