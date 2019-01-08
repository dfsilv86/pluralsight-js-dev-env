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
    public class AtLeastOneMustBeInformedSpecTest
    {
        [Test]
        public void IsSatisfiedBy_Null_False()
        {
            var target = new AtLeastOneMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(null);
            Assert.IsFalse(actual.Satisfied);
            Assert.IsNotNull(actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_NoProperties_False()
        {
            var target = new AtLeastOneMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(new { });
            Assert.IsFalse(actual.Satisfied);
            Assert.IsNotNull(actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AllPropertiesNotInformed_False()
        {
            var target = new AtLeastOneMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)null, e = DateTime.MinValue, f = (DateTime?)null, g = (long)0, h = (long?)null, i = (byte)0 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual("Pelo menos um dos campos deve ser informado: [TEXT NOT FOUND] a, [TEXT NOT FOUND] b, [TEXT NOT FOUND] c, [TEXT NOT FOUND] d, [TEXT NOT FOUND] e, [TEXT NOT FOUND] f, [TEXT NOT FOUND] g, [TEXT NOT FOUND] h ou [TEXT NOT FOUND] i.", actual.Reason);

            actual = target.IsSatisfiedBy(new { a = (int?)0 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual("Pelo menos um dos campos deve ser informado: [TEXT NOT FOUND] a.", actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_OneOrMorePropertiesInformed_True()
        {
            var target = new AtLeastOneMustBeInformedSpec();
            var actual = target.IsSatisfiedBy(new { a = "a", b = 0, c = (object)null, d = (int?)null, e = (long)0, f = (long?)null, g = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 1, c = (object)null, d = (int?)null, e = (long)0, f = (long?)null, g = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = new object(), d = (int?)null, e = (long)0, f = (long?)null, g = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)1, e = (long)0, f = (long?)null, g = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)0, e = DateTime.Now, f = (long)0, g = (long?)null, h = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)null, e = DateTime.Now, f = (long)0, g = (long?)null, h = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)null, e = (long)1, f = (long?)null, g = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)null, e = (long)0, f = (long?)1, g = (byte)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = "", b = 0, c = (object)null, d = (int?)null, e = (long)0, f = (long?)null, g = (byte)1 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (bool?)false, b = (bool?)true });
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AllowZeroes_True()
        {
            var target = new AtLeastOneMustBeInformedSpec(true);

            var actual = target.IsSatisfiedBy(new { a = (long?)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (long)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int?)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int)0 });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (byte)0 });
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_DontAllowZeroes_False()
        {
            var target = new AtLeastOneMustBeInformedSpec(false);

            var actual = target.IsSatisfiedBy(new { a = (long?)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (long)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int?)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (int)0 });
            Assert.IsFalse(actual.Satisfied);

            actual = target.IsSatisfiedBy(new { a = (byte)0 });
            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NullsDontAllowZeroes_Mixed()
        {
            var target = new AtLeastOneMustBeInformedSpec(true);

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
