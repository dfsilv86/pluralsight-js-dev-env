using System;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Web.Optimization
{
    /// <summary>
    /// BundleTransform para o caso da otimização não estar habilitada, assim adiciona um hash de versão na query string 
    /// e evita que se o arquivo for alterado ainda entre no cache do navegador.
    /// <remarks>Código original: http://stackoverflow.com/a/26490575/956886 </remarks>
    /// </summary>
    public sealed class FileHashVersionBundleTransform : IBundleTransform, IDisposable
    {
        #region Fields
        private SHA256Managed m_sha2586 = new SHA256Managed();
        #endregion

        #region Methods
        /// <summary>
        /// Transforms the content in the <see cref="T:System.Web.Optimization.BundleResponse" /> object.
        /// </summary>
        /// <param name="context">The bundle context.</param>
        /// <param name="response">The bundle response.</param>
        public void Process(BundleContext context, BundleResponse response)
        {
            ExceptionHelper.ThrowIfNull("context", context);
            ExceptionHelper.ThrowIfNull("response", response);

            if (!context.EnableOptimizations)
            {
                foreach (var file in response.Files)
                {
                    if (!file.IncludedVirtualPath.Contains("?v="))
                    {
                        using (var fs = File.OpenRead(HostingEnvironment.MapPath(file.IncludedVirtualPath)))
                        {
                            var fileHash = m_sha2586.ComputeHash(fs);
                            var version = HttpServerUtility.UrlTokenEncode(fileHash);
                            file.IncludedVirtualPath = string.Concat(file.IncludedVirtualPath, "?v=", version);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            m_sha2586.Dispose();
        }
        #endregion
    }
}
