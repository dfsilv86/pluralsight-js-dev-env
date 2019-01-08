using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Helpers
{
    [TestFixture]
    [Category("Audit")]
    public class AuditRecordHelperTest
    {
        [Test]
        public void AuditRecordHelper_CreateMapper_FullCopy()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                TestEntity testEntity = new TestEntity { Bar = 1m, BarFoo = DateTime.Today, Foo = 2, FooBar = "FooBar", Id = 2, NullableBar = null, NullableBarFoo = null, NullableFoo = null, FixedInt = TestFixedValueInt.One, FixedString = TestFixedValueString.First };

                AuditRecord<TestEntity> record = new AuditRecord<TestEntity>(testEntity, AuditKind.Insert);

                var target = AuditRecordHelper.CreateMapper<TestEntity>(new string[] { "Bar", "Foo", "FooBar", "BarFoo", "Id", "NullableBar", "NullableBarFoo", "NullableFoo", "FixedInt", "FixedString" });

                var result = target(record);

                Assert.AreEqual(13, result.Count);
                Assert.AreEqual(testEntity.Bar, result["Bar"]);
                Assert.AreEqual(testEntity.Foo, result["Foo"]);
                Assert.AreEqual(testEntity.FooBar, result["FooBar"]);
                Assert.AreEqual(testEntity.BarFoo, result["BarFoo"]);
                Assert.AreEqual(testEntity.Id, result["Id"]);
                Assert.AreEqual(testEntity.NullableBar, result["NullableBar"]);
                Assert.AreEqual(testEntity.NullableBarFoo, result["NullableBarFoo"]);
                Assert.AreEqual(testEntity.NullableFoo, result["NullableFoo"]);
                Assert.AreEqual(123, result["IdAuditUser"]);
                Assert.AreEqual(testEntity.FixedInt.Value, result["FixedInt"]);
                Assert.AreEqual(testEntity.FixedString.Value, result["FixedString"]);
                Assert.IsNotNull(result["DhAuditStamp"]);
                Assert.AreEqual(AuditKind.Insert.Value, result["CdAuditKind"]);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AuditRecordHelper_CreateMapper_PartialCopy()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 123, RoleName = "Guest", RoleId = 1 } };

            try
            {
                TestEntity testEntity = new TestEntity { Bar = 1m, BarFoo = DateTime.Today, Foo = 2, FooBar = "FooBar", Id = 2, NullableBar = null, NullableBarFoo = DateTime.Today.AddDays(-3), NullableFoo = null };

                AuditRecord<TestEntity> record = new AuditRecord<TestEntity>(testEntity, AuditKind.Delete);

                var target = AuditRecordHelper.CreateMapper<TestEntity>(new string[] { "Bar", "Id", "NullableBarFoo", "FixedInt" });

                var result = target(record);

                Assert.AreEqual(7, result.Count);
                Assert.AreEqual(testEntity.Bar, result["Bar"]);
                Assert.IsFalse(result.ContainsKey("Foo"));
                Assert.IsFalse(result.ContainsKey("FooBar"));
                Assert.IsFalse(result.ContainsKey("BarFoo"));
                Assert.AreEqual(testEntity.Id, result["Id"]);
                Assert.IsFalse(result.ContainsKey("NullableBar"));
                Assert.AreEqual(testEntity.NullableBarFoo, result["NullableBarFoo"]);
                Assert.IsFalse(result.ContainsKey("NullableFoo"));
                Assert.AreEqual(123, result["IdAuditUser"]);
                Assert.IsNotNull(result["DhAuditStamp"]);
                Assert.AreEqual(AuditKind.Delete.Value, result["CdAuditKind"]);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

    }
}
