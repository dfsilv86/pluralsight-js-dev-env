using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada
{
    /// <summary>
    /// Especificação referente a se os itens filhos de uma compra casada são validos.
    /// </summary>
    public class TipoRAItensDeveSerIgualAoPaiSpec : SpecBase<IEnumerable<ItemDetalhe>>
    {
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ItemDetalhe> target)
        {
            if (target.Where(i => (i.PaiCompraCasada.HasValue && i.PaiCompraCasada.Value) || (i.FilhoCompraCasada.HasValue && i.FilhoCompraCasada.Value))
                .Select(s => s.VlTipoReabastecimento.Value).GroupBy(g => g).Count() != 1)
            {
                return NotSatisfied(Texts.TpRAMusBeEquals);
            }

            return Satisfied();
        }
    }
}
