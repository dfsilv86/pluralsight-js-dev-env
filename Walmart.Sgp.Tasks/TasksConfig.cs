using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;
using Walmart.Sgp.Infrastructure.Bootstrap;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Tasks
{
    /// <summary>
    /// Configuração da Tasks.
    /// </summary>
    public class TasksConfig
    {
        #region Constructors
        static TasksConfig()
        {
#if DEV
            TasksVersion = "DEV";
#else
            TasksVersion = typeof(TasksConfig).Assembly.GetName().Version.ToString();
#endif
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a versão das Tasks.
        /// </summary>
        public static string TasksVersion { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza o registro da inicialização e configurações.
        /// </summary>
        /// <returns>O container da DI.</returns>
        public ServiceContainer Start()
        {
            var cfg = new SetupConfig("Tasks", TasksVersion);
            cfg.LifetimeFactory = () => new PerContainerLifetime();

            var setup = new Setup(cfg);                     
            setup.Run();

            return setup.DIContainer;
        }
        #endregion
    }
}
