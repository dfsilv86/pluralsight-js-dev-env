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
    public class DapperProcessoGatewayTest
    {
        [Test]
        public void PesquisarCargas_Args_Cargas()
        {
            this.RunTransaction(appDbs =>
            {
                Paging paging = new Paging();
                var target = new DapperProcessoGateway(appDbs);
                var filtro = new ProcessoCargaFiltro
                {
                    CdSistema = 1,
                    CdLoja = 87,
                    Data = new DateTime(2016, 01, 22),
                    IdBandeira = 1
                };
                var actual = target.PesquisarCargas(filtro, new Paging()).ToArray();
                Assert.AreEqual(1, actual.Length);
                var lojaCarga = actual[0];

                Assert.AreEqual(1, lojaCarga.Bandeira.IDBandeira);
                Assert.AreEqual(87, lojaCarga.Loja.cdLoja);
                Assert.AreEqual(filtro.Data, lojaCarga.Data);
                Assert.AreEqual(11, lojaCarga.Cargas.Count);

                Assert.AreEqual(11, lojaCarga.Cargas.Count(c => c.Nome != null && c.Status != null));
            });
        }
    }
}
#endif