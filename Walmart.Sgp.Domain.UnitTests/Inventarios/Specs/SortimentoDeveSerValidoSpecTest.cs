using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class SortimentoDeveSerValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_InventarioSortimentoInvalido_NotSatisfied()
        {
            var gateway = MockRepository.GenerateMock<IInventarioGateway>();
            gateway.Expect(t => t.PossuiSortimentoInvalido(1))
                .Return(true);

            var target = new SortimentoDeveSerValidoSpec(gateway);
            var actual = target.IsSatisfiedBy(new Inventario { Id = 1 });

            gateway.VerifyAllExpectations();
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_InventarioSortimentoValido_Satisfied()
        {
            var gateway = MockRepository.GenerateMock<IInventarioGateway>();
            gateway.Expect(t => t.PossuiSortimentoInvalido(1))
                .Return(false);

            var target = new SortimentoDeveSerValidoSpec(gateway);
            var actual = target.IsSatisfiedBy(new Inventario { Id = 1 });

            gateway.VerifyAllExpectations();
            Assert.IsTrue(actual);
        }
    }
}