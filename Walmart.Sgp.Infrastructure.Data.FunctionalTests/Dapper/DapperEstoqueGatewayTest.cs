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
    public class DapperEstoqueGatewayTest
    {
        [Test]
        public void PesquisarUltimoCustoDoItemPorLoja_Args_Resultado()
        {
            this.RunTransaction(appDbs =>
            {
                Paging paging = new Paging();

                var target = new DapperEstoqueGateway(appDbs);

                // Testa se o sql não possui erros.
                var actual = target.PesquisarUltimoCustoDoItemPorLoja(1, null, 2, Framework.Domain.Acessos.TipoPermissao.PorBandeira, paging).ToList();
                Assert.IsNotNull(actual);
            });
        }
    }
}
