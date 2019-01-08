using System;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests
{
    [TestFixture]
    [Category("Framework")]
    public class GlobalizationHelperTest
    {
        [Test]
        public void GetText_NullKey_Exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                GlobalizationHelper.GetText(null);
            });
        }

        [Test]
        public void GetText_KeyDoesNotExists_NotFoundText()
        {
            Assert.AreEqual("[TEXT NOT FOUND] ___DOES_NOT_EXISTS___", GlobalizationHelper.GetText("___DOES_NOT_EXISTS___"));
        }

        [Test]
        public void GetText_KeyExists_Translated()
        {
            Assert.AreEqual("Nome", GlobalizationHelper.GetText("Name"));
        }

        [Test]
        public void GetText_NullMainKey_Exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                GlobalizationHelper.GetText(null, "Name");
            });
        }

        [Test]
        public void GetText_KeyAndFallbackDoesNotExists_NotFoundText()
        {
            Assert.AreEqual("[TEXT NOT FOUND] ___DOES_NOT_EXISTS___", GlobalizationHelper.GetText("___DOES_NOT_EXISTS___", "___DOES_NOT_EXISTS___"));
        }

        [Test]
        public void GetText_KeyExistsFallbackIgnored_Translated()
        {
            Assert.AreEqual("Nome", GlobalizationHelper.GetText("Name", "User"));
        }

        [Test]
        public void GetText_KeyDoesNotExistsFallbackUsed_Translated()
        {
            Assert.AreEqual("Nome", GlobalizationHelper.GetText("___DOES_NOT_EXISTS___", "Name"));
        }

        [Test]
        public void GetText_ConditionTrue_TrueKey()
        {
            Assert.AreEqual("Sim", GlobalizationHelper.GetText(true, "Yes", "No"));
        }

        [Test]
        public void GetText_ConditionFalse_FalseKey()
        {
            Assert.AreEqual("Não", GlobalizationHelper.GetText(false, "Yes", "No"));
        }

        [Test]
        public void ToYesNo_ConditionTrue_Yes()
        {
            Assert.AreEqual("Sim", true.ToYesNo());
        }

        [Test]
        public void ToYesNo_ConditionFalse_No()
        {
            Assert.AreEqual("Não", false.ToYesNo());
        }
    }
}
