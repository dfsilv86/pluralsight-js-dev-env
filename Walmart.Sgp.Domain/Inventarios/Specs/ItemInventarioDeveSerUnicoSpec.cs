using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se item inventario deve ser unico.
    /// </summary>
    public class ItemInventarioDeveSerUnicoSpec : SpecBase<InventarioItem>
    {
        private readonly IInventarioItemGateway m_inventarioItemGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemInventarioDeveSerUnicoSpec"/>.
        /// </summary>
        /// <param name="inventarioItemGateway">O table data gateway para inventario item.</param>
        public ItemInventarioDeveSerUnicoSpec(IInventarioItemGateway inventarioItemGateway)
        {
            m_inventarioItemGateway = inventarioItemGateway;
        }

        /// <summary>
        /// Verifica se o inventario item informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O inventario item.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo inventario item.
        /// </returns>
        public override SpecResult IsSatisfiedBy(InventarioItem target)
        {
            var count = m_inventarioItemGateway.Count(
                "IDItemDetalhe = @IDItemDetalhe AND IDInventario = @IDInventario AND IDInventarioItem <> @IDInventarioItem",
                new
                {
                    target.IDItemDetalhe,
                    target.IDInventario,
                    target.IDInventarioItem
                });

            if (count > 0)
            {
                return NotSatisfied(Texts.ItemAlreadyExistingInInventory);
            }

            return Satisfied();
        }
    }
}