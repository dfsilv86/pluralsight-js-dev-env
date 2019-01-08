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
    /// Especificação referente a se usuário deve possuir permissões.
    /// </summary>
    public class UsuarioDevePossuirPermissoesSpec : PermissaoServiceSpecBase<Usuario>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioDevePossuirPermissoesSpec" />.
        /// </summary>
        /// <param name="permissaoService">O serviço de permissão.</param>
        public UsuarioDevePossuirPermissoesSpec(IPermissaoService permissaoService)
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

            if (qtdPermissoes == 0)
            {
                return NotSatisfied(Texts.PermissionsDoNotDefinedForUser);
            }

            return Satisfied();
        }
        #endregion
    }
}
