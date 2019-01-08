using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class NotSatisfiedSpecExceptionTest
    {
        [Test]
        public void Constructor_ReasonAndInnerException_Instance()
        {
            var actual = new NotSatisfiedSpecException("reason", new InvalidOperationException());
            Assert.AreEqual("reason", actual.Message);
            Assert.AreEqual(typeof(InvalidOperationException), actual.InnerException.GetType());
        }

        [Test]
        public void Constructor_SerializationInfo_Instance()
        {
            ExceptionTestHelper.TestSerializationCtor<NotSatisfiedSpecException>();
        }
    }
}
