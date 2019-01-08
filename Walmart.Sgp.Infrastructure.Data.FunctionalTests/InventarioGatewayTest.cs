using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Dapper;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Ado")]
    [Category("Dapper")]
    public class InventarioGatewayTest : DataGatewayTestBase<IInventarioGateway>
    {
        [Test]
        public void FindNextInventoryDate_InventoryNotExists_Null()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                Assert.IsNull(target.ObterDataInventarioDaLoja(int.MaxValue));
            });
        }
    }
}
