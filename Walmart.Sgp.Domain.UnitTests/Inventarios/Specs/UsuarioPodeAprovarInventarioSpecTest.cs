using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioPodeAprovarInventarioSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioSemPermissao_NotSatisfied()
        {
            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Importado
            };

            var usuario = MockRepository.GenerateMock<IRuntimeUser>();
            usuario.Expect(t => t.HasPermission(null)).IgnoreArguments().Return(false);

            var target = new UsuarioPodeAprovarInventarioSpec(inventario);

            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
            usuario.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_InventarioStatusIncorreto_NotSatisfied()
        {
            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Aberto
            };

            var usuario = MockRepository.GenerateMock<IRuntimeUser>();
            usuario.Stub(t => t.HasPermission(null)).IgnoreArguments().Return(true);

            var target = new UsuarioPodeAprovarInventarioSpec(inventario);

            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_InventarioStatusCorretoUsuarioComPermissao_Satisfied()
        {
            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Importado
            };

            var usuario = MockRepository.GenerateMock<IRuntimeUser>();
            usuario.Expect(t => t.HasPermission(null)).IgnoreArguments().Return(true);

            var target = new UsuarioPodeAprovarInventarioSpec(inventario);

            var actual = target.IsSatisfiedBy(usuario);
            Assert.True(actual);
            usuario.VerifyAllExpectations();
        }
    }
}