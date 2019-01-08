using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Walmart.Sgp.WebApp.Models;

namespace Walmart.Sgp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var browser = Request.Browser;

            var ies = new string[] { "ie", "internetexplorer" };

            if (ies.Contains(browser.Browser.ToLowerInvariant()))
            {
                double versao = (double)browser.MajorVersion + browser.MinorVersion;

                if (versao < 11)
                {
                    ViewBag.Browser = string.Format("Browser: {0}, MajorVersion: {1}, MinorVersion: {2}", browser.Browser, browser.MajorVersion, browser.MinorVersion);

                    return View("PleaseUpdate");
                }
            }

            // Isso aqui por causa do Angular inicializar incorretamente o $location
            // vide issue https://github.com/angular/angular.js/issues/11091
            // vide PR https://github.com/angular/angular.js/pull/14488
            // corrigido em 6 de junho na versão 1.5.6
            // pode ser removido aqui (e setado base href="~/" como era originalmente no Index.cshtml) caso Angular seja atualizado para >= 1.5.6
            string theRoot = Request.ApplicationPath;
            
            if (!theRoot.EndsWith("/"))
            {
                theRoot += "/";
            }

            string urlSegments = string.Join(string.Empty, this.Request.Url.Segments.ToArray());

            string currentBase = urlSegments;

            if (currentBase.Length >= theRoot.Length)
            {
                currentBase = currentBase.Substring(0, theRoot.Length);
            }

            if (currentBase != theRoot)
            {
                return Redirect(theRoot + "404"); // ?url={0}".With(Server.UrlEncode(urlSegments)));
            }

            // Caso o usuário tenha digitado a url inteira em maiusculo, isto aqui permite ao angular fazer o match entre a url e a rota corretamente
            ViewBag.BaseHref = currentBase;

            return View(new SpaConfigModel());
        }
    }
}