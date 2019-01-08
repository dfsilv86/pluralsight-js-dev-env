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
    public class AllMustBeInformedSpecTest
    {

        [Test]
        public void IsSatisfiedBy_Null_False()
        {
            var target = new AllMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(null);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(string.Empty), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_NoProperties_False()
        {
            var target = new AllMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(new { });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(string.Empty), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AnyPropertiesNotInformed_False()
        {
            var target = new AllMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(new { a = "a", b = 2, c = (object)3, d = (int?)4, e = DateTime.Now, f = (DateTime?)null } );
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("f")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = "a", b = 2, c = (object)3, d = (int?)4, e = DateTime.MinValue, });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("e")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = "a", b = 2, c = (object)3, d = (int?)0});
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("d")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = "a", b = 2, c = (object)null });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("c")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = "a", b = 0 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("b")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = "" });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("a")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = (int?)0 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("a")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = (byte?)null });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformedSingular.With(GlobalizationHelper.GetText("a")), actual.Reason);

            actual = target.IsSatisfiedBy(new { a = (byte?)null, b = "" });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.AllMustBeInformed.With(GlobalizationHelper.GetText("a") + " e " + GlobalizationHelper.GetText("b")), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AllPropertiesInformed_True()
        {
            var target = new AllMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(new { a = "a"});
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "a", b = 1});
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "a", b = 1, c = new object() });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "a", b = 1, c = new object(), d = (int?)1 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "a", b = 1, c = new object(), d = (int?)1, e = DateTime.Now });
            Assert.IsTrue(actual.Satisfied);
        }


        [Test]
        public void IsSatisfiedBy_AllowZeroes_True()
        {
            var target = new AllMustBeInformedSpec(true);

            var actual = target.IsSatisfiedBy(new { a = (long?)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (long)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int?)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int)0 });
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_DontAllowZeroes_False()
        {
            var target = new AllMustBeInformedSpec(false);

            var actual = target.IsSatisfiedBy(new { a = (long?)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (long)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int?)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int)0 });
            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NullsDontAllowZeroes_Mixed()
        {
            var target = new AllMustBeInformedSpec(true);

            var actual = target.IsSatisfiedBy(new { a = (long?)null });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (long)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int?)null });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int)0 });
            Assert.IsTrue(actual.Satisfied);
        }
    }
}
