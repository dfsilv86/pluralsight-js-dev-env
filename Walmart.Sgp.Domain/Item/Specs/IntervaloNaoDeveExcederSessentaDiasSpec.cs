using System;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item.Specs
{
    /// <summary>
    /// Especificação referente a se o intervalo de datas não excede 60 dias.
    /// </summary>
    public class IntervaloNaoDeveExcederSessentaDiasSpec : SpecBase<RangeValue<DateTime>>
    {
        /// <summary>
        /// Verifica se as datas especificadas satisfazem a especificação.
        /// </summary>
        /// <param name="target">As datas.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelas datas.
        /// </returns>
        public override SpecResult IsSatisfiedBy(RangeValue<DateTime> target)
        {
            int intervalo = target.EndValue.Value.Subtract(target.StartValue.Value).Days;

            if (intervalo > 60)
            {
                return NotSatisfied(Texts.IntervalMustBeLessThanSixtyDays);
            }

            return Satisfied();
        }
    }
}
