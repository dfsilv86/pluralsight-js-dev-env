using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Logging;

namespace Walmart.Sgp.Infrastructure.Web.Security
{
    /// <summary>
    /// Constantes utilizadas no contexto de segurança.
    /// </summary>
    public static class SecurityConstants
    {
        #region Constants
        /// <summary>
        /// O nome do emissor do JSON Web Token.
        /// </summary>
        public static readonly string TokenIssuer = "SGP";

        /// <summary>
        /// A url para identicar a audiência.
        /// </summary>
        public static readonly string TokenAudience = "http://localhost/sgp/allUsers";

        /// <summary>
        /// O tempo de vida do token.
        /// </summary>
        public static readonly double TokenLifetimeMinutes = 60;

        /// <summary>
        /// A chave utilizada na criptografia.
        /// </summary>
        internal static readonly byte[] TokenSecret = new byte[64];
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia os membros estáticos da classe <see cref="SecurityConstants"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations"), SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static SecurityConstants()
        {            
            var appSettings = ConfigurationManager.AppSettings;

            try
            {
                TokenLifetimeMinutes = Convert.ToInt32(appSettings["SGP:TokenLifetimeMinutes"], CultureInfo.InvariantCulture);

                // TODO: ler chave do CSP
                TokenSecret = Convert.FromBase64String(appSettings["SGP:TokenSecret"]);
            }
            catch (Exception ex)
            {
                var msg = Texts.ErrorReadingTokenConfig.With(ex.Message);
                LogService.Error(msg);

                throw new ConfigurationErrorsException(msg, ex);
            }
        }
        #endregion
    }
}
