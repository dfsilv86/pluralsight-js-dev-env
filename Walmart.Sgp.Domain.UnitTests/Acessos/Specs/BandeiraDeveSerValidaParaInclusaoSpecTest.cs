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
    public class BandeiraDeveSerValidaParaInclusaoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_BandeiraValidaParaInclusao_True()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();

            bandeiraGatewayMock.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new [] { new Bandeira { BlAtivo = BandeiraStatus.Ativo } })
                .IgnoreArguments();

            var target = new BandeiraDeveSerValidaParaInclusaoSpec(bandeiraGatewayMock);
            var result = target.IsSatisfiedBy(new Bandeira { Id = 1 });

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_BandeiraNaoValidaParaInclusao_False()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();

            bandeiraGatewayMock.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Bandeira { BlAtivo = BandeiraStatus.Inativo } })
                .IgnoreArguments();

            var target = new BandeiraDeveSerValidaParaInclusaoSpec(bandeiraGatewayMock);
            var result = target.IsSatisfiedBy(new Bandeira { Id = 1 });

            Assert.IsFalse(result.Satisfied);
        }
    }
}
