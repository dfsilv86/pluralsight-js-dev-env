using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class UserInvalidOperationExceptionTest
    {
        [Test]
        public void Constructor_ReasonAndInnerException_Instance()
        {
            var actual = new UserInvalidOperationException("reason", new InvalidOperationException());
            Assert.AreEqual("reason", actual.Message);
            Assert.AreEqual(typeof(InvalidOperationException), actual.InnerException.GetType());
        }

        [Test]
        public void Constructor_SerializationInfo_Instance()
        {
            ExceptionTestHelper.TestSerializationCtor<UserInvalidOperationException>();
        }

        [Test]
        public void Is_Exception_False()
        {
            Assert.IsFalse(UserInvalidOperationException.Is(new Exception()));
            Assert.IsFalse(UserInvalidOperationException.Is(new InvalidOperationException()));
        }

        [Test]
        public void Is_UserInvalidOperationException_True()
        {
            Assert.IsTrue(UserInvalidOperationException.Is(new UserInvalidOperationException()));
        }

        [Test]
        public void Is_InvalidCastException_True()
        {
            Assert.IsTrue(UserInvalidOperationException.Is(new InvalidCastException()));
        }
    }
}
