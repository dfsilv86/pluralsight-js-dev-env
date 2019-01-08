using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Movimentacao;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao
{
    [TestFixture]
    [Category("Domain")]
    public class EstoqueTest
    {
        [Test]
        public void TipoMovimentacao_TipoAjusteNulo_Nulo()
        {
            var target = new Estoque();
            target.TipoAjuste = null;
            Assert.IsNull(target.TipoMovimentacao);
        }

        [Test]
        public void TipoMovimentacao_TipoAjusteInvalido_Exception()
        {
            var target = new Estoque();
            target.TipoAjuste = new MotivoMovimentacao { Id = MotivoMovimentacao.IDErroDeQuebraPDV };

            Assert.Catch<InvalidOperationException>(() =>
            {
                var x = target.TipoMovimentacao;
            });
        }

        [Test]
        public void TipoMovimentacao_TipoAjusteEntradaAcerto_Entrada()
        {
            var target = new Estoque();
            target.TipoAjuste = new MotivoMovimentacao { Id = MotivoMovimentacao.IDEntradaAcerto };
            Assert.AreEqual(TipoMovimentacaoEstoque.Entrada, target.TipoMovimentacao);
        }

        [Test]
        public void TipoMovimentacao_TipoAjusteSaidaAcerto_Saida()
        {
            var target = new Estoque();
            target.TipoAjuste = new MotivoMovimentacao { Id = MotivoMovimentacao.IDSaidaAcerto };
            Assert.AreEqual(TipoMovimentacaoEstoque.Saida, target.TipoMovimentacao);
        }
    }
}
