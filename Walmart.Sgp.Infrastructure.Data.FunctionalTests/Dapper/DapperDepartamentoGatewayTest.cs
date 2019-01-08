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
    public class DapperDepartamentoGatewayTest
    {
        [Test]
        public void PesquisarPorDivisaoESistema_Args_DivisaoCarregada()
        {
            this.RunTransaction(appDbs =>
            {
                Paging paging = new Paging();
                var target = new DapperDepartamentoGateway(appDbs);
                var actual = target.PesquisarPorDivisaoESistema(null, null, null, 1, 1, paging).ToList();
                Assert.AreNotEqual(0, actual.Count);

                foreach(var c in actual)
                {
                    Assert.IsNotNull(c.Divisao);
                    Assert.AreNotEqual(0, c.Divisao.IDDivisao);
                }
            });
        }
    }
}
