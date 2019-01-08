using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaDevePermitirSugestaoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_Manual_Ok()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Manual
            };

            Assert.IsTrue(target.IsSatisfiedBy(model));
        }

        [Test]
        public void IsSatisfiedBy_SGPNaAlcada_Ok()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 6,
                vlLimiteSuperior = 12,
                qtdPackCompra = 8,
                qtdPackCompraOriginal = 2, //adicionado para não cair no novo cenário
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1
            };

            Assert.IsTrue(target.IsSatisfiedBy(model));
        }

        [Test]
        public void IsSatisfiedBy_ZerarSemAlcadaOriginalDiferenteZero_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 6,
                vlLimiteSuperior = 12,
                qtdPackCompra = 0,
                vlModulo = 1,
                blZerarItem = false,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.AreEqual(Texts.CannotZeroQtdPackCompra, result.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ZerarSemAlcadaOriginalIgualZero_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 6,
                vlLimiteSuperior = 12,
                qtdPackCompra = 0,
                vlModulo = 1,
                blZerarItem = false,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                qtdPackCompraOriginal = 0
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_ZerarComAlcada_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 6,
                vlLimiteSuperior = 12,
                qtdPackCompra = 0,
                vlModulo = 1,
                blZerarItem = true,
                blAlterarPercentual = true,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_ForaDoLimiteDaAlcada_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 50,
                vlLimiteSuperior = 60,
                qtdPackCompra = 90,
                vlModulo = 1,
                blAlterarPercentual = true,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.StartsWith(Texts.QtdPackCompraIsOutsideRange.Substring(0, 70)));
        }

        /// <summary>
        /// Development 5127:#15 Ao exibir min/max na mensagem de validação da alçada, acatar regras do multiplo de peso.
        /// </summary>
        [Test]
        public void IsSatisfiedBy_ForaDoLimiteDaAlcadaPesoDiferenteDe1_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 15,
                vlLimiteSuperior = 45,
                qtdPackCompra = 50,
                qtdPackCompraOriginal = 2,
                vlModulo = 10,
                blAlterarPercentual = true,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 20"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 40"));
        }

        /// <summary>
        /// Development 5127:#15 Ao exibir min/max na mensagem de validação da alçada, acatar regras do multiplo de peso.
        /// </summary>
        [Test]
        public void IsSatisfiedBy_ForaDoLimiteDaAlcadaPesoDiferenteDe1ComArredonamento_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 6,
                vlLimiteSuperior = 56,
                qtdPackCompra = 60,
                qtdPackCompraOriginal = 2,
                vlModulo = 5.62m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 6"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 54"));
        }

        /// <summary>
        /// Development 5127:#15 Ao exibir min/max na mensagem de validação da alçada, acatar regras do multiplo de peso.
        /// </summary>
        [Test]
        public void IsSatisfiedBy_ForaDoLimiteDaAlcadaPesoInferior1ComArredonamento_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 2,
                vlLimiteSuperior = 56,
                qtdPackCompra = 60,
                qtdPackCompraOriginal = 2,
                vlModulo = 0.42m,
                blAlterarPercentual = true,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 2"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 56"));
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaEUnidadeCompraCaixaComPackCompra1_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5m,
                vlLimiteSuperior = 1.5m,
                qtdPackCompra = 1,
                qtdPackCompraOriginal = 0,
                vlModulo = 0.42m,
                blAlterarPercentual = true,
                qtVendorPackage = 0,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalDoisEUnidadeCompraCaixaComPackCompra1_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 2,
                vlLimiteSuperior = 56,
                qtdPackCompra = 1,
                qtdPackCompraOriginal = 2,
                vlModulo = 0.42m,
                blAlterarPercentual = true,
                qtVendorPackage = 0,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 2"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 56"));
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaEUnidadeCompraCaixaComPackCompra5_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5m,
                vlLimiteSuperior = 1.5m,
                qtdPackCompra = 5,
                qtdPackCompraOriginal = 0,
                vlModulo = 0.042m,
                blAlterarPercentual = true,
                qtVendorPackage = 0,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 1"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 1"));
        }


        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaECdConvertidoUnidadeCompraCaixaComPackCompra5_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5m,
                vlLimiteSuperior = 1.5m,
                qtdPackCompra = 5,
                qtdPackCompraOriginal = 0,
                vlModulo = 0.042m,
                blAlterarPercentual = true,
                qtVendorPackage = 0,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                blCDConvertido = true
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 1"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 1"));
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaEUnidadeCompraKGComKGAMais_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5m,
                vlLimiteSuperior = 1.5m,
                qtdPackCompra = 2,
                qtdPackCompraOriginal = 0,
                vlModulo = 1.0m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 1"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 1"));
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaEUnidadeCompraKGValorSuperior_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 4,
                vlLimiteSuperior = 8,
                qtdPackCompra = 10,
                qtdPackCompraOriginal = 0,
                vlModulo = 5m,
                vlPesoLiquido = 5m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 5"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 5"));
        }


        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaEUnidadeCompraKGValorInferior_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 4,
                vlLimiteSuperior = 8,
                qtdPackCompra = 10,
                qtdPackCompraOriginal = 0,
                vlModulo = 5m,
                vlPesoLiquido = 5m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.IsTrue(result.Reason.Contains("Quantidade mínima permitida: 5"));
            Assert.IsTrue(result.Reason.Contains("Quantidade máxima permitida: 5"));
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalUmEUnidadeCompraEmKgEKG1_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 0.5m,
                vlLimiteSuperior = 1.0m,
                qtdPackCompra = 1,
                qtdPackCompraOriginal = 1,
                vlModulo = 1.0m,
                vlPesoLiquido = 1.0m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_QtdPackOriginalZeradaEUnidadeCompraEmKgEKG5_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 4.56m,
                vlLimiteSuperior = 5.89m,
                qtdPackCompra = 5,
                qtdPackCompraOriginal = 0,
                vlModulo = 4.52m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_LimiteInferior1EModuloInferiorA1_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 1,
                vlLimiteSuperior = 56,
                qtdPackCompra = 1,
                vlModulo = 0.42m,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaNaoPermiteAlterar_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                vlLimiteInferior = 50,
                vlLimiteSuperior = 60,
                qtdPackCompra = 90,
                vlModulo = 1,
                blAlterarPercentual = false,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.AreEqual(Texts.NoCompetenceDefined, result.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaComValorNaoMultiplo_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                qtVendorPackage = 1,
                qtdPackCompra = 3,
                vlModulo = 2
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsSatisfiedBy_QtdCompraZeradaEAlcadaPermiteZerarItemMasNaoPermiteAlterarPercentual_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = false,
                qtdPackCompra = 0,
                vlModulo = 1,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsSatisfiedBy_QtdCompraValidaEAlcadaNaoPermiteAlterarPercentualMasPermiteZerar_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = false,
                vlLimiteInferior = 50,
                vlLimiteSuperior = 60,
                qtdPackCompra = 55,
                vlModulo = 1,
                qtVendorPackage = 1
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
            Assert.AreEqual(Texts.NoCompetenceDefined, result.Reason);
        }

        #region Validação de multiplo de peso bruto, liquido, ou nao precisa validar multiplo

        // qtVendorPackage 1, blCDConvertido 1, tpCaixaFornecedor F -> nao valida multiplo
        [Test]
        public void IsSatisfiedBy_QT1CD1TPF_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 3,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 1, blCDConvertido 1, tpCaixaFornecedor V -> valida multiplo de peso liquido
        [Test]
        public void IsSatisfiedBy_QT1CD1TPV_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 15,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 1, blCDConvertido 1, tpCaixaFornecedor V -> valida multiplo de peso liquido
        [Test]
        public void IsSatisfiedBy_QT1CD1TPV_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                qtdPackCompraOriginal = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 14
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        // qtVendorPackage 1, blCDConvertido 0, tpCaixaFornecedor F -> valida multiplo de peso bruto
        [Test]
        public void IsSatisfiedBy_QT1CD0TPF_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 14,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 1, blCDConvertido 0, tpCaixaFornecedor F -> valida multiplo de peso bruto
        [Test]
        public void IsSatisfiedBy_QT1CD0TPF_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 15,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        // qtVendorPackage 1, blCDConvertido 0, tpCaixaFornecedor V, sem reabastecimento -> valida multiplo de peso bruto
        [Test]
        public void IsSatisfiedBy_QT1CD0TPV_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 14,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 1, blCDConvertido 0, tpCaixaFornecedor V, reabastecimento Dsd -> valida multiplo de peso liquido
        [Test]
        public void IsSatisfiedBy_QT1CD0TPVDSD_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 15,
                qtdPackCompraOriginal = 2, //adicionado para não cair no novo cenário
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 1, blCDConvertido 0, tpCaixaFornecedor V, reabastecimento Dsd -> valida multiplo de peso liquido
        [Test]
        public void IsSatisfiedBy_QT1CD0TPVDSD_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 14,
                qtdPackCompraOriginal = 2, //adicionado para não cair no novo cenário
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        // qtVendorPackage 1, blCDConvertido 0, tpCaixaFornecedor V -> valida multiplo de peso bruto
        [Test]
        public void IsSatisfiedBy_QT1CD0TPV_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 15,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        // qtVendorPackage 2, blCDConvertido 1, tpCaixaFornecedor F -> nao valida multiplo
        [Test]
        public void IsSatisfiedBy_QT2CD1TPF_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 3,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 2, blCDConvertido 1, tpCaixaFornecedor V -> valida multiplo peso liquido
        [Test]
        public void IsSatisfiedBy_QT2CD1TPV_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 15,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 2, blCDConvertido 1, tpCaixaFornecedor V -> valida multiplo peso liquido
        [Test]
        public void IsSatisfiedBy_QT2CD1TPV_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 14,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }

        // qtVendorPackage 2, blCDConvertido 0, tpCaixaFornecedor F -> nao valida multiplo
        [Test]
        public void IsSatisfiedBy_QT2CD0TPF_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 3,
                qtdPackCompraOriginal = 2 //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 2, blCDConvertido 0, tpCaixaFornecedor V -> nao valida multiplo
        [Test]
        public void IsSatisfiedBy_QT2CD0TPV_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 3,
                qtdPackCompraOriginal = 2, //adicionado para não cair no novo cenário
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 2, blCDConvertido 0, tpCaixaFornecedor V, reabastecimento Dsd -> valida multiplo de peso liquido
        [Test]
        public void IsSatisfiedBy_QT2CD0TPVDSD_Satisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 15,
                qtdPackCompraOriginal = 2, //adicionado para não cair no novo cenário
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsTrue(result);
        }

        // qtVendorPackage 2, blCDConvertido 0, tpCaixaFornecedor V, reabastecimento Dsd -> valida multiplo de peso liquido
        [Test]
        public void IsSatisfiedBy_QT2CD0TPVDSD_NotSatisfied()
        {
            var target = new AlcadaDevePermitirSugestaoSpec();

            SugestaoPedidoModel model = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blZerarItem = true,
                blAlterarPercentual = true,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 9999,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 7,        // peso bruto
                vlPesoLiquido = 5,   // peso liquido
                qtdPackCompra = 14,
                qtdPackCompraOriginal = 2, //adicionado para não cair no novo cenário
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
            };

            var result = target.IsSatisfiedBy(model);

            Assert.IsFalse(result);
        }
        #endregion
    }
}
