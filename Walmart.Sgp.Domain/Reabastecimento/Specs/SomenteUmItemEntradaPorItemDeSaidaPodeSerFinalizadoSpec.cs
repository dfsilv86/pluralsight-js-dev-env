using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a validação de finalização de apenas um item de entrada para um item de saída.
    /// </summary>
    public class SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec : SpecBase<IEnumerable<SugestaoPedidoCD>>
    {
        private Func<long, bool> m_existeSugestoesFinalizadasMesmoItemDetalheSaida;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec"/>.
        /// </summary>
        /// <param name="existeSugestoesFinalizadasMesmoItemDetalheSaida">O delegate que verifica se já existem sugestões pedido CD finalizadas para o item de saída.</param>
        public SomenteUmItemEntradaPorItemDeSaidaPodeSerFinalizadoSpec(Func<long, bool> existeSugestoesFinalizadasMesmoItemDetalheSaida)
        {
            m_existeSugestoesFinalizadasMesmoItemDetalheSaida = existeSugestoesFinalizadasMesmoItemDetalheSaida;
        }

        /// <summary>
        /// Verifica se as sugestões pedido CD informadas satisfazem a especificação.
        /// </summary>
        /// <param name="target">A lista de sugestões pedido CD à serem validadas.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<SugestaoPedidoCD> target)
        {
            var existemMultiplosItensEntradaPorItemSaida = target.GroupBy(spc => spc.idItemDetalheSugestao).Any(grouped => grouped.Count() > 1);

            if (existemMultiplosItensEntradaPorItemSaida)
            {
                return NotSatisfied(Texts.validateOnlyOneItemFinalizedByOutputItem);
            }

            if (m_existeSugestoesFinalizadasMesmoItemDetalheSaida != null)
            {
                if (ExistemRegistrosFinalizadosMesmoItemSaida(target))
                {
                    return NotSatisfied(Texts.validateOnlyOneItemFinalizedByOutputItem);
                }
            }

            return Satisfied();
        }

        private bool ExistemRegistrosFinalizadosMesmoItemSaida(IEnumerable<SugestaoPedidoCD> target)
        {
            foreach (var sugestaoPedidoCD in target)
            {
                if (m_existeSugestoesFinalizadasMesmoItemDetalheSaida(sugestaoPedidoCD.IDSugestaoPedidoCD))
                {
                    return true;
                }
            }

            return false;
        }
    }
}