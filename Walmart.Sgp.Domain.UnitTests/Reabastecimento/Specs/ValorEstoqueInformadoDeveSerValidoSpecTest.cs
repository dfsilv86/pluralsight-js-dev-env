using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class ValorEstoqueInformadoDeveSerValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_EstoqueNaoAlterado_Satisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = true,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5M,
                vlLimiteSuperior = 1.5M,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 1,
                Original_vlEstoque = 1,
                vlForecast = 10,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_QtVendorPackageIgualAUmForecastEstoqueValido_Satisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = true,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5M,
                vlLimiteSuperior = 1.5M,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 1,
                vlForecast = 10,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_QtVendorPackageDiferenteDeUm_Satisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = true,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5M,
                vlLimiteSuperior = 1.5M,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 2,
                vlEstoque = 1,
                vlForecast = 20,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_OrigemManual_Satisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = false,
                cdOrigemCalculo = TipoOrigemCalculo.Manual,
                vlLimiteInferior = 2,
                vlLimiteSuperior = 3,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 2,
                vlForecast = 10,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_OriginalDiferenteDeZeroAlcadaNaoPermiteZerar_NotSatisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = false,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5M,
                vlLimiteSuperior = 1.5M,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 5,
                vlForecast = 10,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5,
                Original_qtdPackCompra = 1,
                qtdPackCompraOriginal = 2
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.AreEqual(Texts.CannotZeroQtdPackCompra, result.Reason);
        }

        [Test]
        public void IsSatisfiedBy_OriginalIgualZeroAlcadaNaoPermiteZerar_Satisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = false,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5M,
                vlLimiteSuperior = 1.5M,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 5,
                vlForecast = 10,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5,
                Original_qtdPackCompra = 1,
                qtdPackCompraOriginal = 0
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_NaoTemAlcadaMinima_NotSatisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = false,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 2,
                vlLimiteSuperior = 4,
                qtdPackCompra = 1,
                qtdPackCompraOriginal = 3,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 2,
                vlForecast = 10,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.StartsWith(Texts.QtdPackCompraIsOutsideRange.Substring(0, 70)));
        }

        [Test]
        public void IsSatisfiedBy_NaoTemAlcadaMaxima_NotSatisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // vlEstoque, vlLimiteInferior e vlLimiteSuperior são em unidades de vlFatorConversao; vlForecast é absoluto
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blAlterarInformacaoEstoque = true,
                blZerarItem = false,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 0.5M,
                qtdPackCompra = 1,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                vlEstoque = 1,
                vlForecast = 15,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.StartsWith(Texts.QtdPackCompraIsOutsideRange.Substring(0, 70)));
        }

        [Test]
        public void IsSatisfiedBy_ValidarQtdSolicitada_NotSatisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            // estoque de 1kg com forecast alto e pouco pedido em aberto implica em sugestao maior que a alçada
            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                blAlterarInformacaoEstoque = true,
                blZerarItem = true,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5M,
                vlLimiteSuperior = 1.5M,
                qtdPackCompra = 3,
                vlModulo = 2,
                blAlterarPercentual = true,
                qtVendorPackage = 2,
                vlEstoque = 1,
                vlForecast = 20,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 1,
                vlPesoLiquido = 2
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsSatisfiedBy_ValidarQtdSolicitada_Satisfied()
        {
            Func<GradeSugestao> gradeSugestaoDelegate = () => new GradeSugestao { vlHoraFinal = 2359 };

            var target = new ValorEstoqueInformadoDeveSerValidoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                blAlterarInformacaoEstoque = true,
                blZerarItem = true,
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 3,
                vlLimiteSuperior = 3,
                qtdPackCompra = 2,
                qtVendorPackage = 1,
                vlEstoque = 1,
                vlForecast = 20,
                vlTotalPedidosAberto = 1,
                vlFatorConversao = 5,
                vlPesoLiquido = 2
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }
    }
}
