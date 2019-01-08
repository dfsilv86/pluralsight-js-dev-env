using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Web.Security
{
    /// <summary>
    /// Atributo utilizado para marcar actions que devem ter permissão de acesso.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    public class SecurityWebApiActionAttribute : AuthorizeAttribute
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SecurityWebApiActionAttribute"/>.
        /// </summary>
        /// <remarks>
        /// Se não possui uma action id, então todos os métodos de escrita (POST|PUT|DELETE) devem ter permissões cadastradas.
        /// </remarks>
        public SecurityWebApiActionAttribute()
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SecurityWebApiActionAttribute"/>.
        /// </summary>
        /// <param name="actionId">O id da action.</param>
        public SecurityWebApiActionAttribute(string actionId)
        {
            ActionId = actionId;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o id da ação segura.
        /// </summary>
        /// <remarks>
        /// Se não possui uma action id, então todos os métodos de escrita (POST|PUT|DELETE) devem ter permissões cadastradas.
        /// </remarks>
        public string ActionId { get; private set; }

        /// <summary>
        /// Obtém ou define um valor que indica se permite que uma ação de escrita (POST|PUT|DELETE) não tenha permissão, bastando estar autenticado para acessar.
        /// </summary>
        /// <remarks>
        /// Deve ser utilizado nos casos de requests com muitos argumentos que não podem ser por GET e foi necessário ser por POST.
        /// </remarks>
        public bool AllowWriteActionWithoutPermission { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Calls when an action is being authorized.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool canAccess = true;
            var actionDescriptor = actionContext.ActionDescriptor;

            // Se tiver o atributo AllowAnonymous na action ou controller, então não precisa validar permissões.
            if (!IsAnonymous(actionDescriptor))
            {
                var user = RuntimeContext.Current.User;

                // Não está autenticado.
                if (!user.IsAuthenticated)
                {
                    canAccess = false;
                }
                else
                {
                    var permissionName = ActionId;

                    if (IsWriteActionAutoPermission(permissionName, actionDescriptor))
                    {
                        permissionName = "{0}.{1}".With(actionContext.ControllerContext.ControllerDescriptor.ControllerName, actionDescriptor.ActionName);
                    }

                    // Se não possui um nome de permissão, então não é uma action de escrita e nem foi o utilizada o atributo diretamente.
                    canAccess = String.IsNullOrEmpty(permissionName) || user.HasPermission(permissionName);
                }
            }

            HandleCanAccess(actionContext, canAccess);
        }

        private void HandleCanAccess(HttpActionContext actionContext, bool canAccess)
        {
            if (canAccess)
            {
                IsAuthorized(actionContext);
            }
            else
            {
                // HTTP status code 401.
                HandleUnauthorizedRequest(actionContext);
            }
        }

        private bool IsAnonymous(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0
             || actionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0;
        }

        /// <summary>
        /// Verfica se o atributo não foi sobrecarregado na action e é uma action de escrita (POST|PUT|DELETE).
        /// </summary>
        /// <param name="permissionName">O nome da permissão.</param>
        /// <param name="actionDescriptor">A action.</param>
        /// <returns>True se deve ser utilizado como uma action de escrita.</returns>
        private bool IsWriteActionAutoPermission(string permissionName, HttpActionDescriptor actionDescriptor)
        {
            return !AllowWriteActionWithoutPermission
                    && String.IsNullOrEmpty(permissionName)
                    && actionDescriptor.SupportedHttpMethods.Any(
                    m =>
                        m.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                     || m.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase)
                     || m.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}
