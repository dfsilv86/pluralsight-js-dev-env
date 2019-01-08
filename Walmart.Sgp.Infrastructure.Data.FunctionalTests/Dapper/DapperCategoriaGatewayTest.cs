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
    public class DapperCategoriaGatewayTest
    {
        [Test]
        public void PesquisarPorCategoriaDepartamentoESistema_Args_DepartamentoEDivisaoCarregados()
        {
            this.RunTransaction(appDbs =>
            {
                Paging paging = new Paging();

                var target = new DapperCategoriaGateway(appDbs);
                var actual = target.PesquisarPorCategoriaDepartamentoESistema(null, null, 1, 1, paging).ToList();
                Assert.AreNotEqual(0, actual.Count);

                foreach(var c in actual)
                {
                    Assert.IsNotNull(c.Departamento);
                    Assert.AreNotEqual(0, c.Departamento.IDDepartamento);

                    Assert.IsNotNull(c.Departamento.Divisao);
                    Assert.AreNotEqual(0, c.Departamento.Divisao.IDDivisao);
                }
            });
        }
    }
}
