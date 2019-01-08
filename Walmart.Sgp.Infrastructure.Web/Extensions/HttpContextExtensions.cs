using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Web.Extensions
{
    /// <summary>
    /// Extensions methods para HttpContext.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "HttpContext")]
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Obtém o primeiro arquivo enviado no request.
        /// </summary>
        /// <remarks>
        /// Caso atinja o tamanho máximo de upload, lança exceção com mensagem globalizada e amigável.
        /// Caso não tenha arquivo, lança exceção também.
        /// </remarks>
        /// <param name="context">O contexto http..</param>
        /// <returns>O arquivo.</returns>
        public static HttpPostedFile GetFirstFile(this HttpContext context)
        {
            HttpFileCollection files = null;

            try
            {
                files = context.Request.Files;
            }
            catch (HttpException ex)
            {
                var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                throw new UserInvalidOperationException(Texts.MaxUploadSizeReached.With(section.MaxRequestLength / 1024), ex);
            }

            if (files.AllKeys.Length == 0)
            {
                throw new UserInvalidOperationException(Texts.UploadFileNotFound);
            }

            return files[0];
        }
    }
}
