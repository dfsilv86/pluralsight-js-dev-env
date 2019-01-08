using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingDevePossuirItemEntradaValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemEntradaValido_True()
        {
            var multisourcing = new Multisourcing();

            var target = new MultisourcingDevePossuirItemEntradaValidoSpec(m => new ItemDetalhe { TpStatus = TipoStatusItem.Ativo });
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemEntradaNaoEValido_False()
        {
            var multisourcing = new Multisourcing();

            var target = new MultisourcingDevePossuirItemEntradaValidoSpec(m => null);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.False(actual.Satisfied);
        }
    }
}
