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
    /// Filtro para adicionar informações de paginação no cabeçalho da resposta HTTP, caso presentes.
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class WebApiPagingFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (null != actionExecutedContext.Response)
            {
                var objectContent = actionExecutedContext.Response.Content as ObjectContent;

                if (null != objectContent)
                {
                    var paginated = objectContent.Value as IPaginated;

                    if (null != paginated && null != paginated.Paging)
                    {
                        // Garante que a query foi executada.
                        // (Necessário pois o filtro roda antes da serialização do IEnumerable, 
                        // fazendo com que o TotalCount possa ainda não ter sido determinado aqui)
                        paginated.GetEnumerator();

                        var orderBy = paginated.Paging.OrderBy;

                        if ("(select 1)" == orderBy)
                        {
                            orderBy = null;
                        }

                        actionExecutedContext.Response.Headers.Add("X-Paging-Total-Count", "{0}".With(paginated.TotalCount));
                        actionExecutedContext.Response.Headers.Add("X-Paging-Offset", "{0}".With(paginated.Paging.Offset));
                        actionExecutedContext.Response.Headers.Add("X-Paging-Limit", "{0}".With(paginated.Paging.Limit));
                        actionExecutedContext.Response.Headers.Add("X-Paging-Order-By", "{0}".With(orderBy));
                        actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Paging-Total-Count, X-Paging-Offset, X-Paging-Limit, X-Paging-Order-By");
                    }
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
