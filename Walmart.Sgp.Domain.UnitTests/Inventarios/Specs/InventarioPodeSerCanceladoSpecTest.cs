using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class InventarioPodeSerCanceladoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_InventarioStatusInvalido_NotSatisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Finalizado
            };

            var target = new InventarioPodeSerCanceladoSpec();
            var actual = target.IsSatisfiedBy(inventario);

            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_InventarioStatusCorreto_Satisfied()
        {
            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Importado
            };

            var target = new InventarioPodeSerCanceladoSpec();
            var actual = target.IsSatisfiedBy(inventario);

            Assert.IsTrue(actual);
        }
    }
}