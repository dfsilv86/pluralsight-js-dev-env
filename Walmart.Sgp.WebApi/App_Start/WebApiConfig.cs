using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using LightInject;
using Walmart.Sgp.Infrastructure.Bootstrap;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Web.Filters;
using Walmart.Sgp.Infrastructure.Web.Logging;
using Walmart.Sgp.Infrastructure.Web.Runtime;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.Infrastructure.Web.Serialization;
using Walmart.Sgp.WebApi.Properties;
using global::WebApi.OutputCache.Core.Cache;
using global::WebApi.OutputCache.V2;

namespace Walmart.Sgp.WebApi.App_Start
{
    /// <summary>
    /// Configuração da WEB API.
    /// </summary>
    public static class WebApiConfig
    {
        #region Constructors
        static WebApiConfig()
        {
#if DEV
            // Entrega sempre a mesma versão em DEV, pois não é necessário verificar versão nessa situação.
            ApiVersion = "DEV";
#else
            ApiVersion = typeof(WebApiConfig).Assembly.GetName().Version.ToString();
#endif
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a versão da Web Api.
        /// </summary>
        public static string ApiVersion { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza o registro da inicialização e configurações.
        /// </summary>
        /// <param name="config">A configuração http.</param>
        public static void Start(HttpConfiguration config)
        {
            var cfg = new SetupConfig("Web Api", ApiVersion);
            cfg.AppAbsolutePath = HostingEnvironment.MapPath(@"~\");
            cfg.EmailDomain = Settings.Default.EmailDomain;
            cfg.LifetimeFactory = () => new PerScopeLifetime();

            var setup = new Setup(cfg);
            LightInjectSetup.ConfigureSetup(setup);
            setup.RunEndingCallback = (section) =>
            {
                RegisterCors(config, section);
                RegisterRoutes(config, section);
                RegisterToken(config, section);
                RegisterFilters(config, section);
                RegisterCache(config, section);
                RegisterFormatters(config, section);
                RegisterModelBinders(config, section);
            };

            setup.Run();
        }               

        /// <summary>
        /// Realizada o registro do shutdown.
        /// </summary>
        public static void End()
        {
            using (var section = new LogSection("Web Api end"))
            {
                // From: http://weblogs.asp.net/scottgu/433194
                var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
                
                if (runtime == null)
                {
                    return;
                }
                
                var shutdownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                
                section.Log(shutdownMessage);
            }
        }

        private static void RegisterCors(HttpConfiguration config, LogSection webApiSection)
        {
            using (var section = new LogSection("Cors", webApiSection))
            {
                var origins = "*";
                var headers = "*";
                var methods = "*";
                var exposedHeaders = "content-disposition,x-web-api-version,x-file-vault-ticket-id,x-file-vault-ticket-created-date,x-paging-total-count,x-paging-offset,x-paging-limit,x-paging-order-by,x-processing-ticket,x-processing-state,x-processing-current-progress,x-processing-total-progress,x-processing-status-message,x-processing-name,x-processing-created-date,x-processing-modified-date";
                section.Log("Origins: {0}", origins);
                section.Log("Headers: {0}", headers);
                section.Log("Methods: {0}", methods);
                section.Log("Exposed headers: {0}", exposedHeaders);

                var cors = new EnableCorsAttribute(origins, headers, methods, exposedHeaders);
                config.EnableCors(cors);
            }
        }

        private static void RegisterRoutes(HttpConfiguration config, LogSection webApiSection)
        {
            webApiSection.Log("Registering routes");

            config.MapHttpAttributeRoutes();
            var routes = config.Routes;

            routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "{controller}/{id}",
                 defaults: new
                 {
                     id = RouteParameter.Optional
                 });
        }

        private static void RegisterToken(HttpConfiguration config, LogSection webApiSection)
        {
            // JSON Web Token.
            webApiSection.Log("Adding token validation delegating handler");
            config.MessageHandlers.Add(new TokenValidationDelegatingHandler());

            //// Devido ao ThreadStatic no backing field do Current, agora é o Global.asax que cria o WebApiRuntimeContext no BeginRequest()
            ////RuntimeContext.Current = new WebApiRuntimeContext();
        }

        private static void RegisterFilters(HttpConfiguration config, LogSection webApiSection)
        {
            webApiSection.Log("Adding filters");
            var filters = GlobalConfiguration.Configuration.Filters;
            filters.Add(new SecurityWebApiActionAttribute());
            filters.Add(new WebApiProcessingFilterAttribute());
            filters.Add(new WebApiErrorHandlingFilterAttribute());
            filters.Add(new WebApiVersionFilterAttribute(ApiVersion));
            filters.Add(new WebApiPagingFilterAttribute());
            filters.Add(new WebApiModelStateEnforcerFilterAttribute());

            config.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());
        }

        private static void RegisterCache(HttpConfiguration config, LogSection webApiSection)
        {
            webApiSection.Log("Registering output cache provider");
            var cacheConfig = config.CacheOutputConfiguration();
            cacheConfig.RegisterCacheOutputProvider(() => new MemoryCacheDefault());
            //// Deve ser habilitado se existir necessidade de isolar dados de cache por usuário.
            //// cacheConfig.RegisterDefaultCacheKeyGeneratorProvider(() => new CustomCacheKeyGenerator());
        }

        private static void RegisterFormatters(HttpConfiguration config, LogSection webApiSection)
        {
            webApiSection.Log("Setting json formatter");
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.JsonFormatter);
            formatters.Insert(0, new DefaultJsonFormatter());

            config.Services.Clear(typeof(IBodyModelValidator));
        }

        private static void RegisterModelBinders(HttpConfiguration config, LogSection webApiSection)
        {
            var services = GlobalConfiguration.Configuration.Services;
            webApiSection.Log("Inserting DateTime model binder provider");
            services.Insert(typeof(ModelBinderProvider), 0, new DateTimeModelBinderProvider());

            webApiSection.Log("Inserting FixedValues model binder provider");
            services.Insert(typeof(ModelBinderProvider), 0, new FixedValuesModelBinderProvider());
        }
        #endregion
    }
}
