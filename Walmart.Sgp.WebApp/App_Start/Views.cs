using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Walmart.Sgp.WebApp.Models;

namespace Walmart.Sgp.WebApp.App_Start
{
    /// <summary>
    /// Cache de views html.
    /// </summary>
    public static class Views
    {
        /// <summary>
        /// Nome da chave no cache.
        /// </summary>
        public const string CachedViewsKey = "__CachedViews";

        /// <summary>
        /// Tempo de vida do cache das views, em segundos.
        /// </summary>
        public const int CacheDurationTotalSeconds = 60 * 10;

        /// <summary>
        /// Emite o conteúdo do cache.
        /// </summary>
        /// <remarks>O cache persiste por 10 minutos, ou até que um arquivo incluso no cache seja modificado.</remarks>
        /// <returns>Conteúdo do cache.</returns>
        public static IHtmlString Precache()
        {
            string cacheContent = null;
#if DEV
#else
            cacheContent = (string)HttpContext.Current.Cache[CachedViewsKey];

            if (null == cacheContent)
            {
                StringBuilder builder = new StringBuilder();

                builder.AppendFormat("<!-- View Cache: {0:dd/MM/yyyy HH:mm:ss} -->", DateTime.Now);
                
                SpaConfigModel config = new SpaConfigModel();
                var appVersion = config.AppVersion;

                var allHtmlFiles = GetAllCacheableViews();

                decimal? lastp = 0;

                for (int i = 0, total = allHtmlFiles.Length; i < total; i++)
                {
                    string filePath = allHtmlFiles[i].Item1;
                    string templateName = allHtmlFiles[i].Item2;
                    var pct = (decimal?)Math.Round((25m / total) * i);
                    bool emitpct = lastp != pct;
                    lastp = pct;

                    EmitView(builder, appVersion, filePath, templateName, emitpct ? pct : null);
                }

                cacheContent = builder.ToString();

                HttpContext.Current.Cache.Add(CachedViewsKey, cacheContent, new CacheDependency(allHtmlFiles.Select(f => f.Item1).ToArray()), Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, CacheDurationTotalSeconds), CacheItemPriority.AboveNormal, null);
            }
#endif
            return new MvcHtmlString(cacheContent);
        }
#if DEV
#else
        private static void EmitView(StringBuilder builder, string appVersion, string filePath, string templateName, decimal? pct)
        {
            builder.Append("<script type=\"text/ng-template\" id=\"");
            builder.Append(templateName);
            builder.Append("?v=");
            builder.Append(appVersion);
            builder.Append("\">");
            builder.Append(File.ReadAllText(filePath));
            builder.Append("</script>");
            if (pct.HasValue)
            {
                builder.Append("<script type=\"text/javascript\">document.getElementById(\"__startup_value__\").style.width = \"");
                builder.Append(pct);
                builder.Append("%\";</script>");
            }
        }

        private static Tuple<string, string>[] GetAllCacheableViews()
        {
            string commonRootPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Scripts\common");
            var commonHtmlFiles = Directory.EnumerateFiles(commonRootPath, "*.html", SearchOption.AllDirectories).Select(f => new Tuple<string, string>(f, "Scripts/common{0}".With(string.Join("/", f.Substring(commonRootPath.Length).Split(Path.DirectorySeparatorChar)))));

            string appRootPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Scripts\app");
            var appHtmlFiles = Directory.EnumerateFiles(appRootPath, "*.html", SearchOption.AllDirectories).Select(f => new Tuple<string, string>(f, "Scripts/app{0}".With(string.Join("/", f.Substring(appRootPath.Length).Split(Path.DirectorySeparatorChar)))));

            var allHtmlFiles = commonHtmlFiles.Concat(appHtmlFiles).ToArray();
            return allHtmlFiles;
        }
#endif
    }
}