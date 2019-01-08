using System;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;
using LightInject;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Bootstrap;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.IO.Importing;
using Walmart.Sgp.Infrastructure.IO.Importing.Inventario;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;
using Walmart.Sgp.Infrastructure.Web.FileVault;
using Walmart.Sgp.WebApi.Properties;

namespace Walmart.Sgp.WebApi.App_Start
{
    public static class LightInjectSetup
    {
        public static void ConfigureSetup(Setup setup)
        {
            // As configurações de DI foram movidas para dentro da classe Setup, pois precisavam ser compartilhadas
            // com o projeto Walmart.Sgp.Tasks. 
            // Aqui nesta classe ficaram apenas as configurações específicas para a WebApi.          
            setup.RegisterDIStartedCallback = (container, logSection) =>
            {
                logSection.Log("Enabling LightInject support to web api...");
                container.RegisterApiControllers();
                container.EnablePerWebRequestScope();
                container.EnableWebApi(GlobalConfiguration.Configuration);
            };
        }
    }
}