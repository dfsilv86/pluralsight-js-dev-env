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
    /// Spec para validar se as datas da RS nao estao no passado.
    /// </summary>
    public class ReturnSheetCannotHavePastDatesSpec : SpecBase<ReturnSheet>
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
            var hoje = DateTime.Now.Date;
            if (target.Id == 0 && (target.DhInicioReturn <= hoje || target.DhFinalReturn <= hoje || target.DhInicioEvento <= hoje || target.DhFinalEvento.Date <= hoje))
            {
                return NotSatisfied(Texts.YouCantInformPastDates);
            }

            return Satisfied();
        }
    }
}
