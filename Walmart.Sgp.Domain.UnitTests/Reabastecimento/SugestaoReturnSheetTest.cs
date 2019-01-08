using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain"), Category("SugestaoReturnSheet")]
    public class SugestaoReturnSheetTest
    {
        [Test]
        public void SugestaoReturnSheetFinalizadaComLoja_SugestaoReturnSheetComReturnSheetComDataFinalAnteriorAHoje_SugestaoReturnSheetFinalizada()
        {
            var model = new SugestaoReturnSheet();
            model.ItemLoja = new ReturnSheetItemLoja()
            {
                ItemPrincipal = new ReturnSheetItemPrincipal()
                {
                    ReturnSheet = new ReturnSheet()
                    {
                        DhFinalReturn = DateTime.Now.AddDays(-1)
                    }
                }
            };

            Assert.IsTrue(model.Finalizada);
        }

        [Test]
        public void SugestaoReturnSheetFinalizadaSemLoja_SugestaoReturnSheetSemDados_SugestaoReturnSheetNaoFinalizada()
        {
            var model = new SugestaoReturnSheet();

            Assert.IsFalse(model.Finalizada);
        }

        [Test]
        public void SugestaoReturnSheetFinalizadaComLoja_SugestaoReturnSheetComReturnSheetComDataFinalPosteriorAHoje_SugestaoReturnSheetNaoFinalizada()
        {
            var model = new SugestaoReturnSheet();
            model.ItemLoja = new ReturnSheetItemLoja()
            {
                ItemPrincipal = new ReturnSheetItemPrincipal()
                {
                    ReturnSheet = new ReturnSheet()
                    {
                        DhFinalReturn = DateTime.Now.AddDays(1)
                    }
                }
            };

            Assert.IsFalse(model.Finalizada);
        }
    }
}
