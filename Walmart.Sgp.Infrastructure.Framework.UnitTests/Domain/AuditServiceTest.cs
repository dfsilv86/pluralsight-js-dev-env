using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Data;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Is = Rhino.Mocks.Constraints.Is;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Audit")]
    public class AuditServiceTest
    {
        [SetUp]
        public void Init()
        {
            EntityTableModelRegistry.RegisterEntityTableModel(typeof(TestEntity), "TestTable", "Test");
        }

        class MockAuditGateway : IAuditGateway
        {
            Action<IEntity> m_action;

            public MockAuditGateway(Action<IEntity> action)
            {
                m_action = action;
            }

            public void Insert<TEntity>(AuditRecord<TEntity> record, string[] propertyNames)
            {
                Assert.IsTrue(new string[] { "Test", "Test2" }.SequenceEqual(propertyNames), "Deve gerar a lista de propriedades de TestEntity");

                m_action(record);
            }

            public IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(IEnumerable<string> propertyNames, int? idUsuario, int? idEntidade, DateTime? intervaloInicio, DateTime? intervaloFim, Paging paging) where TEntity : IEntity
            {
                Assert.IsTrue(new string[] { "Test", "Test2" }.SequenceEqual(propertyNames), "Deve gerar a lista de propriedades de TestEntity");

                return new AuditRecord<TEntity>[0];
            }
        }

        [Test]
        public void LogInsertNoProps_Args_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                IAuditGateway gateway = new MockAuditGateway(x =>
                {
                    var auditRecord = (AuditRecord<TestEntity>)x;
                    
                    Assert.AreEqual(AuditKind.Insert, auditRecord.CdAuditKind);
                    Assert.AreEqual(123, auditRecord.IdAuditUser);
                    Assert.IsTrue((DateTime.Now - auditRecord.DhAuditStamp).TotalSeconds < 1);
                    Assert.AreEqual(1, auditRecord.Entity.Test);
                    Assert.AreEqual("X", auditRecord.Entity.Test2);
                });

                AuditService target = new AuditService(gateway);

                target.LogInsert(new TestEntity() { Test = 1, Test2 = "X" });
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void LogUpdateNoProps_Args_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                IAuditGateway gateway = new MockAuditGateway(x =>
                {
                    var auditRecord = (AuditRecord<TestEntity>)x;

                    Assert.AreEqual(AuditKind.Update, auditRecord.CdAuditKind);
                    Assert.AreEqual(123, auditRecord.IdAuditUser);
                    Assert.IsTrue((DateTime.Now - auditRecord.DhAuditStamp).TotalSeconds < 1);
                    Assert.AreEqual(1, auditRecord.Entity.Test);
                    Assert.AreEqual("X", auditRecord.Entity.Test2);
                });

                AuditService target = new AuditService(gateway);

                target.LogUpdate(new TestEntity() { Test = 1, Test2 = "X" });
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void LogDeleteNoProps_Args_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                IAuditGateway gateway = new MockAuditGateway(x =>
                {
                    var auditRecord = (AuditRecord<TestEntity>)x;

                    Assert.AreEqual(AuditKind.Delete, auditRecord.CdAuditKind);
                    Assert.AreEqual(123, auditRecord.IdAuditUser);
                    Assert.IsTrue((DateTime.Now - auditRecord.DhAuditStamp).TotalSeconds < 1);
                    Assert.AreEqual(1, auditRecord.Entity.Test);
                    Assert.AreEqual("X", auditRecord.Entity.Test2);
                });

                AuditService target = new AuditService(gateway);

                target.LogDelete(new TestEntity() { Test = 1, Test2 = "X" });
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void ObterRelatorioNoProps_Args_Ok()
        {
            IAuditGateway gateway = new MockAuditGateway(x =>
            {
                throw new Exception();
            });

            AuditService target = new AuditService(gateway);

            var result = target.ObterRelatorio<TestEntity>(null, null, null, null, new Paging());

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void ObterRelatorioProps_Args_Ok()
        {
            IAuditGateway gateway = new MockAuditGateway(x =>
            {
                throw new Exception();
            });

            AuditService target = new AuditService(gateway);

            var result = target.ObterRelatorio<TestEntity>(new string[] { "Test", "Test2" }, null, null, null, null, new Paging());

            Assert.AreEqual(0, result.Count());
        }
    }
}
