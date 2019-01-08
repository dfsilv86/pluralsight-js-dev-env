using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma SugestaoPedidoCD.
    /// </summary>
    public class SugestaoPedidoCD : EntityBase, IAggregateRoot
    {
        private const int FatorConversaoReviewDate2DayOfWeek = 1;

        private const int TotalDiasSemana = 7;

        private DateTime? m_today;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoCD" />
        /// </summary>
        /// <param name="today">A data atual (utilizado apenas nos testes unitários do cálculo do Próximo Review Date).</param>
        public SugestaoPedidoCD(DateTime today)
        {
            m_today = today;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoCD" />
        /// </summary>
        public SugestaoPedidoCD()
        {
        }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)IDSugestaoPedidoCD;
            }

            set
            {
                IDSugestaoPedidoCD = value;
            }
        }

        /// <summary>
        /// Obtém ou define idSugestaoPedidoCD.
        /// </summary>
        public long IDSugestaoPedidoCD { get; set; }

        /// <summary>
        /// Obtém ou define idFornecedorParametro.
        /// </summary>
        public long idFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define idItemDetalhePedido.
        /// </summary>
        public long idItemDetalhePedido { get; set; }

        /// <summary>
        /// Obtém ou define idItemDetalheSugestao.
        /// </summary>
        public long idItemDetalheSugestao { get; set; }

        /// <summary>
        /// Obtém ou define idCD.
        /// </summary>
        public int idCD { get; set; }

        /// <summary>
        /// Obtém ou define dtPedido.
        /// </summary>
        public DateTime dtPedido { get; set; }

        /// <summary>
        /// Obtém ou define dtEnvioPedido.
        /// </summary>
        public DateTime dtEnvioPedido { get; set; }

        /// <summary>
        /// Obtém ou define dtCancelamentoPedido.
        /// </summary>
        public DateTime dtCancelamentoPedido { get; set; }

        /// <summary>
        /// Obtém ou define dtEnvioPedidoSerialized.
        /// </summary>
        public string dtEnvioPedidoSerialized { get; set; }

        /// <summary>
        /// Obtém ou define dtCancelamentoPedidoSerialized.
        /// </summary>
        public string dtCancelamentoPedidoSerialized { get; set; }

        /// <summary>
        /// Obtém ou define dtCancelamentoPedidoOriginal.
        /// </summary>
        public DateTime dtCancelamentoPedidoOriginal { get; set; }

        /// <summary>
        /// Obtém ou define dtInicioForecast.
        /// </summary>
        public DateTime dtInicioForecast { get; set; }

        /// <summary>
        /// Obtém ou define dtFimForecast.
        /// </summary>
        public DateTime dtFimForecast { get; set; }

        /// <summary>
        /// Obtém ou define tpWeek.
        /// </summary>
        public int tpWeek { get; set; }

        /// <summary>
        /// Obtém ou define tpInterval.
        /// </summary>
        public int tpInterval { get; set; }

        /// <summary>
        /// Obtém ou define cdReviewDate.
        /// </summary>
        public int cdReviewDate { get; set; }

        /// <summary>
        /// Obtém ou define vlLeadTime.
        /// </summary>
        public int vlLeadTime { get; set; }

        /// <summary>
        /// Obtém ou define qtVendorPackage.
        /// </summary>
        public int qtVendorPackage { get; set; }

        /// <summary>
        /// Obtém ou define vlEstoqueSeguranca.
        /// </summary>
        public int vlEstoqueSeguranca { get; set; }

        /// <summary>
        /// Obtém ou define tempoMinimoCD.
        /// </summary>
        public int tempoMinimoCD { get; set; }

        /// <summary>
        /// Obtém ou define tpCaixaFornecedor.
        /// </summary>
        public string tpCaixaFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define vlPesoLiquido.
        /// </summary>
        public decimal vlPesoLiquido { get; set; }

        /// <summary>
        /// Obtém ou define vlTipoReabastecimento.
        /// </summary>
        public int? vlTipoReabastecimento { get; set; }

        /// <summary>
        /// Obtém ou define vlCusto.
        /// </summary>
        public decimal vlCusto { get; set; }

        /// <summary>
        /// Obtém ou define qtdPackCompra.
        /// </summary>
        public int? qtdPackCompra { get; set; }

        /// <summary>
        /// Obtém ou define qtdPackCompraOriginal.
        /// </summary>
        public int? qtdPackCompraOriginal { get; set; }

        /// <summary>
        /// Obtém ou define qtdOnHand.
        /// </summary>
        public int qtdOnHand { get; set; }

        /// <summary>
        /// Obtém ou define qtdOnOrder.
        /// </summary>
        public int qtdOnOrder { get; set; }

        /// <summary>
        /// Obtém ou define qtdForecast.
        /// </summary>
        public int qtdForecast { get; set; }

        /// <summary>
        /// Obtém ou define qtdPipeline.
        /// </summary>
        public int qtdPipeline { get; set; }

        /// <summary>
        /// Obtém ou define IdOrigemDadosCalculo.
        /// </summary>
        public int IdOrigemDadosCalculo { get; set; }

        /// <summary>
        /// Obtém ou define blFinalizado.
        /// </summary>
        public bool blFinalizado { get; set; }

        /// <summary>
        /// Obtém ou define tpStatusEnvio.
        /// </summary>
        public string tpStatusEnvio { get; set; }

        /// <summary>
        /// Obtém ou define dhEnvioSugestao.
        /// </summary>
        public DateTime? dhEnvioSugestao { get; set; }

        #region Propriedades calculadas
        /// <summary>
        /// Obtém ou define ItemDetalhePedido
        /// </summary>
        public Walmart.Sgp.Domain.Item.ItemDetalhe ItemDetalhePedido { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalheSugestao
        /// </summary>
        public Walmart.Sgp.Domain.Item.ItemDetalhe ItemDetalheSugestao { get; set; }

        /// <summary>
        /// Obtém ou define FornecedorParametro
        /// </summary>
        public Gerenciamento.FornecedorParametro FornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define OrigemDadosCalculo
        /// </summary>
        public OrigemDadosCalculo OrigemDadosCalculo { get; set; }

        /// <summary>
        /// Obtém ou define CD
        /// </summary>
        public Walmart.Sgp.Domain.EstruturaMercadologica.CD CD { get; set; }

        /// <summary>
        /// Obtém ou define Selecionado
        /// </summary>
        public bool Selecionado { get; set; }

        /// <summary>
        /// Obtém ou define QtdPendente
        /// </summary>
        public int QtdPendente { get; set; }

        /// <summary>
        /// Obtém ou define MediaVendaDiaItem
        /// </summary>
        public int MediaVendaDiaItem { get; set; }

        /// <summary>
        /// Obtém ProximoReviewDate
        /// </summary>
        public DateTime? ProximoReviewDate
        {
            get
            {
                if (cdReviewDate == 0)
                {
                    return null;
                }

                var currentReviewDate = (int)Today.DayOfWeek + FatorConversaoReviewDate2DayOfWeek;

                var splitedReviewDate = cdReviewDate
                    .ToString(RuntimeContext.Current.Culture)
                    .Select(rd => int.Parse(rd.ToString(RuntimeContext.Current.Culture), RuntimeContext.Current.Culture))
                    .OrderBy(rd => rd);

                var greaterReviewDate = splitedReviewDate.FirstOrDefault(reviewDate => reviewDate > currentReviewDate);
                int daysToAdd = 0;

                if (greaterReviewDate == 0)
                {
                    daysToAdd = splitedReviewDate.First() + TotalDiasSemana - currentReviewDate;
                }
                else
                {
                    daysToAdd = greaterReviewDate - currentReviewDate;
                }

                return Today.AddDays(daysToAdd);
            }
        }
        #endregion

        /// <summary>
        /// Obtém ForecastMedio
        /// </summary>
        public int ForecastMedio
        {
            get
            {
                var numDias = (this.dtFimForecast - this.dtInicioForecast).Days + 1;
                return (int)CalcHelper.CustomDivision((double)this.qtdForecast, (double)numDias);
            }
        }

        /// <summary>
        /// Obtém ou Define TotalKgUn
        /// </summary>
        public int TotalKgUn { get; set; }

        /// <summary>
        /// Obtém Today
        /// </summary>
        public DateTime Today
        {
            get
            {
                return m_today ?? DateTime.Today;
            }
        }

        /// <summary>
        /// Obtém ou define vlReviewTime.
        /// </summary>
        public int vlReviewTime { get; set; }
    }
}