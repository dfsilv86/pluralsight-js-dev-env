using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a se lista de percentuais são multiplos de 5.
    /// </summary>
    public class PercentuaisDevemSerMultiploDeCincoSpec : SpecBase<IEnumerable<decimal?>>
    {
        /// <summary>
        /// Verifica se a lista de percentuais informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A lista de percentuais.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pela lista.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<decimal?> target)
        {
            var valores = target.Where(t => t.HasValue).Select(q => q.Value);
            if (valores.Any(v => !(v % 5 == 0)))
            {
                return NotSatisfied(Texts.MultipleFivePercentuals);
            }

            return Satisfied();
        }
    }
}
