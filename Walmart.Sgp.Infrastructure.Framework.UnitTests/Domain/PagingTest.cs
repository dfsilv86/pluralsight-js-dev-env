using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Framework")]
    public class PagingTest
    {
        [Test]
        public void Constructor_OrderBy_NoOffsetAndLimit()
        {
            var target = new Paging("TEST");
            Assert.AreEqual("TEST", target.OrderBy);
            Assert.AreEqual(0, target.Offset);
            Assert.AreEqual(2147483646, target.Limit);
        }
    }
}
