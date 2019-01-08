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
    public class TipoRAItensDeveSerIgualAoPaiSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItensComVlTipoReabastecimentoIguaisAoPai_True()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37 },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37 },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37 }
            };

            var target = new TipoRAItensDeveSerIgualAoPaiSpec();
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItensComVlTipoReabastecimentoDiferentesDoPai_False()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37 },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97 },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37 }
            };

            var target = new TipoRAItensDeveSerIgualAoPaiSpec();
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
