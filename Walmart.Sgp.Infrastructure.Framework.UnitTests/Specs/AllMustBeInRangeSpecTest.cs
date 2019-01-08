using System;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class AllMustBeInRangeSpecTest
    {
        [Test]
        public void IsSatisfiedBy_OutOfRange_False()
        {
            var target = new AllMustBeInRangeSpec(1, 4);

            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 0,
                prop2 = 1
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.IsTrue(actual.Reason.Contains(GlobalizationHelper.GetText("prop1")));
        }

        [Test]
        public void IsSatisfiedBy_OutOfRangeMultiple_False()
        {
            var target = new AllMustBeInRangeSpec(1.5, 1.9);

            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 1.45,
                prop2 = 1.8,
                prop3 = 1.91

            });

            Assert.IsFalse(actual.Satisfied);
            Assert.IsTrue(actual.Reason.Contains(GlobalizationHelper.GetText("prop1")));
            Assert.IsTrue(actual.Reason.Contains(GlobalizationHelper.GetText("prop3")));
        }
        
        [Test]
        public void IsSatisfiedBy_InRange_True()
        {
            var target = new AllMustBeInRangeSpec(1, 9);

            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 1,
                prop2 = 2,                
                prop3 = 3,
                prop4 = new int?()

            });

            Assert.IsTrue(actual.Satisfied);            
        }
    }
}