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
    public class DapperSubcategoriaGatewayTest
    {
        [Test]
        public void PesquisarPorCategoriaDepartamentoESistema_Args_DepartamentoEDivisaoCarregados()
        {
            this.RunTransaction(appDbs =>
            {
                Paging paging = new Paging();

                var target = new DapperSubcategoriaGateway(appDbs);
                var actual = target.PesquisarPorSubcategoriaCategoriaDepartamentoESistema(null, null, 1, 1, 1, paging).ToList();
                Assert.AreNotEqual(0, actual.Count);

                foreach(var c in actual)
                {
                    Assert.IsNotNull(c.Categoria);
                    Assert.AreNotEqual(0, c.Categoria.Id);

                    Assert.IsNotNull(c.Categoria.Departamento);
                    Assert.AreNotEqual(0, c.Categoria.Departamento.Id);

                    Assert.IsNotNull(c.Categoria.Departamento.Divisao);
                    Assert.AreNotEqual(0, c.Categoria.Departamento.Divisao.Id);
                }
            });
        }
    }
}
