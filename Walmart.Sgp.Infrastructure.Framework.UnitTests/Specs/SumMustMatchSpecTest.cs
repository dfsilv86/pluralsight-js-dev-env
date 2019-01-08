using NUnit.Framework;
using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    public class SumMustMatchSpecTest
    {
        [Test]
        public void SomaBate()
        {
            var target = new SumMustMatchSpec(100);
            var actual = target.IsSatisfiedBy(new List<int> { 50, 50 });
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void SomaNaoBate()
        {
            var target = new SumMustMatchSpec(100);
            var actual = target.IsSatisfiedBy(new List<Double> { 33.3, 33.3, 33.3 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SumDoesntMatch.With(100), actual.Reason);
        }

        [Test]
        public void PropriedadeNaoIEnumerable()
        {
            var target = new SumMustMatchSpec(100);
            var actual = target.IsSatisfiedBy(10);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.PropIsNotIEnumerable, actual.Reason);
        }
    }
}
