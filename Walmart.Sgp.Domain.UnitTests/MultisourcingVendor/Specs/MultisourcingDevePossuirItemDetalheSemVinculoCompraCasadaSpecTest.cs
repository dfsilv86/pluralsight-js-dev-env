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
    public class MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemDetalheEntradaSemVinculoCompraCasada_True()
        {
            var multisourcing = new Multisourcing
            {
                CdItemDetalheEntrada = 123
            };

            var target = new MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpec(cdItemDetalheEntrada => false);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemDetalheEntradaComVinculoCompraCasada_False()
        {
            var multisourcing = new Multisourcing
            {
                CdItemDetalheEntrada = 123
            };

            var target = new MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpec(cdItemDetalheEntrada => true);
            var actual = target.IsSatisfiedBy(new [] { multisourcing });

            Assert.IsFalse(actual.Satisfied);
        }
    }
}