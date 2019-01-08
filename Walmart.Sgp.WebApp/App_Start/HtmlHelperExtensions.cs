using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Walmart.Sgp.Infrastructure.Web.Extensions
{
    /// <summary>
    /// Extensões para HtmlHelper.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Emite o conteúdo de um arquivo localizado no servidor.
        /// </summary>
        /// <param name="html">O HtmlHelper.</param>
        /// <param name="serverPath">O caminho lógico para o arquivo.</param>
        /// <returns>O conteúdo do arquivo.</returns>
        public static IHtmlString RenderFile(this HtmlHelper html, string serverPath)
        {
            var filePath = HttpContext.Current.Server.MapPath(serverPath);

            var markup = File.ReadAllText(filePath);
            return new HtmlString(markup);
        }
    }
}