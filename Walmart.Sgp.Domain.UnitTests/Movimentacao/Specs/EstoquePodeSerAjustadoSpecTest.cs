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

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class EstoquePodeSerAjustadoSpecTest 
    {
        [Test]
        public void IsSatisfiedBy_ItemPendente_False()
        {
            var notaFiscalService = MockRepository.GenerateMock<INotaFiscalService>();
            notaFiscalService.Expect(e => e.ObterItemNaUltimaNotaRecebidaDaLoja(2, 1)).Return(new NotaFiscalItem
            {
                IdNotaFiscalItemStatus = NotaFiscalItemStatus.IdPendente
            });
            var target = new EstoquePodeSerAjustadoSpec(notaFiscalService);            
            var actual = target.IsSatisfiedBy(new Estoque
            {
                IDLoja = 1,
                IDItemDetalhe = 2
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(actual.Reason, Texts.StockCannotBeAdjustedThereIsPendingInvoice);
        }

        [Test]
        public void IsSatisfiedBy_SemItem_True()
        {
            var notaFiscalService = MockRepository.GenerateMock<INotaFiscalService>();
            notaFiscalService.Expect(e => e.ObterItemNaUltimaNotaRecebidaDaLoja(2, 1)).Return(null);

            var target = new EstoquePodeSerAjustadoSpec(notaFiscalService);
            var actual = target.IsSatisfiedBy(new Estoque
            {
                IDLoja = 1,
                IDItemDetalhe = 2
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemNaoPendente_True()
        {
            var notaFiscalService = MockRepository.GenerateMock<INotaFiscalService>();
            notaFiscalService.Expect(e => e.ObterItemNaUltimaNotaRecebidaDaLoja(2, 1)).Return(new NotaFiscalItem
            {
                IdNotaFiscalItemStatus = 1
            });
            var target = new EstoquePodeSerAjustadoSpec(notaFiscalService);
            var actual = target.IsSatisfiedBy(new Estoque
            {
                IDLoja = 1,
                IDItemDetalhe = 2
            });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
