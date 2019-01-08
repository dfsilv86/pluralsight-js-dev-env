using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma SugestaoPedido.
    /// </summary>
    /// <remarks>Alterações aqui devem ser consideradas na cópia realizada no ctor de SugestaoPedidoModel!</remarks>
    public class SugestaoPedido : EntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedido"/>.
        /// </summary>
        public SugestaoPedido()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDSugestaoPedido;
            }

            set
            {
                IDSugestaoPedido = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDSugestaoPedido.
        /// </summary>
        public int IDSugestaoPedido { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhePedido.
        /// </summary>
        public int IDItemDetalhePedido { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalheSugestao.
        /// </summary>
        public int IDItemDetalheSugestao { get; set; }

        /// <summary>
        /// Obtém ou define ValorEmCaixa
        /// </summary>
        public int ValorEmCaixa { get; set; }

        /// <summary>
        /// Obtém ou define ValorEmCaixaRA
        /// </summary>
        public int ValorEmCaixaRA { get; set; }

        /// <summary>
        /// Obtém ou define IdLoja.
        /// </summary>
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define dtPedido.
        /// </summary>
        public DateTime dtPedido { get; set; }

        /// <summary>
        /// Obtém ou define tpWeek.
        /// </summary>
        public TipoSemana tpWeek { get; set; }

        /// <summary>
        /// Obtém ou define tpInterval.
        /// </summary>
        public TipoIntervalo tpInterval { get; set; }

        /// <summary>
        /// Obtém ou define cdReviewDate.
        /// </summary>
        public int cdReviewDate { get; set; }

        /// <summary>
        /// Obtém ou define vlLeadTime.
        /// </summary>
        public short vlLeadTime { get; set; }

        /// <summary>
        /// Obtém ou define qtVendorPackage.
        /// </summary>
        public int qtVendorPackage { get; set; }

        /// <summary>
        /// Obtém ou define dtProximoReviewDate.
        /// </summary>
        public DateTime dtProximoReviewDate { get; set; }

        /// <summary>
        /// Obtém ou define dtInicioForecast.
        /// </summary>
        public DateTime dtInicioForecast { get; set; }

        /// <summary>
        /// Obtém ou define dtFimForecast.
        /// </summary>
        public DateTime dtFimForecast { get; set; }

        /// <summary>
        /// Obtém ou define vlEstoqueSeguranca.
        /// </summary>
        public int vlEstoqueSeguranca { get; set; }

        /// <summary>
        /// Obtém ou define vlShelfLife.
        /// </summary>
        public decimal vlShelfLife { get; set; }

        /// <summary>
        /// Obtém ou define vlLeadTimeReal.
        /// </summary>
        public int vlLeadTimeReal { get; set; }

        /// <summary>
        /// Obtém ou define blAtendePedidoMinimo.
        /// </summary>
        public bool blAtendePedidoMinimo { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedorParametro.
        /// </summary>
        public long IDFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define qtdPackCompra.
        /// </summary>
        public int qtdPackCompra { get; set; }

        /// <summary>
        /// Obtém ou define qtdPackCompraOriginal.
        /// </summary>
        public int qtdPackCompraOriginal { get; set; }

        /// <summary>
        /// Obtém ou define cdOrigemCalculo.
        /// </summary>
        public TipoOrigemCalculo cdOrigemCalculo { get; set; }

        /// <summary>
        /// Obtém ou define o id da origem dos dados de cálculo.
        /// </summary>
        public int? IDOrigemDadosCalculo { get; set; }

        /// <summary>
        /// Obtém ou define a origem dos dados de cálculo.
        /// </summary>
        public OrigemDadosCalculo OrigemDadosCalculo { get; set; }

        /// <summary>
        /// Obtém ou define vlPackSugerido1.
        /// </summary>
        public int vlPackSugerido1 { get; set; }

        /// <summary>
        /// Obtém ou define vlModulo.
        /// </summary>
        public decimal vlModulo { get; set; }

        /// <summary>
        /// Obtém ou define vlEstoque.
        /// </summary>
        public decimal vlEstoque { get; set; }

        /// <summary>
        /// Obtém ou define vlTotalPedidosAberto.
        /// </summary>
        public decimal vlTotalPedidosAberto { get; set; }

        /// <summary>
        /// Obtém ou define vlPipeline.
        /// </summary>
        public decimal vlPipeline { get; set; }

        /// <summary>
        /// Obtém ou define vlForecast.
        /// </summary>
        public decimal vlForecast { get; set; }

        /// <summary>
        /// Obtém ou define vlForecastMedio.
        /// </summary>
        public decimal vlForecastMedio { get; set; }

        /// <summary>
        /// Obtém ou define vlEstoqueSegurancaQtd.
        /// </summary>
        public decimal vlEstoqueSegurancaQtd { get; set; }

        /// <summary>
        /// Obtém ou define vlQtdDiasEstoque.
        /// </summary>
        public decimal vlQtdDiasEstoque { get; set; }

        /// <summary>
        /// Obtém ou define vlSugestaoPedido.
        /// </summary>
        public decimal vlSugestaoPedido { get; set; }

        /// <summary>
        /// Obtém ou define vlEstoqueOriginal.
        /// </summary>
        public decimal vlEstoqueOriginal { get; set; }

        /// <summary>
        /// Obtém ou define vlFatorConversao.
        /// </summary>
        public float vlFatorConversao { get; set; }

        /// <summary>
        /// Obtém ou define blPossuiVendasUltimaSemana.
        /// </summary>
        public bool blPossuiVendasUltimaSemana { get; set; }

        /// <summary>
        /// Obtém ou define tpStatusEnvio.
        /// </summary>
        public string tpStatusEnvio { get; set; }

        /// <summary>
        /// Obtém ou define dhEnvioSugestao.
        /// </summary>
        public DateTime? dhEnvioSugestao { get; set; }

        /// <summary>
        /// Obtém ou define idCD.
        /// </summary>
        public int? idCD { get; set; }

        /// <summary>
        /// Obtém ou define vlTipoReabastecimento.
        /// </summary>
        public ValorTipoReabastecimento vlTipoReabastecimento { get; set; }
        
        //// -------------
        //// Alterações aqui devem ser consideradas na cópia realizada no ctor de SugestaoPedidoModel!
        //// -------------

        /// <summary>
        /// Obtém ou define o número de pedidos On Order.
        /// </summary>
        public decimal vlSaldoOO { get; set; }

        /// <summary>
        /// Obtém ou define o número de pedidos In Warehouse.
        /// </summary>
        public decimal vlSaldoIW { get; set; }

        /// <summary>
        /// Obtém ou define o número de pedidos In Transit.
        /// </summary>
        public decimal vlSaldoIT { get; set; }

        /// <summary>
        /// Obtém ou define Roteiro
        /// </summary>
        public Roteiro Roteiro { get; set; }

        /// <summary>
        /// Obtem ou define qtdSugestaoRoteiroRA
        /// </summary>
        public decimal? qtdSugestaoRoteiroRA { get; set; }

        /// <summary>
        /// Obtém ou define vlPesoLiquido
        /// </summary>
        public decimal? vlPesoLiquido { get; set; }

        /// <summary>
        /// Obtém ou define tpCaixaFornecedor.
        /// </summary>
        public TipoCaixaFornecedor TpCaixaFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define blReturnSheet.
        /// </summary>
        public bool blReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define blCDConvertido.
        /// </summary>
        public bool blCDConvertido { get; set; }

        /// <summary>
        /// Obtém ou define GradeAberta.
        /// </summary>
        public bool? GradeAberta { get; set; }
        #endregion

        #region Relacionamentos

        /// <summary>
        /// Obtém ou define o item detalhe do pedido.
        /// </summary>
        public ItemDetalhe ItemDetalhePedido { get; set; }

        /// <summary>
        /// Obtém ou define o item detalhe de sugestão.
        /// </summary>
        public ItemDetalhe ItemDetalheSugestao { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o parâmetro de fornecedor.
        /// </summary>
        public FornecedorParametro FornecedorParametro { get; set; }

        //// -------------
        //// Alterações aqui devem ser consideradas na cópia realizada no ctor de SugestaoPedidoModel!
        //// -------------

        #endregion

        #region Methods
        /// <summary>
        /// Calcula o valor original.
        /// </summary>
        /// <returns>O valor orignal.</returns>
        public decimal CalcularValorOriginal()
        {
            if (qtdPackCompraOriginal > 0)
            {
                return qtdPackCompraOriginal;
            }

            if (this.IsPesoLiquido() || this.IsPesoBruto())
            {
                return Math.Max(1, Math.Round(this.ObterPeso(), 0, MidpointRounding.AwayFromZero));
            }
            else
            {
                return 1m;
            }
        }

        /// <summary>
        /// Obtém o valor de qtdPackCompra em Caixa, se a sugestão for por KgOuUnidade ele converte antes de retornar. Se não for nenhum destes retorna zero.
        /// </summary>
        /// <returns>A quantidade em Caixa.</returns>
        public int QtdPackCompraToCaixa()
        {
            if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.Caixa)
            {
                return this.qtdPackCompra;
            }
            else if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.KgOuUnidade)
            {
                return (int)CalcHelper.CustomDivision((double)this.qtdPackCompra, (double)this.vlPesoLiquido);
            }

            return 0;
        }

        /// <summary>
        /// Obtém o valor de qtdSugestaoRoteiroRA em Caixa, se a sugestão for por KgOuUnidade ele converte antes de retornar. Se não for nenhum destes retorna zero.
        /// </summary>
        /// <returns>A quantidade em Caixa.</returns>
        public int QtdSugestaoRoteiroRAToCaixa()
        {
            if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.Caixa)
            {
                return Convert.ToInt32(this.qtdSugestaoRoteiroRA, RuntimeContext.Current.Culture);
            }
            else if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.KgOuUnidade)
            {
                return (int)CalcHelper.CustomDivision((double)this.qtdSugestaoRoteiroRA, (double)this.vlPesoLiquido);
            }

            return 0;
        }

        /// <summary>
        /// Obtém o valor de qtdPackCompra em Quilo, se a sugestão for por Caixa ele converte antes de retornar. Se não for nenhum destes retorna zero.
        /// </summary>
        /// <returns>A quantidade em Quilo.</returns>
        public int QtdPackCompraToQuilo()
        {
            if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.Caixa)
            {
                return this.qtdPackCompra * this.qtVendorPackage;
            }
            else if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.KgOuUnidade)
            {
                return qtdPackCompra;
            }

            return 0;
        }

        /// <summary>
        /// Obtém o valor de qtdSugestaoRoteiroRA em Quilo, se a sugestão for por Caixa ele converte antes de retornar. Se não for nenhum destes retorna zero.
        /// </summary>
        /// <returns>A quantidade em Quilo.</returns>
        public int QtdSugestaoRoteiroRAToQuilo()
        {
            if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.Caixa)
            {
                return Convert.ToInt32(this.qtdSugestaoRoteiroRA * this.qtVendorPackage, RuntimeContext.Current.Culture);
            }
            else if (this.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.KgOuUnidade)
            {
                return Convert.ToInt32(this.qtdSugestaoRoteiroRA, RuntimeContext.Current.Culture);
            }

            return 0;
        }
        #endregion
    }
}
