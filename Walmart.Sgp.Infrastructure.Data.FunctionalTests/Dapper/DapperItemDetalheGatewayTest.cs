using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Dapper")]
    public class DapperItemDetalheGatewayTest
    {
        [Test]
        public void ObterItemCustos_Args_ItemCustos()
        {
            this.RunTransaction(appDbs =>
            {
                var target = new DapperItemDetalheGateway(appDbs);
                var actual = target.ObterItemCustos(9073431, 1, null, new Paging()).ToList();
                Assert.IsNotNull(actual);
                Assert.AreEqual(0, actual.Count(a => String.IsNullOrEmpty(a.Loja.nmLoja)));
            });
        }
    }
}
