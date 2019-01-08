using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingNaoDevePossuirVendorWalmartSpecTest
    {
        [Test]
        public void IsSatisfiedBy_MultisourcingsComFornecedorWalmart_False()
        {
            var multisourcing = new Multisourcing() { Fornecedor = new Fornecedor() { cdFornecedor = 1 } };
            var multisourcing2 = new Multisourcing() { Fornecedor = new Fornecedor() { cdFornecedor = 2 } };

            Func<long, bool> f = (v) =>
            {
                return true;
            };

            var target = new MultisourcingNaoDevePossuirVendorWalmartSpec(f);

            var actual = target.IsSatisfiedBy(new[] { multisourcing, multisourcing2 });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_MultisourcingsSemFornecedorWalmart_True()
        {
            var multisourcing = new Multisourcing() { Fornecedor = new Fornecedor() { cdFornecedor = 1 } };
            var multisourcing2 = new Multisourcing() { Fornecedor = new Fornecedor() { cdFornecedor = 2 } };

            Func<long, bool> f = (v) =>
            {
                return false;
            };

            var target = new MultisourcingNaoDevePossuirVendorWalmartSpec(f);

            var actual = target.IsSatisfiedBy(new[] { multisourcing, multisourcing2 });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
