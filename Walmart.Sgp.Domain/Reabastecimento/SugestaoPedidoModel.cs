using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Sugestão de pedido com campos adicionais usados em tela.
    /// </summary>
    //// TODO:  o domínio não deveria ter classes *Model. Essa classe parece ser outra coisa ou deveria estar nas models da web api.
    public class SugestaoPedidoModel : SugestaoPedido, ICloneable
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoModel"/>.
        /// </summary>
        public SugestaoPedidoModel()
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoModel"/>.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão original.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SugestaoPedidoModel(SugestaoPedido sugestaoPedido)
        {
            if (null != sugestaoPedido)
            {
                this.blAtendePedidoMinimo = sugestaoPedido.blAtendePedidoMinimo;
                this.blPossuiVendasUltimaSemana = sugestaoPedido.blPossuiVendasUltimaSemana;
                this.cdOrigemCalculo = sugestaoPedido.cdOrigemCalculo;
                this.cdReviewDate = sugestaoPedido.cdReviewDate;
                this.dhEnvioSugestao = sugestaoPedido.dhEnvioSugestao;
                this.dtFimForecast = sugestaoPedido.dtFimForecast;
                this.dtInicioForecast = sugestaoPedido.dtInicioForecast;
                this.dtPedido = sugestaoPedido.dtPedido;
                this.dtProximoReviewDate = sugestaoPedido.dtProximoReviewDate;
                this.FornecedorParametro = sugestaoPedido.FornecedorParametro;
                this.Id = sugestaoPedido.Id;
                this.IDFornecedorParametro = sugestaoPedido.IDFornecedorParametro;
                this.IDItemDetalhePedido = sugestaoPedido.IDItemDetalhePedido;
                this.IDItemDetalheSugestao = sugestaoPedido.IDItemDetalheSugestao;
                this.IdLoja = sugestaoPedido.IdLoja;
                this.IDSugestaoPedido = sugestaoPedido.IDSugestaoPedido;
                this.ItemDetalhePedido = sugestaoPedido.ItemDetalhePedido;
                this.ItemDetalheSugestao = sugestaoPedido.ItemDetalheSugestao;
                this.Loja = sugestaoPedido.Loja;
                this.OrigemDadosCalculo = sugestaoPedido.OrigemDadosCalculo;
                this.qtdPackCompra = sugestaoPedido.qtdPackCompra;
                this.qtdPackCompraOriginal = sugestaoPedido.qtdPackCompraOriginal;
                this.qtVendorPackage = sugestaoPedido.qtVendorPackage;
                this.tpInterval = sugestaoPedido.tpInterval;
                this.tpStatusEnvio = sugestaoPedido.tpStatusEnvio;
                this.tpWeek = sugestaoPedido.tpWeek;
                this.vlEstoque = sugestaoPedido.vlEstoque;
                this.vlEstoqueOriginal = sugestaoPedido.vlEstoqueOriginal;
                this.vlEstoqueSeguranca = sugestaoPedido.vlEstoqueSeguranca;
                this.vlEstoqueSegurancaQtd = sugestaoPedido.vlEstoqueSegurancaQtd;
                this.vlFatorConversao = sugestaoPedido.vlFatorConversao;
                this.vlForecast = sugestaoPedido.vlForecast;
                this.vlForecastMedio = sugestaoPedido.vlForecastMedio;
                this.vlLeadTime = sugestaoPedido.vlLeadTime;
                this.vlLeadTimeReal = sugestaoPedido.vlLeadTimeReal;
                this.vlModulo = sugestaoPedido.vlModulo;
                this.vlPackSugerido1 = sugestaoPedido.vlPackSugerido1;
                this.vlPipeline = sugestaoPedido.vlPipeline;
                this.vlQtdDiasEstoque = sugestaoPedido.vlQtdDiasEstoque;
                this.vlShelfLife = sugestaoPedido.vlShelfLife;
                this.vlSugestaoPedido = sugestaoPedido.vlSugestaoPedido;
                this.vlTotalPedidosAberto = sugestaoPedido.vlTotalPedidosAberto;
                this.blReturnSheet = sugestaoPedido.blReturnSheet;
                this.blCDConvertido = sugestaoPedido.blCDConvertido;
                this.TpCaixaFornecedor = sugestaoPedido.TpCaixaFornecedor;
                this.vlPesoLiquido = sugestaoPedido.vlPesoLiquido;
                this.vlSaldoOO = sugestaoPedido.vlSaldoOO;
                this.vlSaldoIW = sugestaoPedido.vlSaldoIW;
                this.vlSaldoIT = sugestaoPedido.vlSaldoIT;
                this.idCD = sugestaoPedido.idCD;
                this.vlTipoReabastecimento = sugestaoPedido.vlTipoReabastecimento;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define um valor que indica se pode zerar item. (Não vem da tabela SugestaoPedido. Origem: alçada)
        /// </summary>
        public bool blZerarItem { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se pode alterar percentual. (Não vem da tabela SugestaoPedido. Origem: alçada)
        /// </summary>
        public bool blAlterarPercentual { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se pode alterar informação de estoque. (Não vem da tabela SugestaoPedido. Origem: alçada)
        /// </summary>
        public bool blAlterarInformacaoEstoque { get; set; }

        /// <summary>
        /// Obtém ou define o limite inferior para o qtdPackCompra.
        /// </summary>
        public decimal vlLimiteInferior { get; set; }

        /// <summary>
        /// Obtém ou define o limite superior para o qtdPackCompra.
        /// </summary>
        public decimal vlLimiteSuperior { get; set; }

        /// <summary>
        /// Obtém ou define o valor de estoque original, antes de alguma alteração em tela.
        /// </summary>
        /// <remarks>Possui underscore por causa do change-tracker.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        //// TODO: Alterar o nome para VlEstoqueOriginal e resolver na serialização da web api a necessidade do changetracker da nomenclatura Original_vlEstoque.
        public decimal Original_vlEstoque { get; set; }

        /// <summary>
        /// Obtém ou define o valor de qtd pack compra, antes de alguma alteração em tela.
        /// </summary>
        /// <remarks>Possui underscore por causa do change-tracker.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        //// TODO: Alterar o nome para VlEstoqueOriginal e resolver na serialização da web api a necessidade do changetracker da nomenclatura Original_qtdPackCompra.
        public int Original_qtdPackCompra { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se a grade de sugestões para este item está aberta.
        /// </summary>
        public bool GradeSugestoesAberta { get; set; }

        /// <summary>
        /// Obtém um valor que indica se o campo de edição do valor do estoque estará habilitado para edição, caso visível.
        /// </summary>
        public bool HabilitaAlteracaoEstoque
        {
            get
            {
                return (this.cdOrigemCalculo != TipoOrigemCalculo.Manual) &&
                    (this.cdOrigemCalculo != TipoOrigemCalculo.Sgp || (this.cdOrigemCalculo == TipoOrigemCalculo.Sgp && this.blAlterarInformacaoEstoque));
            }
        }

        /// <summary>
        /// Obtém ou define um valor que indica se possui alçada configurada.
        /// </summary>
        public bool PossuiAlcada { get; set; }

        /// <summary>
        /// Obtém o peso, conforme a configuração do item (se é DSD ou cdConvertido e tipo caixa fornecedor variável, é peso líquido; caso contrário é peso bruto)
        /// </summary>
        public decimal? PesoLiquidoOuBruto
        {
            get
            {
                return this.ObterPeso();
            }
        }

        /// <summary>
        /// Obtém a unidade de compra (caixa ou kg)
        /// </summary>
        public TipoCaixaFornecedor UnidadeCompra
        {
            get
            {
                if (this.IsDsdOuConvertido())
                {
                    return this.TpCaixaFornecedor;
                }

                return this.qtVendorPackage == 1 ? TipoCaixaFornecedor.KgOuUnidade : TipoCaixaFornecedor.Caixa;
            }
        }

        /// <summary>
        /// Obtém a unidade de compra qtdPackCompraAlterado
        /// </summary>
        public bool qtdPackCompraAlterado { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Calcula o valor de qtdPackCompra a partir do vlEstoque informado.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>O qtdPackCompra calculado.</returns>
        public static SugestaoPedidoModel Recalcular(SugestaoPedidoModel sugestaoPedido)
        {
            var result = sugestaoPedido.Clone() as SugestaoPedidoModel;
            //// Validação de estoque compara (o estoque informado + a média do que parece estar dentro de pedidos em aberto) contra o forecast
            //// e determina se está muito acima ou abaixo do sugerido
            decimal vlPack = sugestaoPedido.ObterPack();

            result.vlPipeline = Math.Round(result.vlEstoque + (result.vlTotalPedidosAberto * result.qtVendorPackage * (decimal)result.vlFatorConversao), 3);
            result.vlSugestaoPedido = Math.Max(0, result.vlForecast - result.vlPipeline + result.vlEstoqueSegurancaQtd);
            decimal vlPackSugerido1 = 0;

            if (result.vlSugestaoPedido > 0)
            {
                vlPackSugerido1 = (result.vlFatorConversao == 0) ? Math.Round(result.vlSugestaoPedido / vlPack, 3) : Math.Round(result.vlSugestaoPedido / vlPack / (decimal)result.vlFatorConversao, 3);
            }

            result.vlPackSugerido1 = Convert.ToInt32(vlPackSugerido1);
            var vlPackSuperior = Math.Ceiling(vlPackSugerido1);
            var vlPackInferior = Math.Floor(vlPackSugerido1);

            var calcVlDifPackSuperior = Math.Abs((vlPackSuperior * vlPack * (decimal)result.vlFatorConversao) - result.vlSugestaoPedido);
            var calcVlDifPackInferior = Math.Abs((vlPackInferior * vlPack * (decimal)result.vlFatorConversao) - result.vlSugestaoPedido);

            if (result.qtVendorPackage != 1)
            {
                result.qtdPackCompra = Convert.ToInt32((calcVlDifPackSuperior >= calcVlDifPackInferior) ? vlPackInferior : vlPackSuperior);
            }
            else
            {
                result.qtdPackCompra = Convert.ToInt32((calcVlDifPackSuperior > calcVlDifPackInferior) ? (vlPackInferior * vlPack) : (vlPackSuperior * vlPack));
            }

            return result;
        }

        /// <summary>
        /// Cria uma cópia desta sugestão.
        /// </summary>
        /// <returns>
        /// A cópia.
        /// </returns>
        public object Clone()
        {
            SugestaoPedidoModel result = new SugestaoPedidoModel(this);

            result.blZerarItem = this.blZerarItem;
            result.blAlterarPercentual = this.blAlterarPercentual;
            result.blAlterarInformacaoEstoque = this.blAlterarInformacaoEstoque;
            result.vlLimiteInferior = this.vlLimiteInferior;
            result.vlLimiteSuperior = this.vlLimiteSuperior;
            result.PossuiAlcada = this.PossuiAlcada;
            result.GradeSugestoesAberta = this.GradeSugestoesAberta;
            result.Original_qtdPackCompra = this.Original_qtdPackCompra;
            result.Original_vlEstoque = this.Original_vlEstoque;

            return result;
        }

        /// <summary>
        /// Calcula os limites inferior e superior disponíveis considerando módulo (peso).
        /// </summary>
        /// <returns>Os limites disponiveis.</returns>
        public RangeValue<decimal> CalcularLimitesDisponiveis()
        {
            // Development 5127:#15 Ao exibir min/max na mensagem de validação da alçada, acatar regras do multiplo de peso.            
            decimal vlModuloRounded = this.ObterPeso();
            vlModuloRounded = vlModuloRounded == 0 ? 1 : vlModuloRounded;
            vlModuloRounded = Math.Max(Math.Round(vlModuloRounded, 0, MidpointRounding.AwayFromZero), 1m);

            decimal inferior = Math.Ceiling(vlLimiteInferior / vlModuloRounded) * vlModuloRounded;
            decimal superior = Math.Floor(vlLimiteSuperior / vlModuloRounded) * vlModuloRounded;

            return new RangeValue<decimal>() { StartValue = Math.Min(inferior, superior), EndValue = Math.Max(inferior, superior) };
        }
        #endregion
    }
}
