using System.Web.Mvc;

namespace Walmart.Sgp.WebApi.App_Start
{
    /// <summary>
    /// Configuração de filtros http.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Realiza o registro dos filtros globais.
        /// </summary>
        /// <param name="filters">A lista de filtros.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}