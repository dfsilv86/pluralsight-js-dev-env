using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Memory;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class ParametroSistemaServiceTest
    {
        [Test]
        public void ObterPorNome_ExisteParametroSistema_ParametroSistema()
        {
            var gateway = new MemoryParametroSistemaGateway();
            gateway.Insert(new ParametroSistema { nmParametroSistema = "parametro", vlParametroSistema = "3" });
            var target = new ParametroSistemaService(gateway);

            var actual = target.ObterPorNome("parametro");
            Assert.AreEqual("3", actual.vlParametroSistema);
        }
    }
}
