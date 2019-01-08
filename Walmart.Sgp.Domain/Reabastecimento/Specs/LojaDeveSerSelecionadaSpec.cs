using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação responsável por validar se existem lojas selecionadas.
    /// </summary>
    public class LojaDeveSerSelecionadaSpec : SpecBase<ReturnSheetItemLojaSpecParameter>
    {
        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ReturnSheetItemLojaSpecParameter target)
        {
            var lojasPersistidasExcetoAlteradas = target.LojasPersistidas.Except(target.LojasAlteradas, new ReturnSheetItemLojaEqualityComparer());

            if (target.LojasAlteradas.All(x => !x.selecionado) && lojasPersistidasExcetoAlteradas.All(x => !x.blAtivo))
            {
                return NotSatisfied(Texts.StoreMustBeSelected);
            }

            return Satisfied();
        }
    }
}
