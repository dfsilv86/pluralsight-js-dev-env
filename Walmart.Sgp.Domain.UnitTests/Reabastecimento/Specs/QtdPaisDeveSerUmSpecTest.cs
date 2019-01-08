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
    public class QtdPaisDeveSerUmSpecTest
    {
        [Test]
        public void IsSatisfiedBy_UmItemPai_True()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true },
                new ItemDetalhe(){ VlCustoUnitario = 1 },
                new ItemDetalhe(){ VlCustoUnitario = 1 }
            };

            var target = new QtdPaisDeveSerUmSpec();
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_DoisItensPais_False()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true },
                new ItemDetalhe(){ VlCustoUnitario = 2, PaiCompraCasada = true },
                new ItemDetalhe(){ VlCustoUnitario = 1 }
            };

            var target = new QtdPaisDeveSerUmSpec();
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
