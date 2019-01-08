using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Logging;

namespace Walmart.Sgp.Infrastructure.Web.Security
{
    /// <summary>
    /// O delegating handler responsável por validar o JSON Web Token.
    /// </summary>
    public class TokenValidationDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var localPath = request.RequestUri.LocalPath;

            // Não realiza a validação.
            if (request.Method == HttpMethod.Options || localPath.Contains("/Auth/") || localPath.Contains("/FileVault/"))
            {
                return base.SendAsync(request, cancellationToken);
            }

            string jwtToken;

            if (!TryRetrieveToken(request, out jwtToken))
            {
                return ResponseAuthroizationTokenNotFound(request);
            }

            return ResponseTokenIsValid(request, ref cancellationToken, jwtToken);
        }

        private static Task<HttpResponseMessage> ResponseAuthroizationTokenNotFound(HttpRequestMessage request)
        {
            var unauthorizedResponse = BuildResponseErrorMessage(request, HttpStatusCode.Forbidden, Texts.YouAreNotAuthenticated);

            return Task.FromResult(unauthorizedResponse);
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;

            IEnumerable<string> authHeaders;

            if (request.Headers.TryGetValues("Authorization", out authHeaders))
            {
                var bearerToken = authHeaders.ElementAt(0);

                if (bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = bearerToken.Substring(7);   
                }                
            }            

            return token != null;
        }

        private static HttpResponseMessage BuildResponseErrorMessage(HttpRequestMessage request, HttpStatusCode statusCode, string errorMessage)
        {
            var response = request.CreateErrorResponse(statusCode, errorMessage);

            var authenticateHeader = new AuthenticationHeaderValue("Bearer", "authorization_uri=\"none\",resource_id=\"audience\"");
            response.Headers.WwwAuthenticate.Add(authenticateHeader);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Headers", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "*");

            return response;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private Task<HttpResponseMessage> ResponseTokenIsValid(HttpRequestMessage request, ref CancellationToken cancellationToken, string jwtToken)
        {
            HttpStatusCode statusCode;
            string errorMessage = string.Empty;

            try
            {
                var principal = TokenService.ValidateToken(jwtToken);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                // HTTP Status code 403;
                statusCode = HttpStatusCode.Forbidden;
                errorMessage = Texts.YouDoNotHavePermissionToPerformOperation;
            }
            catch (Exception)
            {
                errorMessage = Texts.YouDoNotHavePermissionToPerformOperation;

                // HTTP Status code 403;
                statusCode = HttpStatusCode.Forbidden;
            }

            var failResponse = BuildResponseErrorMessage(request, statusCode, errorMessage);

            return Task.FromResult(failResponse);
        }
    }
}
