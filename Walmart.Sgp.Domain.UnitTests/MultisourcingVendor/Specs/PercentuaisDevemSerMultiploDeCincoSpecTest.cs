using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class PercentuaisDevemSerMultiploDeCincoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_MultiplosDeCinco_True()
        {
            var target = new PercentuaisDevemSerMultiploDeCincoSpec();
            var actual = target.IsSatisfiedBy(new decimal?[] { 10, 50 });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NaoMultiplosDeCinco_False()
        {
            var target = new PercentuaisDevemSerMultiploDeCincoSpec();
            var actual = target.IsSatisfiedBy(new decimal?[] { 10, 52 });

            Assert.AreEqual(Texts.MultipleFivePercentuals, actual.Reason);
        }
    }
}
