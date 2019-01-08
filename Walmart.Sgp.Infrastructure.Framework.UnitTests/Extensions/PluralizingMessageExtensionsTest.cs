using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Extensions
{
    [TestFixture]
    [Category("Framework")]
    public class PluralizingMessageExtensionsTest
    {
        [Test]
        public void PluralizedMessage_One_Singular()
        {
            var result = 1.PluralizedMessage("Teste");

            Assert.AreEqual(GlobalizationHelper.GetText("TesteSingular"), result);
        }

        [Test]
        public void PluralizedMessage_MoreThanOne_Plural()
        {
            var result = 2.PluralizedMessage("Teste");

            Assert.AreEqual(GlobalizationHelper.GetText("TestePlural"), result);
        }

        [Test]
        public void PluralizedMessage_Zero_Plural()
        {
            var result = 0.PluralizedMessage("Teste");

            Assert.AreEqual(GlobalizationHelper.GetText("TestePlural"), result);
        }
    }
}
