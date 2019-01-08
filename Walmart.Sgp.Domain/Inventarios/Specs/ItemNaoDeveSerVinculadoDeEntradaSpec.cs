using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se item nao deve ser vinculado de entrada.
    /// </summary>
    public class ItemNaoDeveSerVinculadoDeEntradaSpec : SpecBase<ItemDetalhe>
    {
        /// <summary>
        /// Verifica se o item detalhe informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O item detalhe.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo item detalhe.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ItemDetalhe target)
        {
            return target.TpVinculado == TipoVinculado.Entrada
                ? NotSatisfied(Texts.ItemNaoDeveSerVinculadoDeEntradaSpecReason)
                : Satisfied();
        }
    }
}