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
    public class ReturnSheetCannotHavePastDatesSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ReturnSheetComDataFutura_Satisfied()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetCannotHavePastDatesSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha,
                DhFinalReturn = amanha.AddDays(1),
                DhInicioEvento = amanha.AddDays(2),
                DhFinalEvento = amanha.AddDays(4)
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComDataPassada_NotSatisfied()
        {
            var ontem = DateTime.Now.AddDays(-1);

            var target = new ReturnSheetCannotHavePastDatesSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = ontem.AddDays(1),
                DhFinalReturn = ontem,
                DhInicioEvento = ontem.AddDays(2),
                DhFinalEvento = ontem.AddDays(4)
            });

            Assert.IsFalse(actual.Satisfied);

            Assert.AreEqual(Texts.YouCantInformPastDates, actual.Reason);
        }
    }
}
