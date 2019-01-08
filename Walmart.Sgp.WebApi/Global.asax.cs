using System;
using System.Configuration;
using System.Web.Http;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Web.Runtime;
using Walmart.Sgp.WebApi.App_Start;

namespace Walmart.Sgp.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Start);   
            
            DapperProxy.DefaultCommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SGP:CommandTimeout"]);
        }

        protected void Application_End()
        {
            WebApiConfig.End();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            RuntimeContext.Current = new WebApiRuntimeContext();
        }
    }
}
