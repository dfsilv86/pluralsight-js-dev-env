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
    public class DataValidaSpecTest
    {
        [Test]
        public void DataValidaSpec_Specs_Satisfied()
        {
            var tomorrow = DateTime.Now.AddDays(1);

            var target = new DataValidaSpec();
            var actual = target.IsSatisfiedBy(tomorrow);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void DataValidaSpec_Specs_NotSatisfied()
        {
            var invalidDate = new DateTime(1650,01,01);

            var target = new DataValidaSpec();
            var actual = target.IsSatisfiedBy(invalidDate);

            Assert.IsFalse(actual.Satisfied);

            Assert.AreEqual(Texts.DateMustBeGreaterThan1753, actual.Reason);
        }
    }
}
