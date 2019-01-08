using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a se o horário militar informado é válido.
    /// </summary>
    public class ValidMilitaryTimeSpec : SpecBase<int>
    {
        /// <summary>
        /// Verifica se o int32 informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O int32.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo int32.
        /// </returns>
        public override SpecResult IsSatisfiedBy(int target)
        {
            if (target < 0)
            {
                return NotSatisfied(Texts.InvalidTime);
            }

            int minutos = target % 100;
            int horas = (target - minutos) / 100;

            if (horas > 23)
            {
                return NotSatisfied(Texts.InvalidTime);
            }

            if (minutos > 59)
            {
                return NotSatisfied(Texts.InvalidTime);
            }

            return Satisfied();
        }
    }
}
