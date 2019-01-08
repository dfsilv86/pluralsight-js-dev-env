using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item.Specs
{
    /// <summary>
    /// Especificação referente a se informacoes de estoque podem ser atualizadas.
    /// </summary>
    public class InformacoesDeEstoquePodemSerAtualizadasSpec : SpecBase<ItemDetalhe>
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
            if (target.RecebeNota)
            {
                return new AllMustBeInformedSpec().IsSatisfiedBy(new { ConvertionFactor = target.VlFatorConversao });
            }

            if (target.VlFatorConversao.HasValue && target.VlFatorConversao.Value != 1f)
            {
                return NotSatisfied(Texts.CannotSetAConversionFactorPleaseInformOne);
            }

            return Satisfied();
        }
    }
}