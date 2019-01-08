using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class RoteiroLojaTest
    {
        [Test]
        public void ObterLojasValidas_Filtros_RetornaLojasValidas()
        {
            var RoteiroLojaGatewayMock = MockRepository.GenerateMock<IRoteiroLojaGateway>();
            var RoteiroLojaService = new RoteiroLojaService(RoteiroLojaGatewayMock);

            var paging = new Paging();

            RoteiroLojaGatewayMock
                .Expect(x => x.ObterLojasValidas(1, "RS", 3, paging))
                .Return(new[] { new RoteiroLoja() });

            var actual = RoteiroLojaService.ObterLojasValidas(1, "RS", 3, paging);

            RoteiroLojaGatewayMock.AssertWasCalled(x => x.ObterLojasValidas(1, "RS", 3, paging));
            Assert.AreEqual(1, actual.Count());
        }

        [Test]
        public void ObterPorIdRoteiro_IdRoteiro_RetornaRoteiroLoja()
        {
            var gateway = MockRepository.GenerateMock<IRoteiroLojaGateway>();
            var target = new RoteiroLojaService(gateway);

            gateway.Expect(g => g.ObterPorIdRoteiro(1)).Return(new[] { new RoteiroLoja() { Id = 1 } });

            var r = target.ObterPorIdRoteiro(1);

            Assert.AreEqual(1, r.Count());
        }
    }
}