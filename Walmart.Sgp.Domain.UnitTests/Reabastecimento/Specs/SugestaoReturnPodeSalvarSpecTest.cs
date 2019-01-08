using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class SugestaoReturnPodeSalvarSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SugestaoReturnSheetComDataFinalDeReturnSheetAmanha_Satisfied()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new SugestaoReturnPodeSalvarSpec();
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ReturnSheet = new ReturnSheet()
                        {
                            DhFinalReturn = amanha
                        }
                    }
                }
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SugestaoReturnSheetComDataFinalDeReturnSheetOntem_NotSatisfied()
        {
            var ontem = DateTime.Now.AddDays(-1);

            var target = new SugestaoReturnPodeSalvarSpec();
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ReturnSheet = new ReturnSheet()
                        {
                            DhFinalReturn = ontem
                        }
                    }
                }
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.CantSaveRSclosed, actual.Reason);
        }
    }
}
