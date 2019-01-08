using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Especificação referente a se usuário deve possuir permissões.
    /// </summary>
    public class UsuarioDevePossuirPermissaoManutencaoSpec : SpecBase<IRuntimeUser>
    {
        private readonly IPermissaoGateway m_permissaoGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioDevePossuirPermissaoManutencaoSpec" />.
        /// </summary>
        /// <param name="permissaoGateway">O data table gateway de permissão.</param>
        public UsuarioDevePossuirPermissaoManutencaoSpec(IPermissaoGateway permissaoGateway)
        {
            m_permissaoGateway = permissaoGateway;
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
            if (target.IsAdministrator)
            {
                return Satisfied();
            }

            try
            {
                m_permissaoGateway.ObterPermissoesDoUsuario(target.Id);
            }
            catch (InvalidOperationException)
            {
                return NotSatisfied(Texts.UserDoesNotHavePermission);    
            }

            return Satisfied();
        }
    }
}
