using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec SugestaoReturnPodeSalvarSpec
    /// </summary>
    public class SugestaoReturnPodeSalvarSpec : SpecBase<SugestaoReturnSheet>
    {
        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(SugestaoReturnSheet target)
        {
           if (DateTime.Now > target.ItemLoja.ItemPrincipal.ReturnSheet.DhFinalReturn)
           {
               return NotSatisfied(Texts.CantSaveRSclosed);
           }

            return Satisfied();
        }
    }
}
