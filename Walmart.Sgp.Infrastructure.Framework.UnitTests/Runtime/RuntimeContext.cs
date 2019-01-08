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
    public class RuntimeContextTest
    {
        [Test]
        public void Current_NotDefined_MemoryContext()
        {
            Assert.IsInstanceOf(typeof(MemoryRuntimeContext), RuntimeContext.Current);
        }

        [Test]
        public void Environment_CurrentCompilation()
        {
            Assert.AreNotEqual(-1, RuntimeContext.Environment);
        }

        [Test]
        public void User_IsAdministrator_False()
        {
            Assert.IsFalse(RuntimeContext.Current.User.IsAdministrator);
        }
    }
}
