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
    public class NotSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UnderlyingTrue_False()
        {
            var target = new AllMustBeInformedSpec().Not("NOT REASON");
            var actual = target.IsSatisfiedBy(new { id = 1 });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual("NOT REASON", actual.Reason);
        }
    }
}
