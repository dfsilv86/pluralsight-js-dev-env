using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Spec para validar se a data informada é real.
    /// </summary>
    public class DataValidaSpec : SpecBase<DateTime>
    {
        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(DateTime target)
        {
            if (target < new DateTime(1753, 01, 01))
            {
                return NotSatisfied(Texts.DateMustBeGreaterThan1753);
            }

            return Satisfied();
        }
    }
}
