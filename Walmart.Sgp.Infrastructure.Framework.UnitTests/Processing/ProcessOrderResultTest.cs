using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ProcessOrderResultTest
    {
        [Test]
        public void BaseConstructor_Arguments_Ok()
        {
            var something = new object();
            var order = new ProcessOrder() { State = ProcessOrderState.Finished };

            var target = new ProcessOrderResult(something, order);

            Assert.IsNotNull(target);
            Assert.AreEqual(something, target.UnwrapResult());
        }

        [Test]
        public void Constructor_BaseProcessOrderResult_Ok()
        {
            var something = 1;
            var order = new ProcessOrder() { State = ProcessOrderState.ResultsAvailable };

            var baseResult = new ProcessOrderResult(something, order);

            var target = new ProcessOrderResult<int>(baseResult);

            Assert.IsNotNull(target);
            Assert.AreEqual(something, target.UnwrapResult());
        }

        [Test]
        public void Constructor_Arguments_Ok()
        {
            var something = 1;
            var order = new ProcessOrder() { State = ProcessOrderState.ResultsAvailable };

            var target = new ProcessOrderResult<int>(something, order);

            Assert.IsNotNull(target);
            Assert.AreEqual(something, target.UnwrapResult());
        }

        [Test]
        public void Constructor_Queued_Exception()
        {
            var something = 1;
            var order = new ProcessOrder() { State = ProcessOrderState.Queued };

            var baseResult = new ProcessOrderResult(something, order);

            var target = new ProcessOrderResult<int>(baseResult);

            Assert.IsNotNull(target);

            Assert.Throws<ProcessNotFinishedException>(() =>
            {
                Assert.AreEqual(something, target.UnwrapResult());
            });
        }

        [Test]
        public void Constructor_Error_Exception()
        {
            var something = 1;
            var order = new ProcessOrder() { State = ProcessOrderState.Error };

            var baseResult = new ProcessOrderResult(something, order);

            var target = new ProcessOrderResult<int>(baseResult);

            Assert.IsNotNull(target);

            Assert.Throws<ProcessException>(() =>
            {
                Assert.AreEqual(something, target.UnwrapResult());
            });
        }
    }
}
