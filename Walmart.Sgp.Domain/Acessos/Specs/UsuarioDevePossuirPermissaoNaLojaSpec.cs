using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação que valida se o usuário possui permissão à bandeira da loja.
    /// </summary>
    public class UsuarioDevePossuirPermissaoNaLojaSpec : SpecBase<UsuarioDevePossuirPermissaoNaLojaSpecParameter>
    {
        private readonly IBandeiraGateway m_bandeiraGateway;
        private readonly IPermissaoBandeiraGateway m_permissaoBandeiraGateway;
        private readonly IPermissaoLojaGateway m_permissaoLojaGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioDevePossuirPermissaoNaLojaSpec" />.
        /// </summary>
        /// <param name="bandeiraGateway">O data table gateway de bandeira.</param>
        /// <param name="permissaoBandeiraGateway">O data table gateway de permissão bandeira.</param>
        /// <param name="permissaoLojaGateway">O data table gateway de permissão loja.</param>
        public UsuarioDevePossuirPermissaoNaLojaSpec(IBandeiraGateway bandeiraGateway, IPermissaoBandeiraGateway permissaoBandeiraGateway, IPermissaoLojaGateway permissaoLojaGateway)
        {
            m_bandeiraGateway = bandeiraGateway;
            m_permissaoBandeiraGateway = permissaoBandeiraGateway;
            m_permissaoLojaGateway = permissaoLojaGateway;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(UsuarioDevePossuirPermissaoNaLojaSpecParameter target)
        {
            var usuario = target.Usuario;

            if (!usuario.IsAdministrator && !usuario.IsHo)
            {
                return NotSatisfied(Texts.userDoesNotHavePermissionStoreChain);
            }

            var possuiPermissaoLoja = false;
            var possuiPermissaoBandeira = false;
            var idUsuario = usuario.Id;
            var idLoja = target.IdLoja;

            possuiPermissaoLoja = m_permissaoLojaGateway.UsuarioPossuiPermissaoLoja(idUsuario, idLoja);

            if (!possuiPermissaoLoja)
            {
                var bandeira = m_bandeiraGateway.ObterPorIdLoja(idLoja);
                possuiPermissaoBandeira = m_permissaoBandeiraGateway.UsuarioPossuiPermissaoBandeira(idUsuario, bandeira.IDBandeira);
            }

            return possuiPermissaoLoja || possuiPermissaoBandeira ? Satisfied() : NotSatisfied(Texts.userDoesNotHavePermissionStoreChain);
        }
    }
}
