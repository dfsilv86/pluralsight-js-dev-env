using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class ValidaQtdSolicitadaXUnidadeCompraSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTest_V()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(false);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "V" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdLoja = 30
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTest_F()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(false);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "F" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdLoja = 3
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsNotSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTest_V()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(false);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet() 
            {
                ItemLoja = new ReturnSheetItemLoja(){
                    ItemPrincipal = new ReturnSheetItemPrincipal() {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "V"}
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdLoja = 10
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.QtdMultipleWeight, actual.Reason);
        }

        [Test]
        public void IsNotSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTest_F()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(false);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "F" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdLoja = -1
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.QtdInformedMustBeGreaterThanZero, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTestRA_V()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(true);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "V" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdRA = 30
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTestRA_F()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(true);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "F" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdRA = 3
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsNotSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTestRA_V()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(true);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "V" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdRA = 10
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.QtdMultipleWeight, actual.Reason);
        }

        [Test]
        public void IsNotSatisfiedBy_ValidarQtdSolicitadaXUnidadeCompraSpecTestRA_F()
        {
            var target = new ValidaQtdSolicitadaXUnidadeCompraSpec(true);
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe() { TpCaixaFornecedor = "F" }
                    }
                },
                vlPesoLiquidoItemCompra = 3,
                QtdRA = -1
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.QtdInformedMustBeGreaterThanZero, actual.Reason);
        }
    }
}
