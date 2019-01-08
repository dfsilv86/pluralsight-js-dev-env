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
    /// Spec para validar data envio SugestaoPedidoCD
    /// </summary>
    public class SugestaoPedidoCDDataEnvioValidaSpec : SpecBase<SugestaoPedidoCD>
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
            if (target.dtEnvioPedido.Date > target.dtCancelamentoPedido.Date)
            {
                return NotSatisfied(Texts.SugestionSendingDateCantBeGreatThanCancel);
            }

            if (target.dtEnvioPedido.Date < DateTime.Now.Date)
            {
                return NotSatisfied(Texts.SendingDateCantBeLowerThanToday);
            }

            return Satisfied();
        }
    }
}
