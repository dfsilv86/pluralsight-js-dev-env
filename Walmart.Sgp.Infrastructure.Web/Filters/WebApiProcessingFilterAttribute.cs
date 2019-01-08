using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Web.Filters
{
    /// <summary>
    /// Filtro para adicionar informações de paginação no cabeçalho da resposta HTTP, caso presentes.
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class WebApiProcessingFilterAttribute : ActionFilterAttribute
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
                    var processResult = objectContent.Value as ProcessOrderResult;

                    if (null != processResult)
                    {
                        actionExecutedContext.Response.Headers.Add("x-processing-ticket", processResult.ProcessOrder.Ticket);
                        actionExecutedContext.Response.Headers.Add("x-processing-name", processResult.ProcessOrder.ProcessName);
                        actionExecutedContext.Response.Headers.Add("x-processing-state", processResult.ProcessOrder.State.ToString());
                        actionExecutedContext.Response.Headers.Add("x-processing-current-progress", "{0}".With(processResult.ProcessOrder.CurrentProgress));
                        actionExecutedContext.Response.Headers.Add("x-processing-created-date", "{0:s}Z".With(processResult.ProcessOrder.CreatedDate.ToUniversalTime()));

                        if (processResult.ProcessOrder.ModifiedDate.HasValue)
                        {
                            actionExecutedContext.Response.Headers.Add("x-processing-modified-date", "{0:s}Z".With(processResult.ProcessOrder.ModifiedDate.Value.ToUniversalTime()));
                        }

                        actionExecutedContext.Response.Headers.Add("x-processing-total-progress", "{0}".With(processResult.ProcessOrder.TotalProgress));
                        if (null != processResult.ProcessOrder.Message)
                        {
                            actionExecutedContext.Response.Headers.Add("x-processing-status-message", processResult.ProcessOrder.Message.LimitTo(1024));
                        }

                        actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", "x-processing-ticket, x-processing-name, x-processing-state, x-processing-current-progress, x-processing-total-progress, x-processing-status-message, x-processing-created-date, x-processing-modified-date");

                        if (processResult.ProcessOrder.State == ProcessOrderState.Error)
                        {
                            actionExecutedContext.Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                            objectContent.Value = new { Code = 500, Message = processResult.ProcessOrder.Message };
                        }
                        else if ((int)processResult.ProcessOrder.State < (int)ProcessOrderState.Failed)
                        {
                            objectContent.Value = null;
                        }
                        else if (processResult.ProcessOrder.State == ProcessOrderState.Failed)
                        {
                            actionExecutedContext.Response.Headers.Remove("x-processing-status-message");
                            string msg = Texts.ProcessOrderStateFixedValueFailed.With(processResult.ProcessOrder.Message ?? Texts.NotDefined, null);
                            actionExecutedContext.Response.Headers.Add("x-processing-status-message", msg.LimitTo(1024));
                            actionExecutedContext.Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                            objectContent.Value = new { Code = 500, Message = processResult.ProcessOrder.Message };
                            ////throw new Exception(processResult.BackgroundProcess.Message);
                        }
                        else
                        {
                            var resultInstance = processResult.UnwrapResult();
                            objectContent.Value = resultInstance;
                            actionExecutedContext.Response.Headers.Add("x-processing-result-type-name", resultInstance.GetType().Name);

                            var fvt = resultInstance as FileVaultTicket;
                            if (null != fvt)
                            {
                                actionExecutedContext.Response.Headers.Add("x-file-vault-ticket-id", HttpUtility.UrlEncode(fvt.Id));
                                actionExecutedContext.Response.Headers.Add("x-file-vault-ticket-created-date", fvt.CreatedDate.ToUniversalTime().ToString("o"));
                            }
                        }
                    }
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
