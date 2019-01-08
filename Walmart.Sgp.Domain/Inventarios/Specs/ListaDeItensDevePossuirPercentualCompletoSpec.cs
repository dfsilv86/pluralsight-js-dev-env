using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se lista de itens do tipo ItemDetalheCD tem percentual 100%.
    /// </summary>
    public class ListaDeItensDevePossuirPercentualCompletoSpec : SpecBase<IEnumerable<ItemDetalheCD>>
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
            if (target.Sum(idcd => idcd.vlPercentual) == 100)
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.IncompletePercentageListOfItemDetalheCD);
        }
    }
}
