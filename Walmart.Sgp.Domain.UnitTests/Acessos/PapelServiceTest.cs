using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    [Category("Acessos")]
    public class PapelServiceTest
    {
        [Test]
        public void ObterPorNome_Nome_Papel()
        {
            var gateway = new MemoryPapelGateway();
            gateway.Insert(new Papel { Name = "papel1" });
            gateway.Insert(new Papel { Name = "papel2" });

            var target = new PapelService(gateway);
            var actual = target.ObterPorNome("papel1");
            Assert.AreEqual("papel1", actual.Name);

            actual = target.ObterPorNome("papel2");
            Assert.AreEqual("papel2", actual.Name);

            actual = target.ObterPorNome("papel3");
            Assert.IsNull(actual);
        }       
    }
}
