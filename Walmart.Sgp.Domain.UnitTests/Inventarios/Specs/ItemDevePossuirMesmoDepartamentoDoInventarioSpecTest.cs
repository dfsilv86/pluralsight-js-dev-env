using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class ItemDevePossuirMesmoDepartamentoDoInventarioSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemMesmoDepartamento_Satisfied()
        {
            var itemDetalhe = new ItemDetalhe
            {
                IDDepartamento = 4
            };

            var target = new ItemDevePossuirMesmoDepartamentoDoInventarioSpec(4);
            var actual = target.IsSatisfiedBy(itemDetalhe);
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSatisfiedBy_ItemDepartamentoDiferente_NotSatisfied()
        {
            var itemDetalhe = new ItemDetalhe
            {
                IDDepartamento = 5
            };

            var target = new ItemDevePossuirMesmoDepartamentoDoInventarioSpec(4);
            var actual = target.IsSatisfiedBy(itemDetalhe);
            Assert.IsFalse(actual);
        }
    }
}