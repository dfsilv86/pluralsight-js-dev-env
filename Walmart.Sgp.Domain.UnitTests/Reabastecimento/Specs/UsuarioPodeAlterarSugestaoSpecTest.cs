using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioPodeAlterarSugestaoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioSemAlcada_Satisfied()
        {
            var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
            alcadaService.Stub(t => t.ObterPorPerfil(0)).IgnoreArguments().Return(null);
            var runtimeUser = new MemoryRuntimeUser();

            var target = new UsuarioPodeAlterarSugestaoSpec(alcadaService);
            var actual = target.IsSatisfiedBy(runtimeUser);
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioComPermissao_Satisfied()
        {
            var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
            alcadaService.Stub(t => t.ObterPorPerfil(0)).IgnoreArguments().Return(new Alcada { blAlterarSugestao = true });
            var runtimeUser = new MemoryRuntimeUser();

            var target = new UsuarioPodeAlterarSugestaoSpec(alcadaService);
            var actual = target.IsSatisfiedBy(runtimeUser);
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioSemPermissao_NotSatisfied()
        {
            var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
            alcadaService.Stub(t => t.ObterPorPerfil(0)).IgnoreArguments().Return(new Alcada { blAlterarSugestao = false });
            var runtimeUser = new MemoryRuntimeUser();

            var target = new UsuarioPodeAlterarSugestaoSpec(alcadaService);
            var actual = target.IsSatisfiedBy(runtimeUser);
            Assert.IsFalse(actual);
        }
    }
}
