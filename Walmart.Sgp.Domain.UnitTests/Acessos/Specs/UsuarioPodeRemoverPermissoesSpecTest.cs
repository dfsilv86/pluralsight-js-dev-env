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
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioPodeRemoverPermissoesSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioNaoEhAdmin_False()
        {
            var permissaoService = MockRepository.GenerateMock<IPermissaoService>();

            var target = new UsuarioPodeRemoverPermissoesSpec(permissaoService);            
            var usuario = new MemoryRuntimeUser { Id = 1, IsHo = true, IsAdministrator = false };
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(actual.Reason, Texts.UserShouldBeAdminToRemovePermissions);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioEhAdmin_True()
        {
            var permissaoService = MockRepository.GenerateMock<IPermissaoService>();

            var target = new UsuarioPodeRemoverPermissoesSpec(permissaoService);
            var usuario = new MemoryRuntimeUser { Id = 1, IsHo = false, IsAdministrator = true };
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsTrue(actual.Satisfied);
        }      
    }
}
