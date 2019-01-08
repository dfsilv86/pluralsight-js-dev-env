using System;
using System.Web.Http.ExceptionHandling;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Web.Logging
{
    /// <summary>
    /// Logger de exceptions para a WebApi
    /// </summary>
    public class WebApiExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// Registra o log.
        /// </summary>
        /// <param name="context">O contexto.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            ExceptionHelper.ThrowIfNull("context", context);

            var error = context.Exception;

            if (UserInvalidOperationException.Is(error))
            {
                LogService.Info("{0}: {1}", context.Request.RequestUri.AbsoluteUri, error.Message);
            }
            else
            {
                LogService.Error("{0}: {1}{2}{3}", context.Request.RequestUri.AbsoluteUri, error.Message, Environment.NewLine, error.StackTrace);
                LogException(error.InnerException);
            }

            base.Log(context);
        }

        private static void LogException(Exception error)
        {
            if (error != null)
            {
                LogService.Error("{0}{1}{2}", error.Message, Environment.NewLine, error.StackTrace);
                LogException(error.InnerException);
            }
        }
    }
}
