using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class ItemEntradaDevePossuirTipoReabastecimentoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_PossuiTipoReabastecimento_Satisfied()
        {
            var service = MockRepository.GenerateMock<IItemDetalheService>();

            service.Expect(s => s.ObterPorItemESistema(10, 1)).Return(new ItemDetalhe { VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97 });

            var target = new ItemEntradaDevePossuirTipoReabastecimentoSpec(service.ObterPorItemESistema, 1);

            var result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 10 } });

            Assert.IsTrue(result.Satisfied);

            service.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_NaoPossuiTipoReabastecimento_NotSatisfied()
        {
            var service = MockRepository.GenerateMock<IItemDetalheService>();

            service.Expect(s => s.ObterPorItemESistema(10, 1)).Return(new ItemDetalhe { VlTipoReabastecimento = null });
            service.Expect(s => s.ObterPorItemESistema(20, 1)).Return(new ItemDetalhe { VlTipoReabastecimento = ValorTipoReabastecimento.Nenhum });

            var target = new ItemEntradaDevePossuirTipoReabastecimentoSpec(service.ObterPorItemESistema, 1);

            var result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 10 } });

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual("Item de Entrada não possui tipo de reabastecimento", result.Reason);

            result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 20 } });

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual("Item de Entrada não possui tipo de reabastecimento", result.Reason);

            service.VerifyAllExpectations();
        }
    }
}
