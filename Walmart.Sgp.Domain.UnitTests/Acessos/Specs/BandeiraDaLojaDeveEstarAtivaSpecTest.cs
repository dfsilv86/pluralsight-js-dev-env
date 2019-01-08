using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class BandeiraDaLojaDeveEstarAtivaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_BandeiraDaLojaEstaAtiva_True()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();

            bandeiraGatewayMock.Expect(x => x.ObterPorIdLoja(1))
                .Return(new Bandeira { BlAtivo = BandeiraStatus.Ativo });

            var target = new BandeiraDaLojaDeveEstarAtivaSpec(bandeiraGatewayMock);
            var result = target.IsSatisfiedBy(new Loja { Id = 1 });

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_BandeiraDaLojaNaoEstaAtiva_False()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();

            bandeiraGatewayMock.Expect(x => x.ObterPorIdLoja(1))
                .Return(new Bandeira { BlAtivo = BandeiraStatus.Inativo });

            var target = new BandeiraDaLojaDeveEstarAtivaSpec(bandeiraGatewayMock);
            var result = target.IsSatisfiedBy(new Loja { Id = 1 });

            Assert.IsFalse(result.Satisfied);
        }
    }
}
