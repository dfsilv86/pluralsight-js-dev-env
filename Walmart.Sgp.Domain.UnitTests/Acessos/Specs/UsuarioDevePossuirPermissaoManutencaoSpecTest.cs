using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioDevePossuirPermissaoManutencaoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioNaoAdministradorENaoPossuiPermissoes_False()
        {
            var permissaoGatewayMock = MockRepository.GenerateMock<IPermissaoGateway>();

            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.Id).Return(1);
            usuarioMock.Expect(x => x.IsAdministrator).Return(false);

            permissaoGatewayMock.Expect(x => x.ObterPermissoesDoUsuario(1))
                .Throw(new InvalidOperationException());

            var target = new UsuarioDevePossuirPermissaoManutencaoSpec(permissaoGatewayMock);
            var result = target.IsSatisfiedBy(usuarioMock);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioAdministrador_True()
        {
            var permissaoGatewayMock = MockRepository.GenerateMock<IPermissaoGateway>();

            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.Id).Return(1);
            usuarioMock.Expect(x => x.IsAdministrator).Return(true);

            var target = new UsuarioDevePossuirPermissaoManutencaoSpec(permissaoGatewayMock);
            var result = target.IsSatisfiedBy(usuarioMock);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioNaoAdministradorEPossuiPermissoes_True()
        {
            var permissaoGatewayMock = MockRepository.GenerateMock<IPermissaoGateway>();

            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.Id).Return(1);
            usuarioMock.Expect(x => x.IsAdministrator).Return(false);

            permissaoGatewayMock.Expect(x => x.ObterPermissoesDoUsuario(1))
                .Return(new UsuarioPermissoes(1, new[] { new Bandeira() }, new Loja[0]));

            var target = new UsuarioDevePossuirPermissaoManutencaoSpec(permissaoGatewayMock);
            var result = target.IsSatisfiedBy(usuarioMock);

            Assert.IsTrue(result.Satisfied);
        }
    }
}
