using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Audit")]
    public class AuditRecordTest
    {
        [Test]
        public void AuditRecord_Ctor_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var target = new AuditRecord<TestEntity>(new TestEntity() { Test = 1, Test2 = "Foo" }, AuditKind.Update);

                Assert.AreEqual(123, target.IdAuditUser);
                Assert.AreEqual(0, target.Id);
                Assert.AreEqual(0, target.IdAuditRecord);
                Assert.AreEqual(AuditKind.Update, target.CdAuditKind);
                Assert.IsTrue((DateTime.Now - target.DhAuditStamp).TotalSeconds < 1);
                Assert.IsNotNull(target.Entity);
                Assert.AreEqual(1, target.Entity.Test);
                Assert.AreEqual("Foo", target.Entity.Test2);

                target.Id = 100;

                Assert.AreEqual(100, target.IdAuditRecord);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AuditRecord_PrivateCtor_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var target = typeof(AuditRecord<TestEntity>).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

                Assert.IsNotNull(target);

                var result = (AuditRecord<TestEntity>)target.Invoke(null);

                Assert.IsNotNull(result);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }
    }
}
