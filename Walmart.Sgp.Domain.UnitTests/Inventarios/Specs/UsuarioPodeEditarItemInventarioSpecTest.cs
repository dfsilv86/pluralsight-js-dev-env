using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioPodeEditarItemInventarioSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UsuarioGaInventarioAprovado_Satisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Aprovado
            };

            var usuario = new MemoryRuntimeUser
            {
                IsGa = true
            };

            var target = new UsuarioPodeEditarItemInventarioSpec(inventario);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioNaoGaInventarioAprovado_NotSatisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Aprovado
            };

            var usuario = new MemoryRuntimeUser
            {
                IsGa = false
            };

            var target = new UsuarioPodeEditarItemInventarioSpec(inventario);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioGaInventarioImportado_Satisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Importado
            };

            var usuario = new MemoryRuntimeUser
            {
                IsGa = true
            };

            var target = new UsuarioPodeEditarItemInventarioSpec(inventario);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSatisfiedBy_UsuarioGaInventarioAberto_Satisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Aberto
            };

            var usuario = new MemoryRuntimeUser
            {
                IsGa = false
            };

            var target = new UsuarioPodeEditarItemInventarioSpec(inventario);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_InventarioCancelado_NotSatisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Cancelado
            };

            var usuario = new MemoryRuntimeUser
            {
                IsGa = true
            };

            var target = new UsuarioPodeEditarItemInventarioSpec(inventario);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }
    }
}