using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class TraitItensFilhoDevemSerIguaisAoPaiSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItensComTraitsIguaisAoPai_True()
        {
            var itens = new[] {
                new ItemDetalhe(){ VlCustoUnitario = 1, PaiCompraCasada = true, Traits = 3 },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, Traits = 3 },
                new ItemDetalhe(){ VlCustoUnitario = 1, FilhoCompraCasada = true, Traits = 3 }
            };

            var target = new TraitItensFilhoDevemSerIguaisAoPaiSpec((q, w, e) => { return new List<Loja>() { new Loja() { IDLoja = 1 } }; });
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItensComTraitsDiferentes_False()
        {
            var itens = new[] {
                new ItemDetalhe(){ IDItemDetalhe = 1, CdSistema = 1, VlCustoUnitario = 1, PaiCompraCasada = true },
                new ItemDetalhe(){ IDItemDetalhe = 2, CdSistema = 1, VlCustoUnitario = 1, FilhoCompraCasada = true },
                new ItemDetalhe(){ IDItemDetalhe = 3, CdSistema = 1, VlCustoUnitario = 1, FilhoCompraCasada = true }
            };

            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1 }
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(2, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 2 }
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 2 }
            });

            var target = new TraitItensFilhoDevemSerIguaisAoPaiSpec(itemDetalheGateway.ObterTraitsPorItem);
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItensComTraitsIguais_True()
        {
            var itens = new[] {
                new ItemDetalhe(){ IDItemDetalhe = 1, CdSistema = 1, VlCustoUnitario = 1, PaiCompraCasada = true },
                new ItemDetalhe(){ IDItemDetalhe = 2, CdSistema = 1, VlCustoUnitario = 1, FilhoCompraCasada = true },
                new ItemDetalhe(){ IDItemDetalhe = 3, CdSistema = 1, VlCustoUnitario = 1, FilhoCompraCasada = true }
            };

            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 2 }
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(2, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 2 }
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 2 }
            });

            var target = new TraitItensFilhoDevemSerIguaisAoPaiSpec(itemDetalheGateway.ObterTraitsPorItem);
            var actual = target.IsSatisfiedBy(itens);

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
