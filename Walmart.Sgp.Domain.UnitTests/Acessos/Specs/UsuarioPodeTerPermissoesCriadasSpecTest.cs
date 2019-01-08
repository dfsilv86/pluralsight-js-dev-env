using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioPodeTerPermissoesCriadasSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioTemPermissoes_False()
        {
            var permissaoService = MockRepository.GenerateMock<IPermissaoService>();
            permissaoService.Expect(u => u.ContarPermissoesPorUsuario(11)).Return(1);

            var target = new UsuarioPodeTerPermissoesCriadasSpec(permissaoService);
            var actual = target.IsSatisfiedBy(new Usuario() { Id = 11 });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.UserAlreadyHasPermissions, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioNaoTemPermissoes_True()
        {
            var permissaoService = MockRepository.GenerateMock<IPermissaoService>();
            permissaoService.Expect(u => u.ContarPermissoesPorUsuario(11)).Return(0);

            var target = new UsuarioPodeTerPermissoesCriadasSpec(permissaoService);
            var actual = target.IsSatisfiedBy(new Usuario() { Id = 11 });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
