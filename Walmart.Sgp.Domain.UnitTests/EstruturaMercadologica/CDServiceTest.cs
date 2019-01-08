using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Memory;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class CDServiceTest
    {
        [Test]
        public void AtualizarNomeCD_IdENome_NomeAtualizado()
        {
            var gateway = new MemoryCDGateway();
            gateway.Insert(new CD { Id = 1, nmNome = "Nome 1" });
            gateway.Insert(new CD { Id = 2, nmNome = "Nome 2" });
            gateway.Insert(new CD { Id = 3, nmNome = "Nome 3" });

            var target = new CDService(gateway);
            target.AtualizarNomeCD(1, "Novo nome 1");
            Assert.AreEqual(0, gateway.Entities.Count(e => e.nmNome.Equals("Nome 1")));
            Assert.AreEqual(1, gateway.Entities.Count(e => e.nmNome.Equals("Novo nome 1")));
            Assert.AreEqual(1, gateway.Entities.Count(e => e.nmNome.Equals("Nome 2")));
            Assert.AreEqual(1, gateway.Entities.Count(e => e.nmNome.Equals("Nome 3")));

            target.AtualizarNomeCD(3, "Novo nome 3");
            Assert.AreEqual(0, gateway.Entities.Count(e => e.nmNome.Equals("Nome 1")));
            Assert.AreEqual(1, gateway.Entities.Count(e => e.nmNome.Equals("Novo nome 1")));
            Assert.AreEqual(1, gateway.Entities.Count(e => e.nmNome.Equals("Nome 2")));
            Assert.AreEqual(0, gateway.Entities.Count(e => e.nmNome.Equals("Nome 3")));
            Assert.AreEqual(1, gateway.Entities.Count(e => e.nmNome.Equals("Novo nome 3")));
        }
    }
}
