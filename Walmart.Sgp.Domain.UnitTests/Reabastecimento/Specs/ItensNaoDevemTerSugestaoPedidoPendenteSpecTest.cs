using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class ItensNaoDevemTerSugestaoPedidoPendenteSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItensSemSugestaoPendente_True()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true, ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 } },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 } },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 } }
            };

            var target = new ItensNaoDevemTerSugestaoPedidoPendenteSpec(1, (q, x, w, z) => { return false; }, (q, x, w, z) => { return false; });

            var actual = target.IsSatisfiedBy(itens);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItensComSugestaoPendente_False()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true, ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 } },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 } },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 } }
            };

            var target = new ItensNaoDevemTerSugestaoPedidoPendenteSpec(1, (q, x, w, z) => { return true; }, (q, x, w, z) => { return false; });

            var actual = target.IsSatisfiedBy(itens);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
