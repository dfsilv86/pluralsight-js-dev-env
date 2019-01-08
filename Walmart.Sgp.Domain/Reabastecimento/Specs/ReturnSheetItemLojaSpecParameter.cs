using System.Collections.Generic;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Representa um parâmetro para as especificações que validam ReturnSheetItemLoja oriúndos da interface e da persistência.
    /// </summary>
    public class ReturnSheetItemLojaSpecParameter
    {
        /// <summary>
        /// Obtém ou define LojasAlteradas.
        /// </summary>
        public IEnumerable<ReturnSheetItemLoja> LojasAlteradas { get; set; }

        /// <summary>
        /// Obtém ou define LojasPersistidas.
        /// </summary>
        public IEnumerable<ReturnSheetItemLoja> LojasPersistidas { get; set; }
    }
}
