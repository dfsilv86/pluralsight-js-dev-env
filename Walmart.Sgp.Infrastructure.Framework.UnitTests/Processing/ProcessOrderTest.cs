using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ProcessOrderTest
    {
        [Test]
        public void CurrentProgressPercentage_Progress_Percent()
        {
            ProcessOrder target = new ProcessOrder();

            target.CurrentProgress = 1;
            target.TotalProgress = 2;

            Assert.AreEqual(50, target.CurrentProgressPercentage);
        }

        [Test]
        public void CurrentProgressPercentage_NoProgress_Zero()
        {
            ProcessOrder target = new ProcessOrder();

            target.CurrentProgress = 0;
            target.TotalProgress = 0;

            Assert.AreEqual(0, target.CurrentProgressPercentage);
        }

        [Test]
        public void Summarize_OrderWithArguments_Summary()
        {
            var current = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext() { User = new MemoryRuntimeUser { Id = 1000 } };

                DateTime startDate = new DateTime(2008, 01, 01), endDate = new DateTime(2012, 01, 01), executeAfter = new DateTime(2009, 01, 01);

                var order1 = new ProcessOrder()
                {
                    CurrentProgress = 1,
                    EndDate = endDate,
                    ExecuteAfter = executeAfter,
                    Id = 2,
                    Message = "Foo",
                    ProcessName = "Bar",
                    Service = new ProcessOrderService()
                    {
                        MaxGlobal = 3,
                        MaxPerUser = 4,
                        ResultFilePath = "Foobar",
                        ResultTypeFullName = "fubar",
                        ServiceMethodName = "Barfoo",
                        ServiceTypeName = "barfu",
                    },
                    StartDate = startDate,
                    State = ProcessOrderState.ResultsExpunged,
                    Ticket = "Ichijio Hikaru",
                    TotalProgress = 2,
                    WorkerName = "Kudo Shin"
                };

                order1.Arguments.Add(new ProcessOrderArgument() { Name = "Hidden", IsExposed = false });
                order1.Arguments.Add(new ProcessOrderArgument() { Name = "Visible", IsExposed = true });

                order1.StampInsert();
                order1.StampUpdate();

                var createdDate = order1.CreatedDate;
                var modifiedDate = order1.ModifiedDate;

                var target = order1.Summarize(true, a => a.IsExposed);

                Assert.AreEqual(createdDate, target.CreatedDate);
                Assert.AreEqual(1000, target.CreatedUserId);
                Assert.AreEqual(1, target.CurrentProgress);
                Assert.AreEqual(endDate, target.EndDate);
                Assert.AreEqual(executeAfter, target.ExecuteAfter);
                Assert.AreEqual(2, target.Id);
                Assert.AreEqual("Foo", target.Message);
                Assert.AreEqual(modifiedDate, target.ModifiedDate);
                Assert.AreEqual(1000, target.ModifiedUserId);
                Assert.AreEqual("Bar", target.ProcessName);
                Assert.AreEqual(startDate, target.StartDate);
                Assert.AreEqual(ProcessOrderState.ResultsExpunged, target.State);
                Assert.AreEqual("Ichijio Hikaru", target.Ticket);
                Assert.AreEqual(2, target.TotalProgress);
                Assert.AreEqual(50, target.CurrentProgressPercentage);

                Assert.AreEqual(1, target.Arguments.Count);
                Assert.AreEqual("Visible", target.Arguments[0].Name);
            }
            finally
            {
                RuntimeContext.Current = current;
            }
        }

        [Test]
        public void Summarize_OrderWithArgumentsButNoFilter_Summary()
        {
            var current = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext() { User = new MemoryRuntimeUser { Id = 1000 } };

                DateTime startDate = new DateTime(2008, 01, 01), endDate = new DateTime(2012, 01, 01), executeAfter = new DateTime(2009, 01, 01);

                var order1 = new ProcessOrder();

                order1.Arguments.Add(new ProcessOrderArgument() { Name = "Hidden", IsExposed = false });
                order1.Arguments.Add(new ProcessOrderArgument() { Name = "Visible", IsExposed = true });

                order1.StampInsert();
                order1.StampUpdate();

                var createdDate = order1.CreatedDate;
                var modifiedDate = order1.ModifiedDate;

                var target = order1.Summarize(true);

                Assert.AreEqual(0, target.Arguments.Count);
            }
            finally
            {
                RuntimeContext.Current = current;
            }
        }

        [Test]
        public void IStampContainer_Getters_Values()
        {
            var current = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext() { User = new MemoryRuntimeUser { Id = 1000 } };

                var order = new ProcessOrder();

                order.StampInsert();
                order.StampUpdate();

                var target = order as IStampContainer;

                Assert.AreEqual(order.ModifiedDate, target.DhAtualizacao);
                Assert.AreEqual(order.CreatedDate, target.DhCriacao);
                Assert.AreEqual(order.ModifiedUserId, target.CdUsuarioAtualizacao);
                Assert.AreEqual(order.CreatedUserId, target.CdUsuarioCriacao);
            }
            finally
            {
                RuntimeContext.Current = current;
            }
        }

        [Test]
        public void IStampContainer_NullCdUsuarioCriacao_Exception()
        {
            var order = new ProcessOrder();
            var target = order as IStampContainer;

            Assert.Throws<NotSupportedException>(() => { target.CdUsuarioCriacao = null; });
        }
    }
}
