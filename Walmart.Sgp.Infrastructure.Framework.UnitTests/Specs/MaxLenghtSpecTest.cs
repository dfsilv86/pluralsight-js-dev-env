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
    public class MaxLenghtSpecTest
    {
        [Test]
        public void MaxLenghtSpec_Specs_Satisfied()
        {
            var target = new MaxLenghtSpec(10);
            var actual = target.IsSatisfiedBy("MenorQDez");

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void MaxLenghtSpec_Specs_NotSatisfied()
        {
            var target = new MaxLenghtSpec(10);
            var actual = target.IsSatisfiedBy("Bem maior que dez");

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.MaxLenghtExceed, actual.Reason);
        }
    }
}
