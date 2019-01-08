using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se usuario pode editar item inventario.
    /// </summary>
    public class UsuarioPodeEditarItemInventarioSpec : SpecBase<IRuntimeUser>
    {
        private readonly Inventario m_inventario;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeEditarItemInventarioSpec"/>.
        /// </summary>
        /// <param name="inventario">O inventario.</param>
        public UsuarioPodeEditarItemInventarioSpec(Inventario inventario)
        {
            m_inventario = inventario;
        }

        /// <summary>
        /// Verifica se o usuário informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O usuário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo usuário.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IRuntimeUser target)
        {
            // ref bug #4138
            if (m_inventario.stInventario == InventarioStatus.Importado ||
                (target.IsGa && m_inventario.stInventario == InventarioStatus.Aprovado))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.ThisInventoryCannotBeChangedForItsStatusIs.With(m_inventario.stInventario.Description));
        }
    }
}