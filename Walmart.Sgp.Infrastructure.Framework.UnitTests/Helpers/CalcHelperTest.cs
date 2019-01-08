using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Helpers
{
    [TestFixture]
    [Category("Framework")]
    public class CalcHelperTest
    {
        [Test]
        public void CustomDivisionByZero()
        {
            var result = CalcHelper.CustomDivision(0, 0);

            Assert.AreEqual(0, result);
        }
    }
}
