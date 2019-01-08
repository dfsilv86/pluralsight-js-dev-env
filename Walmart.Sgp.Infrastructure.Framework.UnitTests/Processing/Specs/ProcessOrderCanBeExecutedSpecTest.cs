using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing.Specs
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ProcessOrderCanBeExecutedSpecTest
    {
        [Test]
        public void IsSatisfied_ProcessOrdersReadyToRun_Satisfied()
        {
            var target = new ProcessOrderCanBeExecutedSpec();

            var order = new ProcessOrder { State = ProcessOrderState.Created, WorkerName = "Foo" };

            Assert.IsTrue(target.IsSatisfiedBy(order));

            order = new ProcessOrder { State = ProcessOrderState.Queued, WorkerName = "Foo" };

            Assert.IsTrue(target.IsSatisfiedBy(order));
        }

        [Test]
        public void IsSatisfied_ErrorProcessOrder_NotSatisfied()
        {
            var target = new ProcessOrderCanBeExecutedSpec();

            var order = new ProcessOrder { State = ProcessOrderState.Error, WorkerName = "Foo" };

            Assert.IsFalse(target.IsSatisfiedBy(order));
        }

        [Test]
        public void IsSatisfied_MissingWorkerName_NotSatisfied()
        {
            var target = new ProcessOrderCanBeExecutedSpec();

            var order = new ProcessOrder { State = ProcessOrderState.Queued };

            Assert.IsFalse(target.IsSatisfiedBy(order));
        }
    }
}
