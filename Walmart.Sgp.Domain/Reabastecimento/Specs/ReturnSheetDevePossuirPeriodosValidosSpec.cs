using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec para validar as datas de um ReturnSheet no seu cadastro.
    /// </summary>
    public class ReturnSheetDevePossuirPeriodosValidosSpec : SpecBase<ReturnSheet>
    {
        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O object.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ReturnSheet target)
        {
            if (target.DhInicioReturn > target.DhFinalReturn)
            {
                return NotSatisfied(Texts.RSStartDateCantBeBiggerThanFinal);
            }

            if (target.DhInicioEvento > target.DhFinalEvento)
            {
                return NotSatisfied(Texts.EventStartDateCantBeBiggerThanFinal);
            }

            if (target.DhFinalReturn.Date > target.DhInicioEvento)
            {
                return NotSatisfied(Texts.RSFinalDateCantBeBiggerThanEventStart);
            }

            return Satisfied();
        }
    }
}
