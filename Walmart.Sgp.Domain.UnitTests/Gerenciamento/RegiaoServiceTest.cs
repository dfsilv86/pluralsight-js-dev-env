using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class RegiaoServiceTest
    {
        [Test]
        public void ObterPorBandeira_IDBandeira_Regioes()
        {
            Regiao[] regioes = new Regiao[] { new Regiao { Id = 1 }, new Regiao { Id = 2 } };

            var gateway = MockRepository.GenerateMock<IRegiaoGateway>();
            gateway.Expect(fs => fs.Find(null, null)).IgnoreArguments().Return(regioes);

            var target = new RegiaoService(gateway);

            var result = target.ObterPorBandeira(1);

            Assert.AreEqual(regioes, result);
            gateway.VerifyAllExpectations();
        }
    }
}
