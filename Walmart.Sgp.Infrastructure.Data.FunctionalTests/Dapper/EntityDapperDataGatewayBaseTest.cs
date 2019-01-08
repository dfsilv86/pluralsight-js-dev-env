using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Dapper")]
    public class EntityDapperDataGatewayBaseTest
    {
        [Test]
        public void Ctor_InvalidIdColumnName_Exception() 
        {            
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                InvalidGateway gateway = new InvalidGateway(new ApplicationDatabases());
            });
        }

        private class InvalidGateway : EntityDapperDataGatewayBase<TestEntity>
        {
            public InvalidGateway(ApplicationDatabases databases)
                : base(databases.Wlmslp, "TestTable", "InvalidPropertyName")
            {

            }

            protected override IEnumerable<string> ColumnsName
            {
                get { return new string[] { "Foo", "Bar" }; }
            }
        }

        public class TestEntity : EntityBase, IAggregateRoot
        {
            public string Foo { get; set;}
            public string Bar { get; set; }
        }
    }
}
