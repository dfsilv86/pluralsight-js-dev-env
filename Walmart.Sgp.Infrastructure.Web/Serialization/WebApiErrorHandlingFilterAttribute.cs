using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Filters;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.Web.Logging;
using Walmart.Sgp.Infrastructure.Web.Runtime;

namespace Walmart.Sgp.Infrastructure.Web.Serialization
{
    /// <summary>
    /// Filtro utilizado pela web api para retornar uma mensagem de erro customizada quando um exceção acontece.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WebApiErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        #region Public Methods
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">Filter context.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ExceptionHelper.ThrowIfNull("actionExecutedContext", actionExecutedContext);

            if (actionExecutedContext.Exception != null)
            {
                // To avoid memory leak as described here: http://stackoverflow.com/a/20762570/956886
                if (actionExecutedContext.Response != null)
                {
                    actionExecutedContext.Response.Dispose();
                }

                var exception = actionExecutedContext.Exception;

                if (exception is TargetInvocationException && null != exception.InnerException)
                {
                    exception = exception.InnerException;
                }

                var httpResponseException = exception as HttpResponseException;
                int code = 500;
                string message = exception.Message;
                var data = exception.Data;

                if (null != httpResponseException)
                {
                    HttpResponseMessage responseMessage = httpResponseException.Response;
                    message = responseMessage.ReasonPhrase;
                    code = (int)responseMessage.StatusCode;
                } 
                else if (UserInvalidOperationException.Is(exception))
                {                    
                    code = 400;
                }
                else if (exception is NotImplementedException)
                {
                        code = 501;
                }

                if (exception.Message == Texts.MultipleDeleteNotSupporteOnDapperGatewayWithChildrenEntities1 || exception.Message == Texts.MultipleInsertNotSupporteOnDapperGatewayWithChildrenEntities)
                {
                    code = 409;
                }

               CreateResponse(actionExecutedContext, exception, code, message, data);
            }
        }

        private static void CreateResponse(HttpActionExecutedContext actionExecutedContext, Exception exception, int code, string message, IDictionary data)
        {
#if PRD
            bool enableDebug = false;
#else
            bool enableDebug = true;
#endif

            if (enableDebug || WebApiRuntimeContext.IsDebugEnabled(actionExecutedContext.Request))
            {
                if (exception is AggregateException)
                {
                    var aggregateException = (AggregateException)exception;

                    message = "{0}: {1}".With(Texts.MultipleExceptionsMessage, string.Join("; ", aggregateException.InnerExceptions.Select(ex => ex.GetBaseException().Message).ToArray()));

                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse((HttpStatusCode)code, new { code = code, message = message, stackTrace = exception.StackTrace, exceptions = aggregateException.InnerExceptions.Select(iex => new { Message = iex.Message, StackTrace = iex.StackTrace, InnerException = iex.InnerException }), data = data }, new JsonMediaTypeFormatter());
                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse((HttpStatusCode)code, new { code = code, message = message, stackTrace = exception.StackTrace, data = data }, new JsonMediaTypeFormatter());
                }
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse((HttpStatusCode)code, new { code = code, message = message, data = data }, new JsonMediaTypeFormatter());
            }
        }
        #endregion
    }
}
