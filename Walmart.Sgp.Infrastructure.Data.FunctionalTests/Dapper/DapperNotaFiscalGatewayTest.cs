#if DEV
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Dapper")]
    public class DapperNotaFiscalGatewayTest
    {
        [Test]
        public void ObterItensDaNotaFiscal_IdNotaFiscal_SomenteItensPereciveis()
        {
            this.RunTransaction(appDbs =>
            {
                var itemDetalheGateway = new DapperItemDetalheGateway(appDbs);
                Paging paging = new Paging();
                var target = new DapperNotaFiscalGateway(appDbs);

                // Nota fiscal sem itens perecíveis
                var actual = target.ObterItensDaNotaFiscal(19392339, paging);                
                Assert.AreEqual(0, actual.Count());

                // Nota fiscal com itens perecíveis
                actual = target.ObterItensDaNotaFiscal(19153383, paging);
                itemDetalheGateway = new DapperItemDetalheGateway(appDbs);

                Assert.AreNotEqual(0, actual.Count());

                foreach (var item in actual)
                {
                    var itemDetalhe = itemDetalheGateway.ObterEstruturadoPorId(item.ItemDetalhe.IDItemDetalhe);

                    Assert.AreEqual("S", itemDetalhe.Departamento.blPerecivel);
                }
            });
        }
    }
}
#endif