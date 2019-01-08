using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Data;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Data
{
    [TestFixture]
    [Category("Audit")]
    public class EntityTableModelRegistryTest
    {
        [Test]
        public void GetTableModelForEntity_Args_TableModel()
        {
            EntityTableModelRegistry.RegisterEntityTableModel(typeof(DateTime), "Test", "Test2");

            var result = EntityTableModelRegistry.GetTableModelForEntity(typeof(DateTime));

            Assert.AreEqual("Test", result.Item1);
            Assert.AreEqual("Test2", result.Item2);

            result = EntityTableModelRegistry.GetTableModelForEntity(typeof(DateTime).FullName);

            Assert.AreEqual("Test", result.Item1);
            Assert.AreEqual("Test2", result.Item2);
        }
    }
}
