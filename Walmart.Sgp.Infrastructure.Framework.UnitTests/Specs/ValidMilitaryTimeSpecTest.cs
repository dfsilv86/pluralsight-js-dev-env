using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class ValidMilitaryTimeSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ValidTimes_Satisfied()
        {
            var target = new ValidMilitaryTimeSpec();

            Assert.IsTrue(target.IsSatisfiedBy(0));
            Assert.IsTrue(target.IsSatisfiedBy(1));
            Assert.IsTrue(target.IsSatisfiedBy(10));
            Assert.IsTrue(target.IsSatisfiedBy(59));
            Assert.IsTrue(target.IsSatisfiedBy(100));
            Assert.IsTrue(target.IsSatisfiedBy(159));
            Assert.IsTrue(target.IsSatisfiedBy(1000));
            Assert.IsTrue(target.IsSatisfiedBy(1059));
            Assert.IsTrue(target.IsSatisfiedBy(2359));
        }

        [Test]
        public void IsSatisfiedBy_InvalidTimes_NotSatisfied()
        {
            var target = new ValidMilitaryTimeSpec();

            Assert.IsFalse(target.IsSatisfiedBy(-1));
            Assert.IsFalse(target.IsSatisfiedBy(60));
            Assert.IsFalse(target.IsSatisfiedBy(99));
            Assert.IsFalse(target.IsSatisfiedBy(160));
            Assert.IsFalse(target.IsSatisfiedBy(199));
            Assert.IsFalse(target.IsSatisfiedBy(1060));
            Assert.IsFalse(target.IsSatisfiedBy(1099));
            Assert.IsFalse(target.IsSatisfiedBy(2360));
            Assert.IsFalse(target.IsSatisfiedBy(2399));
            Assert.IsFalse(target.IsSatisfiedBy(2400));
            Assert.IsFalse(target.IsSatisfiedBy(2430));
            Assert.IsFalse(target.IsSatisfiedBy(2460));
            Assert.IsFalse(target.IsSatisfiedBy(2499));
            Assert.IsFalse(target.IsSatisfiedBy(3000));
            Assert.IsFalse(target.IsSatisfiedBy(3030));
            Assert.IsFalse(target.IsSatisfiedBy(3060));
            Assert.IsFalse(target.IsSatisfiedBy(3099));
            Assert.IsFalse(target.IsSatisfiedBy(5900));
            Assert.IsFalse(target.IsSatisfiedBy(5930));
            Assert.IsFalse(target.IsSatisfiedBy(5960));
            Assert.IsFalse(target.IsSatisfiedBy(5999));
        }
    }
}
