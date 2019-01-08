using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Globalization
{
    [TestFixture]
    [Category("Framework")]
    public class CultureInfoHelperTest
    {
        [Test]
        public void GetCultureInfoByCurrency_ValidIsoCurrencySimbol_CultureInfo()
        {
            var actual = CultureInfoHelper.GetCultureInfoByCurrency("BRL");
            Assert.AreEqual("pt-BR", actual.Name);

            actual = CultureInfoHelper.GetCultureInfoByCurrency("USD");
            Assert.AreEqual("en-US", actual.Name);
        }
    }
}
