using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class SugestaoReturnSheetNaoPodeTerExportacaoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ReturnSheetItemLoja_Satisfied()
        {
            var target = new SugestaoReturnSheetNaoPodeTerExportacaoSpec((x) => { return false; });
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                    {
                        ItemPrincipal = new ReturnSheetItemPrincipal()
                        {
                            ReturnSheet = new ReturnSheet()
                            {
                                Id = 1
                            }
                        }
                    }
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetItemLoja_NotSatisfied()
        {
            var target = new SugestaoReturnSheetNaoPodeTerExportacaoSpec((x) => { return true; });
            var actual = target.IsSatisfiedBy(new SugestaoReturnSheet()
            {
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ReturnSheet = new ReturnSheet()
                        {
                            Id = 1
                        }
                    }
                }
            });

            Assert.IsFalse(actual.Satisfied);

            Assert.AreEqual(Texts.CantChangeExportedReturnSheet, actual.Reason);
        }
    }
}
