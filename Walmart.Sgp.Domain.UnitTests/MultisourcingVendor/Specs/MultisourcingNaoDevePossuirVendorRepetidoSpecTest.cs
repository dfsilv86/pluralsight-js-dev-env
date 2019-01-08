using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingNaoDevePossuirVendorRepetidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_MultisourcingsComFornecedorRepetido_False()
        {
            var multisourcing = new Multisourcing();
            var multisourcing2 = new Multisourcing();

            var contadorFornecedor = 1;
            Func<long, byte, ItemDetalhe> f = (s, i) =>
            {
                return new ItemDetalhe() { IdFornecedorParametro = contadorFornecedor };
            };

            var target = new MultisourcingNaoDevePossuirVendorRepetidoSpec(f, 1);

            var actual = target.IsSatisfiedBy(new[] { multisourcing, multisourcing2 });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_MultisourcingsSemFornecedorRepetido_True()
        {
            var multisourcing = new Multisourcing();
            var multisourcing2 = new Multisourcing();

            var contadorFornecedor = 1;
            Func<long, byte, ItemDetalhe> f = (s, i) =>
            {
                contadorFornecedor++;
                return new ItemDetalhe() { IdFornecedorParametro = contadorFornecedor };
            };

            var target = new MultisourcingNaoDevePossuirVendorRepetidoSpec(f, 1);

            var actual = target.IsSatisfiedBy(new[] { multisourcing, multisourcing2 });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
