using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class AllDatesMustBeGreaterThanNowSpecTest
    {
        [Test]
        public void AllDatesMustBeGreaterThanNowSpec_Specs_Satisfied()
        {
            var tomorrow = DateTime.Now.AddDays(1);
            var nextWeek = DateTime.Now.AddDays(7);

            var target = new AllDatesMustBeGreaterThanNowSpec();
            var actual = target.IsSatisfiedBy(new DateTime[] { tomorrow, nextWeek });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void AllDatesMustBeGreaterThanNowSpec_Specs_NotSatisfied()
        {
            var yesterday = DateTime.Now.AddDays(-1);
            var pastWeek = DateTime.Now.AddDays(-7);

            var target = new AllDatesMustBeGreaterThanNowSpec();
            var actual = target.IsSatisfiedBy(new DateTime[] { yesterday, pastWeek });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.YouCantInformPastDates, actual.Reason);
        }
    }
}
