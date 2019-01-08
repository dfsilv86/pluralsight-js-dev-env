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
    public class ProcessNotFinishedExceptionTest
    {
        [Test]
        public void Constructor_Nothing_ProcessNotFinishedException()
        {
            var target = new ProcessNotFinishedException();

            Assert.AreEqual(Texts.ProcessingNotFinished, target.Message);
        }

        [Test]
        public void Constructor_Message_ProcessNotFinishedException()
        {
            var target = new ProcessNotFinishedException("Foo");

            Assert.AreEqual("Foo", target.Message);
        }

        [Test]
        public void Constructor_MessageInner_ProcessNotFinishedException()
        {
            var inner = new NotSupportedException();
            var target = new ProcessNotFinishedException("Foo", inner);

            Assert.AreEqual("Foo", target.Message);
            Assert.AreSame(inner, target.InnerException);
        }

        [Test]
        public void Constructor_ProcessOrder_ProcessNotFinishedException()
        {
            var other = new ProcessOrderResult(null, null);
            var target = new ProcessNotFinishedException(other);

            Assert.AreEqual(Texts.ProcessingNotFinished, target.Message);
            Assert.AreSame(other, target.ProcessResult);
        }

        [Test]
        public void Constructor_SerializationInfo_Instance()
        {
            ExceptionTestHelper.TestSerializationCtor<ProcessNotFinishedException>();
        }

        [Test]
        public void GetObjectData_Null_Exception()
        {
            var other = new ProcessOrderResult(null, null);
            var target = new ProcessNotFinishedException(other);

            Assert.Throws<ArgumentNullException>(() =>
            {
                target.GetObjectData(null, new System.Runtime.Serialization.StreamingContext());
            });
        }
    }
}
