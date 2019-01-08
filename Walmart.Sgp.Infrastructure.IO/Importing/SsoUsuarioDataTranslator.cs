using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.IO.Importing;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Tradutor de dados de acesso de usuário oriundo do Single Sign-on para a estrutura do SGP.
    /// </summary>
    public sealed class SsoUsuarioDataTranslator : IDataTranslator<UsuarioAcessoInfo>
    {
        #region Fields
        private ISsoService m_ssoService;
        private SsoOptions m_options;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SsoUsuarioDataTranslator"/>.
        /// </summary>
        /// <param name="ssoService">O serviço de Single Sign-On</param>
        /// <param name="options">As opções do SSO.</param>
        public SsoUsuarioDataTranslator(ISsoService ssoService, SsoOptions options)
        {
            m_ssoService = ssoService;
            m_options = options;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Traduz os dados para o formato esperado.
        /// </summary>
        /// <returns>
        /// O resultado.
        /// </returns>
        public UsuarioAcessoInfo Translate()
        {
            var userName = m_options.UserName;
            var password = m_options.UserPassword;
            var profileCode = m_options.ProfileCode;

            var result = new UsuarioAcessoInfo
            {
                Usuario = m_ssoService.ObterUsuario(userName, password)
            };

            if (profileCode.HasValue)
            {
                result.Papel = m_ssoService.ObterPapel(profileCode.Value);
            }

            return result;
        }
        #endregion
    }
}
