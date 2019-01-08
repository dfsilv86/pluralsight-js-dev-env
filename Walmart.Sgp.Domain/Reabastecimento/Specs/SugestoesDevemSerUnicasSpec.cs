using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a se sugestoes devem ser unicas.
    /// </summary>
    public class SugestoesDevemSerUnicasSpec : SpecBase<IEnumerable<GradeSugestao>>
    {
        /// <summary>
        /// Verifica se o i enumerable informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O i enumerable.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo i enumerable.
        /// </returns>        
        public override SpecResult IsSatisfiedBy(IEnumerable<GradeSugestao> target)
        {
            foreach (var sugestao in target)
            {
                var duplicado = target.Any(s => !ReferenceEquals(s, sugestao) &&
                        s.IDBandeira == sugestao.IDBandeira &&
                        s.IDLoja == sugestao.IDLoja &&
                        s.IDDepartamento == sugestao.IDDepartamento);

                if (duplicado)
                {
                    return NotSatisfied(Texts.SuggestionGridAlreadyExists);
                }
            }

            return Satisfied();
        }
    }
}
