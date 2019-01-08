using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação referente a se um usuário pode remover permissões.
    /// </summary>
    public class UsuarioPodeRemoverPermissoesSpec : PermissaoServiceSpecBase<IRuntimeUser>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeRemoverPermissoesSpec"/>.
        /// </summary>
        /// <param name="permissaoService">O serviço de permissão.</param>
        public UsuarioPodeRemoverPermissoesSpec(IPermissaoService permissaoService)
            : base(permissaoService)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o usuário informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O usuário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo usuário.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IRuntimeUser target)
        {
            if (target.IsAdministrator)
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.UserShouldBeAdminToRemovePermissions);
        }
        #endregion
    }
}
