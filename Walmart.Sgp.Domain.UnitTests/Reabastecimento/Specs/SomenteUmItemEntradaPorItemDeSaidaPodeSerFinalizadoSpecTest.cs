using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_FinalizarMultiplosItensEntradaPorItemSaida_False() 
        {
            var sugestoesParaFinalizar = new []
            {
                new SugestaoPedidoCD
                {
                    idItemDetalheSugestao = 123,
                    idItemDetalhePedido = 444
                },

                new SugestaoPedidoCD
                {
                    idItemDetalheSugestao = 123,
                    idItemDetalhePedido = 555
                },

                new SugestaoPedidoCD
                {
                    idItemDetalheSugestao = 112,
                    idItemDetalhePedido = 321
                }
            };

            var target = new SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec(null);
            var result = target.IsSatisfiedBy(sugestoesParaFinalizar);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_FinalizarUmItemEntradaPorItemSaida_True()
        {
            var sugestoesParaFinalizar = new[]
            {
                new SugestaoPedidoCD
                {
                    idItemDetalhePedido = 123,
                    idItemDetalheSugestao = 444
                },

                new SugestaoPedidoCD
                {
                    idItemDetalhePedido = 112,
                    idItemDetalheSugestao = 321
                }
            };

            var target = new SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec(null);
            var result = target.IsSatisfiedBy(sugestoesParaFinalizar);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_FinalizarItemEntradaExistindoRegistrosItensEntradaJaFinalizados_False()
        {
            var sugestoesParaFinalizar = new[]
            {
                new SugestaoPedidoCD
                {
                    idItemDetalhePedido = 123,
                    idItemDetalheSugestao = 444
                }
            };

            Func<long, bool> existemRegistrosJaFinalizados = (idSugestaoProdutoCD) => 
            {
                return true;
            };

            var target = new SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec(existemRegistrosJaFinalizados);
            var result = target.IsSatisfiedBy(sugestoesParaFinalizar);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_FinalizarItemEntradaNaoExistindoRegistrosItensEntradaJaFinalizados_False()
        {
            var sugestoesParaFinalizar = new[]
            {
                new SugestaoPedidoCD
                {
                    idItemDetalhePedido = 123,
                    idItemDetalheSugestao = 444
                }
            };

            Func<long, bool> existemRegistrosJaFinalizados = (idSugestaoProdutoCD) =>
            {
                return false;
            };

            var target = new SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec(existemRegistrosJaFinalizados);
            var result = target.IsSatisfiedBy(sugestoesParaFinalizar);

            Assert.IsTrue(result.Satisfied);
        }
    }
}