using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação referente a se usuário pode ter permissões criadas.
    /// </summary>
    public class UsuarioPodeTerPermissoesCriadasSpec : PermissaoServiceSpecBase<Usuario>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeTerPermissoesCriadasSpec" />.
        /// </summary>
        /// <param name="permissaoService">O serviço de permissão.</param>
        public UsuarioPodeTerPermissoesCriadasSpec(IPermissaoService permissaoService)
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
        public override SpecResult IsSatisfiedBy(Usuario target)
        {
            var qtdPermissoes = PermissaoService.ContarPermissoesPorUsuario(target.Id);

            if (qtdPermissoes > 0)
            {
                return NotSatisfied(Texts.UserAlreadyHasPermissions);
            }

            return Satisfied();
        }
        #endregion
    }
}
