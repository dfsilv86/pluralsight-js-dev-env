using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec para validar se uma lista de sugestoes nao é nula
    /// </summary>
    public class SugestoesReturnSheetNaoPodeSerNullSpec : SpecBase<IEnumerable<SugestaoReturnSheet>>
    {
        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O object.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<SugestaoReturnSheet> target)
        {
            if (target == null || !target.Any())
            {
                return NotSatisfied(Texts.NoItensToBeSaved);
            }

            return Satisfied();
        }
    }
}
