﻿using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se um inventário pode ser preparado
    /// </summary>
    public class InventarioPodeSerPreparadoSpec : SpecBase<Inventario>
    {
        #region Fields
        private readonly IInventarioService m_inventarioService;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioPodeSerPreparadoSpec"/>.
        /// </summary>
        /// <param name="inventarioService">O serviço de inventário.</param>
        public InventarioPodeSerPreparadoSpec(IInventarioService inventarioService)
        {
            m_inventarioService = inventarioService;
        }
        #endregion

        /// <summary>
        /// Verifica se as datas especificadas satisfazem a especificação.
        /// </summary>
        /// <param name="target">O inventário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Inventario target)
        {
            var inventarios = m_inventarioService.ObterInventariosAbertosParaImportacao(target.IDLoja, null, target.IDDepartamento, target.IDCategoria);

            if (inventarios.Count() == 0)
            {
                return NotSatisfied(Texts.ThereAreNoOpenInventoryToPrepareInventory);
            }

            return Satisfied();
        }
    }
}
