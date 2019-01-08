using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se usuario pode finalizar inventario.
    /// </summary>
    public class UsuarioPodeFinalizarInventarioSpec : UsuarioPermissaoInventarioSpecBase
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeFinalizarInventarioSpec"/>
        /// </summary>
        /// <param name="inventario">O inventario.</param>
        public UsuarioPodeFinalizarInventarioSpec(Inventario inventario) : base(inventario)
        {
        }

        /// <summary>
        /// Obtém o texto que indica o que não pode ser feito.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.CannotFinishInventory; }
        }

        /// <summary>
        /// Obtém o id da permissão.
        /// </summary>
        protected override string IdPermissao
        {
            get { return InventarioPermissoes.Finalizar; }
        }

        /// <summary>
        /// Obtém os status validos do inventário.
        /// </summary>        
        protected override IEnumerable<InventarioStatus> StatusValidos
        {
            get { return new[] { InventarioStatus.Aprovado }; }
        }
    }
}