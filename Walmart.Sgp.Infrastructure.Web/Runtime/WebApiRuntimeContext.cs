using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.Infrastructure.Web.Runtime
{
    /// <summary>
    /// Contexto de execução da WebApi.
    /// </summary>
    public class WebApiRuntimeContext : IRuntimeContext
    {
        #region Constants
        /// <summary>
        /// O nome do HTTP header que define se a web api deve retornar informações de debug nas respostas das requisições realizadas.
        /// </summary>
        public const string DebugHttpHeaderName = "X-WebApi-Debug"; // X- denota custom header
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebApiRuntimeContext"/>.
        /// </summary>
        public WebApiRuntimeContext()
        {
            Culture = new CultureInfo("pt-BR");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o usuário corrente.
        /// </summary>
        public IRuntimeUser User
        {
            get
            {
                var result = TokenService.ConvertTo(HttpContext.Current.User as ClaimsPrincipal);

                if (result == null || result.Id == 0)
                {
                    result = new MemoryRuntimeUser();
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a cultura.
        /// </summary>
        public CultureInfo Culture { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o request solicitou informações de debug.
        /// </summary>
        /// <param name="request">O request.</param>
        /// <returns>True se o debug foi solicitado.</returns>
        public static bool IsDebugEnabled(HttpRequestMessage request)
        {
            ExceptionHelper.ThrowIfNull("request", request);
            return request.Headers.Contains(DebugHttpHeaderName);
        }
        #endregion
    }
}
