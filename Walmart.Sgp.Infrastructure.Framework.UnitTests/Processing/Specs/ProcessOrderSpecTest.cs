using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Processing.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing.Specs
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ProcessOrderSpecTest
    {
        [Test]
        public void ProcessOrder_Created_Valid()
        {
            // foi criada no banco
            ProcessOrder po = new ProcessOrder()
            {
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_Created_Invalid()
        {
            // nao pode criar no banco
            ProcessOrder po = new ProcessOrder();

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_CreatedToError_Valid()
        {
            // foi criada e executada imediatamente e ocorreu erro interno
            ProcessOrder po = new ProcessOrder()
            {
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                CreatedMachineName = "Escocia",
                WorkerName = "X"
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_Queued_Valid()
        {
            // foi criada e enfileirada
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.Queued,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_Queued_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.Queued,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_QueuedToError_Valid()
        {
            // foi criada e enfileirada e deu erro interno ao executar
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.Error,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedDate = DateTime.Now,
                ModifiedUserId = 1,
                CreatedMachineName = "Escocia",
                WorkerName = "X"
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_Error_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.Error,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedDate = DateTime.Now,
                ModifiedUserId = 1,
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_IsExecuting_Valid()
        {
            // foi criada e enfileirada, e inicio execucao posteriormente
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.IsExecuting,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                WorkerName = "X",
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_IsExecutingNoWorkerName_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.IsExecuting,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_IsExecutingNoStartDate_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.IsExecuting,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                CreatedMachineName = "Escocia",
                WorkerName = "X"
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_Finished_Valid()
        {
            // foi criada e enfileirada, executada e finalizada com sucesso
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.Finished,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                WorkerName = "X",
                EndDate = DateTime.Now,
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_Finished_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.Finished,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                WorkerName = "X",
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_ResultsAvailable_Valid()
        {
            // foi criada e enfileirada, executada, finalizada com sucesso e resultado gravado em arquivo
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.ResultsAvailable,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                WorkerName = "X",
                EndDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    ResultFilePath = "X",
                    ResultTypeFullName = "X",
                    RoleId = 1,
                },
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_ResultsAvailable_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.ResultsAvailable,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                WorkerName = "X",
                EndDate = DateTime.Now,
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_ResultsExpunged_Valid()
        {
            // foi criada e enfileirada, executada, finalizada com sucesso, resultado gravado em arquivo, e posteriormente expurgada
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.ResultsExpunged,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                WorkerName = "X",
                EndDate = DateTime.Now,
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_ResultsExpunged_Invalid()
        {
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.ResultsExpunged,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService()
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ProcessOrder_ErrorToResultsExpunged_Valid()
        {
            // foi criada e enfileirada, ocorreu erro interno e posteriormente expurgada
            ProcessOrder po = new ProcessOrder()
            {
                State = ProcessOrderState.ResultsExpunged,
                ProcessName = "X",
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Service = new ProcessOrderService
                {
                    ServiceMethodName = "X",
                    ServiceTypeName = "X",
                    RoleId = 1,
                },
                Ticket = "X",
                ModifiedUserId = 1,
                ModifiedDate = DateTime.Now,
                WorkerName = "X",
                CreatedMachineName = "Escocia",
            };

            ProcessOrderSpec target = new ProcessOrderSpec();

            var result = target.IsSatisfiedBy(po);

            Assert.IsTrue(result.Satisfied);
        }
    }
}
