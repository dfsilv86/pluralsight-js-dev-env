using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Classe base para especificações que buscam permissões do usuario e status de inventário.
    /// </summary>
    public abstract class UsuarioPermissaoInventarioSpecBase : SpecBase<IRuntimeUser>
    {
        private readonly Inventario m_inventario;        

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPermissaoInventarioSpecBase"/>.
        /// </summary>
        /// <param name="inventario">O inventario.</param>        
        protected UsuarioPermissaoInventarioSpecBase(Inventario inventario)
        {
            m_inventario = inventario;            
        }

        /// <summary>
        /// Obtém o texto que indica o que não pode ser feito.
        /// </summary>
        protected abstract string NotSatisfiedReason { get; }

        /// <summary>
        /// Obtém o id da permissão.
        /// </summary>
        protected abstract string IdPermissao { get; }

        /// <summary>
        /// Obtém os status validos do inventário.
        /// </summary>
        protected abstract IEnumerable<InventarioStatus> StatusValidos { get; }

        /// <summary>
        /// Verifica se o usuario informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O usuario.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo usuario.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IRuntimeUser target)
        {
            return InventarioNoStatusCorreto() && UsuarioPossuiPermissao(target)
                ? Satisfied()
                : NotSatisfied(NotSatisfiedReason);
        }

        private bool UsuarioPossuiPermissao(IRuntimeUser target)
        {
            return target.HasPermission(IdPermissao);
        }

        private bool InventarioNoStatusCorreto()
        {            
            return StatusValidos.Contains(m_inventario.stInventario);
        }
    }
}