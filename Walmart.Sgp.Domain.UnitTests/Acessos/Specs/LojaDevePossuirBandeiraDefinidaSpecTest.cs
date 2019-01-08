using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class LojaDevePossuirBandeiraDefinidaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_LojaPossuiBandeiraDefinida_True()
        {
            var lojaGatewayMock = MockRepository.GenerateMock<ILojaGateway>();

            lojaGatewayMock.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Loja { IDBandeira = 2 } })
                .IgnoreArguments();

            var target = new LojaDevePossuirBandeiraDefinidaSpec(lojaGatewayMock);
            var result = target.IsSatisfiedBy(new Loja { Id = 1 });

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_LojaNaoPossuiBandeiraDefinida_False()
        {
            var lojaGatewayMock = MockRepository.GenerateMock<ILojaGateway>();

            lojaGatewayMock.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Loja { IDBandeira = null } })
                .IgnoreArguments();

            var target = new LojaDevePossuirBandeiraDefinidaSpec(lojaGatewayMock);
            var result = target.IsSatisfiedBy(new Loja { Id = 1 });

            Assert.IsFalse(result.Satisfied);
        }
    }
}
