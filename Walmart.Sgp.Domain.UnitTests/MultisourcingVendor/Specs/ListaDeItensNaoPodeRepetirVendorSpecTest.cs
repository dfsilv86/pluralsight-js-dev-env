using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class ListaDeItensNaoPodeRepetirVendorSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItensMesmoVendor_False()
        {
            var target = new ListaDeItensNaoPodeRepetirVendorSpec((i, s) => { return new Domain.Item.ItemDetalhe() { IdFornecedorParametro = 1 }; }, 1);
            var actual = target.IsSatisfiedBy(new ItemDetalheCD[] { new ItemDetalheCD() { vlPercentual = 50 }, new ItemDetalheCD() { vlPercentual = 50 } });

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
