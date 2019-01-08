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
    public class MultisourcingDevePossuirItemSaidaValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemSaidaValido_True()
        {
            var multisourcing = new Multisourcing();

            var target = new MultisourcingDevePossuirItemSaidaValidoSpec(m => new ItemDetalhe { TpStatus = TipoStatusItem.Ativo });
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaNaoEValido_False()
        {
            var multisourcing = new Multisourcing();

            var target = new MultisourcingDevePossuirItemSaidaValidoSpec(m => null);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.False(actual.Satisfied);
        }
    }
}
