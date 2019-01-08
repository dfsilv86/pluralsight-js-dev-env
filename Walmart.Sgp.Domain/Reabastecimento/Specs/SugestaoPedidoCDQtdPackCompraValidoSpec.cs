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
    /// Spec para validar SugestaoPedidoCD.QtdPackCompra
    /// </summary>
    public class SugestaoPedidoCDQtdPackCompraValidoSpec : SpecBase<SugestaoPedidoCD>
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
            if (target.qtdPackCompra < 0)
            {
                return NotSatisfied(Texts.InformedQtMustBeGreaterOrEqualToZero);
            }

            return Satisfied();
        }
    }
}
