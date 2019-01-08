using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingDevePossuirFornecedorValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_FornecedorValido_True()
        {
            var multisourcing = new Multisourcing();

            var target = new MultisourcingDevePossuirFornecedorValidoSpec(m => new Fornecedor());
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_FornecedorNaoEValido_False()
        {
            var multisourcing = new Multisourcing();

            var target = new MultisourcingDevePossuirFornecedorValidoSpec(m => null);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.False(actual.Satisfied);
        }
    }
}
