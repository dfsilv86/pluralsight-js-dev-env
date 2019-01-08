using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Web.Filters
{
    /// <summary>
    /// Filtro para adicionar informações de versão no cabeçalho da resposta HTTP.
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class WebApiVersionFilterAttribute : ActionFilterAttribute
    {
        #region Fields
        private string m_version;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebApiVersionFilterAttribute"/>.
        /// </summary>
        /// <param name="version">A versão da web api.</param>
        public WebApiVersionFilterAttribute(string version)
        {
            m_version = version;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (null != actionExecutedContext.Response)
            {
                actionExecutedContext.Response.Headers.Add("X-Web-Api-Version", m_version);
            }

            base.OnActionExecuted(actionExecutedContext);
        }
        #endregion
    }
}
