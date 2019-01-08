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
    class SugestoesReturnSheetNaoPodeSerNullSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ListaComUmaReturnSheet_Satisfied()
        {
            var target = new SugestoesReturnSheetNaoPodeSerNullSpec();
            var actual = target.IsSatisfiedBy(new List<SugestaoReturnSheet>() { new SugestaoReturnSheet() { Id = 1 } });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ListaDeReturnSheetVazia_NotSatisfied()
        {
            var target = new SugestoesReturnSheetNaoPodeSerNullSpec();
            var actual = target.IsSatisfiedBy(new List<SugestaoReturnSheet>());

            Assert.IsFalse(actual.Satisfied);

            Assert.AreEqual(Texts.NoItensToBeSaved, actual.Reason);
        }
    }
}
