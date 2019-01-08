using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação responsável por validar se todas as lojas selecionadas possuem um preço de venda definido.
    /// </summary>
    public class PrecoVendaDeveSerInformadoSpec : SpecBase<IEnumerable<ReturnSheetItemLoja>>
    {
        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ReturnSheetItemLoja> target)
        {
            if (target.Any(x => x.selecionado && (!x.PrecoVenda.HasValue || (x.PrecoVenda.HasValue && x.PrecoVenda.Value == 0))))
            { 
                return NotSatisfied(Texts.PriceSaleMustBeInformed);
            }

            return Satisfied();
        }
    }
}
