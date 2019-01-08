using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se inventario pode ser cancelado.
    /// </summary>
    public class InventarioPodeSerCanceladoSpec : SpecBase<Inventario>
    {
        /// <summary>
        /// Verifica se o inventario informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O inventario.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo inventario.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Inventario target)
        {
            return target.stInventario == InventarioStatus.Aberto ||
                   target.stInventario == InventarioStatus.Importado ||
                   target.stInventario == InventarioStatus.Aprovado
                ? Satisfied()
                : NotSatisfied(Texts.CannotCancelInventory);
        }
    }
}