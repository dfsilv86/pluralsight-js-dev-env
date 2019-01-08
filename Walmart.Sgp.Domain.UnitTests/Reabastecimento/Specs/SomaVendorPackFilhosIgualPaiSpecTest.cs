using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class SomaVendorPackFilhosIgualPaiSpecTest
    {
        [Test]
        public void IsSatisfiedBy_VendorPackItensFilhoIgualAoVendorPackPai_True()
        {
            var itens = new[] {
                new ItemDetalhe(){ QtVendorPackage = 2, PaiCompraCasada = true },
                new ItemDetalhe(){ QtVendorPackage = 1, FilhoCompraCasada = true },
                new ItemDetalhe(){ QtVendorPackage = 1, FilhoCompraCasada = true }
            };

            var target = new SomaVendorPackFilhosIgualPaiSpec();
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_VendorPackItensFilhoDiferenteDoVendorPackPai_False()
        {
            var itens = new[] {
                new ItemDetalhe(){ QtVendorPackage = 2, PaiCompraCasada = true },
                new ItemDetalhe(){ QtVendorPackage = 1, FilhoCompraCasada = true },
                new ItemDetalhe(){ QtVendorPackage = 3, FilhoCompraCasada = true }
            };

            var target = new SomaVendorPackFilhosIgualPaiSpec();
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
