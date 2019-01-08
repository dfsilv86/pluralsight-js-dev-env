using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class SpecResultTest
    {
        [Test]
        public void BoolOperator_NoSatisfied_False()
        {
            var target = new SpecResult(false, "teste");
            Assert.IsFalse(target);
        }

        [Test]
        public void BoolOperator_Satisfied_True()
        {
            var target = new SpecResult(true);
            Assert.IsTrue(target);
        }

        [Test]
        public void StringOperator_Reason_String()
        {
            var target = new SpecResult(false, "not satisfied by reason");
            Assert.AreEqual("not satisfied by reason", (string)target);
        }
    }
}
