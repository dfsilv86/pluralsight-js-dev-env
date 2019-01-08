using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class ItemInventarioDeveSerUnicoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemUnico_Satisfied()
        {
            var gateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            gateway.Expect(t => t.Count(null, new { })).IgnoreArguments()
                .Return(0);

            var target = new ItemInventarioDeveSerUnicoSpec(gateway);
            var inventarioItem = new InventarioItem
            {
                IDInventario = 1,
                IDInventarioItem = 1,
                IDItemDetalhe = 1
            };

            var actual = target.IsSatisfiedBy(inventarioItem);
            Assert.IsTrue(actual);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_ItemDuplicado_NotSatisfied()
        {
            var gateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            gateway.Expect(t => t.Count(null, new { })).IgnoreArguments()
                .Return(1);

            var target = new ItemInventarioDeveSerUnicoSpec(gateway);
            var inventarioItem = new InventarioItem
            {
                IDInventario = 1,
                IDInventarioItem = 1,
                IDItemDetalhe = 1
            };

            var actual = target.IsSatisfiedBy(inventarioItem);
            Assert.IsFalse(actual);
            gateway.VerifyAllExpectations();
        }
    }
}