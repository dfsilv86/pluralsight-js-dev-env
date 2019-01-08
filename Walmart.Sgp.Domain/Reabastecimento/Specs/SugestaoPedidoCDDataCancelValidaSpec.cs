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
    /// Spec para validar data cancelamento SugestaoPedidoCD
    /// </summary>
    public class SugestaoPedidoCDDataCancelValidaSpec : SpecBase<SugestaoPedidoCD>
    {
        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(SugestaoPedidoCD target)
        {
            if (target.dtCancelamentoPedido <= target.dtEnvioPedido)
            {
                return NotSatisfied(Texts.SugestionCancelDateMustBeGreaterThanOriginal);
            }

            return Satisfied();
        }
    }
}
