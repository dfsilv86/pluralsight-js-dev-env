using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class MustRespectRangeSpecTest
    {
        [Test]
        public void IsSatisfiedBy_FirstEqualsThanSecondAllowEqualsFalse_False()
        {
            var target = new MustRespectRangeSpec();
            target.AllowEquals = false;
            var actual = target.IsSatisfiedBy(new { a = 1, b = 1 });            
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(actual.Reason, Texts.TheFieldMustNotBeGreaterOrEqualThanField.With(GlobalizationHelper.GetText("a"), GlobalizationHelper.GetText("b")));
        }

        [Test]
        public void IsSatisfiedBy_FirstEqualsThanSecondAllowEqualsTrue_True()
        {
            var target = new MustRespectRangeSpec();
            target.AllowEquals = true;
            var actual = target.IsSatisfiedBy(new { a = 1, b = 1 });
            Assert.IsTrue(actual.Satisfied);
            Assert.IsNull(actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_FirstGreaterThanSecond_False()
        {
            var target = new MustRespectRangeSpec();
            var actual = target.IsSatisfiedBy(new { a = 1, b = 0 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(actual.Reason, Texts.TheFieldMustNotBeGreaterThanField.With(GlobalizationHelper.GetText("a"), GlobalizationHelper.GetText("b")));
        }

        [Test]
        public void IsSatisfiedBy_FirstLowerThanSecond_True()
        {
            var target = new MustRespectRangeSpec();
            var actual = target.IsSatisfiedBy(new { a = 0, b = 1 });
            Assert.IsTrue(actual.Satisfied);
            Assert.IsNull(actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_OnlyOneProperty_True()
        {
            var target = new MustRespectRangeSpec();
            var actual = target.IsSatisfiedBy(new { a = 0 });
            Assert.IsTrue(actual.Satisfied);
            Assert.IsNull(actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_OnlyOneIComparableProperty_True()
        {            
            var target = new MustRespectRangeSpec();
            var actual = target.IsSatisfiedBy(new { a = 0, b = new Exception() });
            Assert.IsTrue(actual.Satisfied);
            Assert.IsNull(actual.Reason);
        }
    }
}
