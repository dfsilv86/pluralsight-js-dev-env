using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Bootstrap
{
    /// <summary>
    /// Configuração para o setup da aplicação.
    /// </summary>
    public class SetupConfig
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SetupConfig"/>.
        /// </summary>
        /// <param name="appName">O nome da aplicação.</param>
        /// <param name="appVersion">A versão da aplicação.</param>
        public SetupConfig(string appName, string appVersion)
        {
            AppName = appName;
            AppVersion = appVersion;
            AppAbsolutePath = AppDomain.CurrentDomain.BaseDirectory;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome da aplicação.
        /// </summary>
        public string AppName { get; private set; }

        /// <summary>
        /// Obtém a versão da aplicação.
        /// </summary>
        public string AppVersion { get; private set; }

        /// <summary>
        /// Obtém ou define o caminho absoluto da aplicação.
        /// </summary>
        public string AppAbsolutePath { get; set; }

        /// <summary>
        /// Obtém ou define o domínio de e-mail padrão.
        /// </summary>
        public string EmailDomain { get; set; }

        /// <summary>
        /// Obtém ou define a factory para ILifetime utilizado na configuração de DI.
        /// </summary>
        public Func<ILifetime> LifetimeFactory { get; set; }
        #endregion
    }
}
