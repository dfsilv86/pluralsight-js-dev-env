using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ProcessExceptionTest
    {
        [Test]
        public void Constructor_Nothing_ProcessException()
        {
            var target = new ProcessException();

            Assert.AreEqual(Texts.ProcessOrderStateErrorMessage, target.Message);
        }

        [Test]
        public void Constructor_Message_ProcessException()
        {
            var target = new ProcessException("Foo");

            Assert.AreEqual("Foo", target.Message);
        }

        [Test]
        public void Constructor_MessageInner_ProcessException()
        {
            var inner = new NotSupportedException();
            var target = new ProcessException("Foo", inner);

            Assert.AreEqual("Foo", target.Message);
            Assert.AreSame(inner, target.InnerException);
        }

        [Test]
        public void Constructor_ProcessOrder_ProcessException()
        {
            var other = new ProcessOrderResult(null, null);
            var target = new ProcessException(other);

            Assert.AreEqual(Texts.ProcessOrderStateErrorMessage, target.Message);
            Assert.AreSame(other, target.ProcessResult);
        }

        [Test]
        public void Constructor_SerializationInfo_Instance()
        {
            ExceptionTestHelper.TestSerializationCtor<ProcessException>();
        }

        [Test]
        public void GetObjectData_Null_Exception()
        {
            var other = new ProcessOrderResult(null, null);
            var target = new ProcessException(other);

            Assert.Throws<ArgumentNullException>(() =>
            {
                target.GetObjectData(null, new System.Runtime.Serialization.StreamingContext());
            });
        }
    }
}
