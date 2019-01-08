using System;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class AllMustBeGreaterThanSpecTest
    {
        [Test]
        public void IsSatisfiedBy_LowerValue_False()
        {
            var target = new AllMustBeGraterThanSpec(1);
            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 0
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.IsTrue(actual.Reason.Contains("prop1"));
        }

        [Test]
        public void IsSatisfiedBy_NullableValue_True()
        {
            var target = new AllMustBeGraterThanSpec(0.0);
            var actual = target.IsSatisfiedBy(new
            {                
                prop1 = 1.8,
                prop2 = new double?(),
                prop3 = 2.0
            });

            Assert.IsTrue(actual.Satisfied);            
        }

        [Test]
        public void IsSatisfiedBy_ZeroValue_False()
        {
            var target = new AllMustBeGraterThanSpec(0.0);
            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 0.0
            });

            Assert.IsFalse(actual.Satisfied);            
            Assert.AreEqual(
                actual.Reason,
                Texts.AllMustBeGreaterThanSingular.With(GlobalizationHelper.GetText("prop1"), Texts.Zero));

        }

        [Test]
        public void IsSatisfiedBy_LowerValueMultiple_False()
        {
            var target = new AllMustBeGraterThanSpec(5);
            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 0,
                prop2 = 10,
                prop3 = 2
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.IsTrue(actual.Reason.Contains("prop1"));
            Assert.IsTrue(actual.Reason.Contains("prop3"));
            Assert.IsFalse(actual.Reason.Contains("prop2"));
        }

        [Test]
        public void IsSatisfiedBy_GreaterValue_True()
        {
            var target = new AllMustBeGraterThanSpec(5);
            var actual = target.IsSatisfiedBy(new
            {
                prop1 = 6,
                prop2 = 10,
                prop3 = 7
            });

            Assert.IsTrue(actual.Satisfied);            
        }
    }
}