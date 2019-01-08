using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Walmart.Sgp.Infrastructure.Web.Filters
{
    /// <summary>
    /// Filtro para expor erros na deserialização de parametros de actions da api, levantando exceptions antes do início da execução da action.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class WebApiModelStateEnforcerFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var statesWithErrors = actionContext.ModelState.Select(ms => new { Name = ms.Key, Errors = ms.Value.Errors });

                var firstStateWithExceptions = statesWithErrors.FirstOrDefault(ee => ee.Errors.Any(e => null != e.Exception));

                if (null != firstStateWithExceptions)
                {
                    // InvalidCastExceptions por causa de valores inválidos para FixedValues aparecem aqui
                    var firstErrorWithException = firstStateWithExceptions.Errors.FirstOrDefault(ee => null != ee.Exception);

                    throw firstErrorWithException.Exception;
                }
            }
        }
    }
}