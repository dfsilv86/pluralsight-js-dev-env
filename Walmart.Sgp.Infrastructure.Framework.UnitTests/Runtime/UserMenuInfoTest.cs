using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Runtime
{
    [TestFixture]
    [Category("Framework")]
    public class UserMenuInfoTest
    {
        [Test]
        public void Equals_Diff_False()
        {
            var target = new UserMenuInfo("1");
            var other = new UserMenuInfo("2");

            Assert.IsFalse(target.Equals((object)null));
            Assert.IsFalse(target.Equals(null as UserMenuInfo));
            Assert.IsFalse(target.Equals(other));
            Assert.AreNotEqual(target.GetHashCode(), other.GetHashCode());
        }

        [Test]
        public void Equals_Equal_True()
        {
            var target = new UserMenuInfo("1");
            var other = new UserMenuInfo("1");

            Assert.IsTrue(target.Equals(other));
            Assert.AreEqual(target.GetHashCode(), other.GetHashCode());
        }
    }
}
