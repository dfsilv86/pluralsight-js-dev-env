using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: ListaDeItensNaoPodeTerItemCemPorcentoSpec.
    /// </summary>
    public class ListaDeItensNaoPodeTerItemCemPorcentoSpec : SpecBase<IEnumerable<ItemDetalheCD>>
    {
        /// <summary>
        /// Verifica se a lista de itens informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A lista de ItemDetalheCD.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pela lista.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ItemDetalheCD> target)
        {
            if (target.Any(i => i.vlPercentual > 99))
            {
                return NotSatisfied(Texts.NotAllowedToHaveOneItemHundredPercent);
            }

            return Satisfied();
        }
    }
}
