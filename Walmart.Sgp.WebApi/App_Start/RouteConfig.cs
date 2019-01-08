using System.Web.Mvc;
using System.Web.Routing;
using RouteMagic;

namespace Walmart.Sgp.WebApi.App_Start
{
    /// <summary>
    /// Configuração de rotas.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Realiza o registro das rotas.
        /// </summary>
        /// <param name="routes">A coleção de rotas.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var newPath = routes.MapRoute("new", "docs/{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            routes.Redirect(r => r.MapRoute("old", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional })).To(newPath);

            routes.Map(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}