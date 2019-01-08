using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Movimentacao.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class MtrPodeSerRealizadaSpecTest 
    {
        [Test]
        public void IsSatisfiedBy_QuantidadeNaoInformada_False()
        {
            var target = new MtrPodeSerRealizadaSpec(null);
            var actual = target.IsSatisfiedBy(new MovimentacaoMtr
            {
                Quantidade = 0
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.NeedToConfirmQuantity, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ItemOrigemEDestinoIguais_False()
        {
            var target = new MtrPodeSerRealizadaSpec(null);
            var actual = target.IsSatisfiedBy(new MovimentacaoMtr
            {
                Quantidade = 1,
                IdItemDestino = 2,
                IdItemOrigem = 2
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SourceItemAndDestItemShouldBeDiff, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ItemOrigemEDestinoMesmoDepartamento_False()
        {
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorIds(1, 2)).IgnoreArguments().Return(new ItemDetalhe[] { new ItemDetalhe { IDDepartamento = 11 }, new ItemDetalhe { IDDepartamento = 11 } });

            var target = new MtrPodeSerRealizadaSpec(itemDetalheService);
            var actual = target.IsSatisfiedBy(new MovimentacaoMtr
            {
                Quantidade = 1,
                IdItemDestino = 1,
                IdItemOrigem = 2
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SourceItemAndDestItemShouldBeFromDiffDepartament, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_QuantidadeInformadaItemOrigemEDestinoDiferente_True()
        {
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorIds(1, 2)).IgnoreArguments().Return(new ItemDetalhe[] { new ItemDetalhe { IDDepartamento = 11 }, new ItemDetalhe { IDDepartamento = 22 } });

            var target = new MtrPodeSerRealizadaSpec(itemDetalheService);
            var actual = target.IsSatisfiedBy(new MovimentacaoMtr
            {
                Quantidade = 1,
                IdItemDestino = 2,
                IdItemOrigem = 3
            });

            Assert.IsTrue(actual.Satisfied);
        }  
    }
}
