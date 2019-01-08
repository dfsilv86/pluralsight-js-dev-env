using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se usuario pode aprovar inventario.
    /// </summary>
    public class UsuarioPodeAprovarInventarioSpec : UsuarioPermissaoInventarioSpecBase
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeAprovarInventarioSpec"/>
        /// </summary>
        /// <param name="inventario">O inventario.</param>
        public UsuarioPodeAprovarInventarioSpec(Inventario inventario)
            : base(inventario)
        {
        }

        /// <summary>
        /// Obtém o texto que indica o que não pode ser feito.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.CannotApproveInventory; }
        }

        /// <summary>
        /// Obtém o id da permissão.
        /// </summary>
        protected override string IdPermissao
        {
            get { return InventarioPermissoes.Aprovar; }
        }

        /// <summary>
        /// Obtém os status validos do inventário.
        /// </summary>
        protected override IEnumerable<InventarioStatus> StatusValidos
        {
            get { return new[] { InventarioStatus.Importado }; }
        }
    }
}