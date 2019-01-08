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
    public class SpecExtensionsTest
    {
        [Test]
        public void SpecExtensions_IEnumerableIsSatisfiedBy_Ok()
        {
            int[] values = new int[] { 0, 1, 1, 3, 5, 8, 13, 21, 34, 55, 89, 144 };

            var result = values.IsSatisfiedBy(new TestSpec()).ToArray();

            Assert.AreEqual(6, result.Length);

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(1, result[2]);
            Assert.AreEqual(3, result[3]);
            Assert.AreEqual(5, result[4]);
            Assert.AreEqual(8, result[5]);
        }

        [Test]
        public void SpecExtensions_IEnumerableIsNotSatisfiedBy_Ok()
        {
            int[] values = new int[] { 0, 1, 1, 3, 5, 8, 13, 21, 34, 55, 89, 144 };

            var result = values.IsNotSatisfiedBy(new TestSpec()).ToArray();

            Assert.AreEqual(6, result.Length);

            Assert.AreEqual(13, result[0]);
            Assert.AreEqual(21, result[1]);
            Assert.AreEqual(34, result[2]);
            Assert.AreEqual(55, result[3]);
            Assert.AreEqual(89, result[4]);
            Assert.AreEqual(144, result[5]);
        }

        public class TestSpec : SpecBase<int>
        {
            public override SpecResult IsSatisfiedBy(int target)
            {
                if (target > 10)
                {
                    return NotSatisfied("Foo");
                }

                return Satisfied();
            }
        }
    }
}
