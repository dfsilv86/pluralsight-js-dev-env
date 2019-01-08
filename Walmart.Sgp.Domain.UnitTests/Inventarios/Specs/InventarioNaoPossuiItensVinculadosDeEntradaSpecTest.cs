using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class InventarioNaoPossuiItensVinculadosDeEntradaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_VinculadoEntrada_NotSatisfied()
        {
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(t => t.PossuiItemVinculadoEntrada(1))
                .Return(true);

            var target = new InventarioNaoPossuiItensVinculadosDeEntradaSpec(inventarioGateway);
            var actual = target.IsSatisfiedBy(new Inventario { Id = 1 });

            Assert.IsFalse(actual);
            inventarioGateway.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_SemVinculadoEntrada_Satisfied()
        {
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(t => t.PossuiItemVinculadoEntrada(1))
                .Return(false);

            var target = new InventarioNaoPossuiItensVinculadosDeEntradaSpec(inventarioGateway);
            var actual = target.IsSatisfiedBy(new Inventario { Id = 1 });

            Assert.IsTrue(actual);
            inventarioGateway.VerifyAllExpectations();
        }
    }
}