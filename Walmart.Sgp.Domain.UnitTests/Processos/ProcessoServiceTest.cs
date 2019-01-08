using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Processos;

namespace Walmart.Sgp.Domain.UnitTests.Processos
{
    [TestFixture]
    [Category("Domain")]
    public class ProcessoServiceTest
    {
        [Test]
        public void ObterCargaPorLoja_Filtro_Cargas()
        {
            var gateway = MockRepository.GenerateMock<IProcessoGateway>();
            gateway.Expect(g => g.PesquisarCargas(null, null)).IgnoreArguments().Return(new LojaProcessosCarga[] {
                new LojaProcessosCarga(DateTime.Now, null, null),
                new LojaProcessosCarga(DateTime.Now, null, null)            
            });
            var target = new ProcessoService(gateway);
            Assert.IsNotNull(target.ObterCargaPorLoja(new ProcessoCargaFiltro { 
                 CdLoja = 3,
                 IdBandeira  = 1,
                 CdSistema = 1,
                 Data = DateTime.Now
            }));
        }
    }
}
