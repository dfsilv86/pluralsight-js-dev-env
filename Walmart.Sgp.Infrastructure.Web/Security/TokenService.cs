using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Web.Security
{
    /// <summary>
    /// Serviço de infraestrutura pra JSON Web Token.
    /// </summary>
    public static class TokenService
    {
        #region Fields
        private static Dictionary<string, IRuntimeUser> s_runtimeUserCache = new Dictionary<string, IRuntimeUser>();
        #endregion

        #region Public methods
        /// <summary>
        /// Cria o JSON Web Token.
        /// </summary>
        /// <param name="user">O usuário para qual será criado o token.</param>
        /// <returns>O token.</returns>
        public static string CreateToken(IRuntimeUser user)
        {
            ExceptionHelper.ThrowIfNull("user", user);

            var claimList = ToClaims(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new InMemorySymmetricSecurityKey(SecurityConstants.TokenSecret);

            var token = tokenHandler.CreateToken(MakeSecurityTokenDescriptor(securityKey, claimList));

            RemoveFromCache(user);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Valida o JSON Web Token.
        /// </summary>
        /// <param name="token">O token.</param>
        /// <returns>Os claims obtidos do token.</returns>
        public static ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParams =
                new TokenValidationParameters()
                {
                    ValidIssuer = SecurityConstants.TokenIssuer,
                    ValidateIssuer = true,
                    IssuerSigningToken = new BinarySecretSecurityToken(SecurityConstants.TokenSecret),
                    ValidAudience = SecurityConstants.TokenAudience,
                    ValidateAudience = true
                };

            SecurityToken securityToken;

            return tokenHandler.ValidateToken(token, validationParams, out securityToken);
        }

        /// <summary>
        /// Converte os claims para o usuário.
        /// </summary>
        /// <param name="principal">Os claims.</param>
        /// <returns>O usuário.</returns>
        public static IRuntimeUser ConvertTo(ClaimsPrincipal principal)
        {
            ExceptionHelper.ThrowIfNull("principal", principal);

            var claimId = principal.Claims.FirstOrDefault(c => c.Type.Equals("SGP.id"));

            if (claimId == null)
            {
                return null;
            }

            var id = claimId.Value;
            AddToCache(id, principal.Claims);

            return s_runtimeUserCache[id];
        }
        #endregion

        #region Private methods
        private static SecurityTokenDescriptor MakeSecurityTokenDescriptor(SecurityKey securityKey, IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;

            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = SecurityConstants.TokenIssuer,
                AppliesToAddress = SecurityConstants.TokenAudience,
                Lifetime = new Lifetime(now, now.AddMinutes(SecurityConstants.TokenLifetimeMinutes)),
                SigningCredentials = new SigningCredentials(
                    securityKey,
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),
            };
        }

        private static IEnumerable<Claim> ToClaims(IRuntimeUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("SGP.id", user.Id.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("SGP.userName", user.UserName));
            claims.Add(new Claim("SGP.fullName", System.Web.HttpUtility.HtmlEncode(user.FullName ?? String.Empty)));
            claims.Add(new Claim("SGP.roleId", user.RoleId.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("SGP.tipoPermissao", ((int)user.TipoPermissao).ToString(CultureInfo.InvariantCulture)));
            if (user.RoleName != null)
            {
                claims.Add(new Claim("SGP.roleName", user.RoleName));
            }

            claims.Add(new Claim("SGP.isAdministrator", user.IsAdministrator.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("SGP.isGa", user.IsGa.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("SGP.isHo", user.IsHo.ToString(CultureInfo.InvariantCulture)));
            
            if (user.StoreId.HasValue)
            {
                claims.Add(new Claim("SGP.storeId", user.StoreId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            if (user.HasAccessToSingleStore)
            {
                claims.Add(new Claim("SGP.restrictStore", "true"));
            }
            
            AddActions(user, claims);
            claims.Add(new Claim("SGP.email", user.Email ?? String.Empty));

            return claims;
        }

        private static void AddActions(IRuntimeUser user, List<Claim> claims)
        {
            claims.Add(new Claim("SGP.actions", user.Actions == null ? String.Empty : string.Join(";", user.Actions.Select(a => a.Id))));
        }

        private static IRuntimeUser FromClaims(IEnumerable<Claim> claims)
        {
            var result = new MemoryRuntimeUser
            {
                Id = Convert.ToInt32(GetClaimValue(claims, "SGP.id"), CultureInfo.InvariantCulture),
                UserName = GetClaimValue(claims, "SGP.userName"),
                FullName = GetClaimValue(claims, "SGP.fullName"),
                RoleId = Convert.ToInt32(GetClaimValue(claims, "SGP.roleId"), CultureInfo.InvariantCulture),
                RoleName = GetClaimValue(claims, "SGP.roleName", false),
                Actions = ParseActions(claims),
                Email = GetClaimValue(claims, "SGP.email"),
                TipoPermissao = (TipoPermissao)Convert.ToInt32(GetClaimValue(claims, "SGP.tipoPermissao"))
            };

            var storeId = GetClaimValue(claims, "SGP.storeId", false);

            if (storeId != null)
            {
                result.StoreId = Convert.ToInt32(storeId, CultureInfo.InvariantCulture);
            }

            var isAdministrator = GetClaimValue(claims, "SGP.isAdministrator", false);
            if (isAdministrator != null)
            {
                result.IsAdministrator = Convert.ToBoolean(isAdministrator, CultureInfo.InvariantCulture);
            }

            var isGa = GetClaimValue(claims, "SGP.isGa", false);
            if (isGa != null)
            {
                result.IsGa = Convert.ToBoolean(isGa, CultureInfo.InvariantCulture);
            }

            var isHo = GetClaimValue(claims, "SGP.isHo", false);
            if (isHo != null)
            {
                result.IsHo = Convert.ToBoolean(isHo, CultureInfo.InvariantCulture);
            }

            return result;
        }     

        private static IEnumerable<UserActionInfo> ParseActions(IEnumerable<Claim> claims)
        {
            var claimValue = GetClaimValue(claims, "SGP.actions");

            if (String.IsNullOrEmpty(claimValue))
            {
                return new UserActionInfo[0];
            }

            return claimValue
                    .Split(';')
                    .Select(action => new UserActionInfo(action))
                    .ToList();
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string type, bool throwErrorNotExisting = true)
        {
            var claim = claims.SingleOrDefault(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (claim == null)
            {
                if (throwErrorNotExisting)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }

                return null;
            }

            return claim.Value;
        }

        private static void AddToCache(string id, IEnumerable<Claim> claims)
        {
            lock (s_runtimeUserCache)
            {
                if (!s_runtimeUserCache.ContainsKey(id))
                {
                    s_runtimeUserCache.Add(id, FromClaims(claims));
                }
            }
        }

        private static void RemoveFromCache(IRuntimeUser user)
        {
            lock (s_runtimeUserCache)
            {
                s_runtimeUserCache.Remove(user.Id.ToString(CultureInfo.InvariantCulture));
            }
        }
        #endregion
    }
}
