using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Resources;

namespace Walmart.Sgp.Infrastructure.Web.Extensions
{
    /// <summary>
    /// Extensions methods para HttpPostedFile.
    /// </summary>
    public static class HttpPostedFileExtensions
    {
        /// <summary>
        /// Transforma o HttpPostedFile em arquivopara ser utilizado no file vault.
        /// </summary>
        /// <param name="file">O arquivo postado.</param>
        /// <returns>O arquivo para entrada no file vault.</returns>
        public static IFile ToFileVault(this HttpPostedFile file)
        {
            return new HttpPostedFileAdapter(file);
        }
    }
}
