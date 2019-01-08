using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Spec para Todas datas serem maiores que agora.
    /// </summary>
    public class AllDatesMustBeGreaterThanNowSpec : SpecBase<DateTime[]>
    {
        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O object.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(DateTime[] target)
        {
            var d = DateTime.Now;
            foreach (var t in target)
            {
                if (t < d)
                {
                    return NotSatisfied(Texts.YouCantInformPastDates);
                }
            }

            return Satisfied();
        }
    }
}
