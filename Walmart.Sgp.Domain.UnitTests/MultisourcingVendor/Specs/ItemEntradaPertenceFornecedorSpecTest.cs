using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class ItemEntradaPertenceFornecedorSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemEntradaPercenteFornecedor_True()
        {
            var multisourcing = new Multisourcing();

            var target = new ItemEntradaPertenceFornecedorSpec(m => true);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemEntradaNaoPercenteFornecedor_False()
        {
            var multisourcing = new Multisourcing();

            var target = new ItemEntradaPertenceFornecedorSpec(m => false);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.False(actual.Satisfied);
        }
    }
}
