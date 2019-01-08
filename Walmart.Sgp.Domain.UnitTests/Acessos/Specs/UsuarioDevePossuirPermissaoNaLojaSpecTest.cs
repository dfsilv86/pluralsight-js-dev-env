using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioDevePossuirPermissaoNaLojaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioNaoEAdministradorNemHO_False() 
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();
            var permissaoBandeiraGateway = MockRepository.GenerateMock<IPermissaoBandeiraGateway>();
            var permissaoLojaGateway = MockRepository.GenerateMock<IPermissaoLojaGateway>();
            
            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.IsAdministrator).Return(false);
            usuarioMock.Expect(x => x.IsHo).Return(false);

            var usuarioLoja = new UsuarioDevePossuirPermissaoNaLojaSpecParameter
            { 
                Usuario = usuarioMock,
                IdLoja = 1
            };

            var target = new UsuarioDevePossuirPermissaoNaLojaSpec(bandeiraGatewayMock, permissaoBandeiraGateway, permissaoLojaGateway);
            var result = target.IsSatisfiedBy(usuarioLoja);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioAdministradorEPossuiPermissaoBandeira_True()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();
            bandeiraGatewayMock.Expect(x => x.ObterPorIdLoja(1)).Return(new Bandeira { IDBandeira = 2 });
            
            var permissaoBandeiraGateway = MockRepository.GenerateMock<IPermissaoBandeiraGateway>();
            permissaoBandeiraGateway.Expect(x => x.UsuarioPossuiPermissaoBandeira(1, 2)).Return(true);

            var permissaoLojaGateway = MockRepository.GenerateMock<IPermissaoLojaGateway>();
            permissaoLojaGateway.Expect(x => x.UsuarioPossuiPermissaoLoja(1, 1)).Return(false);

            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.Id).Return(1);
            usuarioMock.Expect(x => x.IsAdministrator).Return(true);
            usuarioMock.Expect(x => x.IsHo).Return(false);

            var usuarioLoja = new UsuarioDevePossuirPermissaoNaLojaSpecParameter
            {
                Usuario = usuarioMock,
                IdLoja = 1
            };

            var target = new UsuarioDevePossuirPermissaoNaLojaSpec(bandeiraGatewayMock, permissaoBandeiraGateway, permissaoLojaGateway);
            var result = target.IsSatisfiedBy(usuarioLoja);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioHOENaoPossuiPermissaoBandeira_True()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();
            bandeiraGatewayMock.Expect(x => x.ObterPorIdLoja(1)).Return(new Bandeira { IDBandeira = 2 });

            var permissaoBandeiraGateway = MockRepository.GenerateMock<IPermissaoBandeiraGateway>();
            permissaoBandeiraGateway.Expect(x => x.UsuarioPossuiPermissaoBandeira(1, 2)).Return(false);

            var permissaoLojaGateway = MockRepository.GenerateMock<IPermissaoLojaGateway>();
            permissaoLojaGateway.Expect(x => x.UsuarioPossuiPermissaoLoja(1, 1)).Return(false);

            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.Id).Return(1);
            usuarioMock.Expect(x => x.IsAdministrator).Return(false);
            usuarioMock.Expect(x => x.IsHo).Return(true);

            var usuarioLoja = new UsuarioDevePossuirPermissaoNaLojaSpecParameter
            {
                Usuario = usuarioMock,
                IdLoja = 1
            };

            var target = new UsuarioDevePossuirPermissaoNaLojaSpec(bandeiraGatewayMock, permissaoBandeiraGateway, permissaoLojaGateway);
            var result = target.IsSatisfiedBy(usuarioLoja);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioAdministradorEPossuiPermissaoLoja_True()
        {
            var bandeiraGatewayMock = MockRepository.GenerateMock<IBandeiraGateway>();
            bandeiraGatewayMock.Expect(x => x.ObterPorIdLoja(1)).Return(new Bandeira { IDBandeira = 2 });

            var permissaoBandeiraGateway = MockRepository.GenerateMock<IPermissaoBandeiraGateway>();
            permissaoBandeiraGateway.Expect(x => x.UsuarioPossuiPermissaoBandeira(1, 2)).Return(false);

            var permissaoLojaGateway = MockRepository.GenerateMock<IPermissaoLojaGateway>();
            permissaoLojaGateway.Expect(x => x.UsuarioPossuiPermissaoLoja(1, 1)).Return(true);

            var usuarioMock = MockRepository.GenerateMock<IRuntimeUser>();
            usuarioMock.Expect(x => x.Id).Return(1);
            usuarioMock.Expect(x => x.IsAdministrator).Return(true);
            usuarioMock.Expect(x => x.IsHo).Return(false);

            var usuarioLoja = new UsuarioDevePossuirPermissaoNaLojaSpecParameter
            {
                Usuario = usuarioMock,
                IdLoja = 1
            };

            var target = new UsuarioDevePossuirPermissaoNaLojaSpec(bandeiraGatewayMock, permissaoBandeiraGateway, permissaoLojaGateway);
            var result = target.IsSatisfiedBy(usuarioLoja);

            Assert.IsTrue(result.Satisfied);
        }
    }
}
