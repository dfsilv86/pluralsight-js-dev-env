using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Walmart.Sgp.WebApp.Models
{
    /// <summary>
    /// Model com informações que serão passadas a aplicação SPA.
    /// </summary>
    public class SpaConfigModel
    {
        #region Fields
        private static string s_apiHost = ConfigurationManager.AppSettings["api:Root"];
        private static string s_appVersion = typeof(SpaConfigModel).Assembly.GetName().Version.ToString();
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a url raiz da WEB API.
        /// </summary>
        public string ApiHost
        {
            get
            {
                return s_apiHost;
            }
        }

        /// <summary>
        /// Obtém a versão da app.
        /// <remarks>
        /// Essa configuração é utilizada para garantir que os templates do AngularJs sejam entregues atualizados quando a Web App for compilada.
        /// </remarks>
        /// </summary>
        public string AppVersion
        {
            get
            {
#if DEV
                // Entregua uma versão diferente a cada refresh da página principal da app em modo DEV,
                // assim evita cache durante o desenvolvimento.
                return "{0}.dev.{1:yyyy-MM-dd.HH-mm-ss}".With(s_appVersion, DateTime.Now);
#else
                // Entrega uma nova versão a cada build/publish em TST|HLG|PRD.
                return s_appVersion;
#endif
            }
        }

        /// <summary>
        /// Obtém a URL da página de visualização do SSRS.
        /// </summary>
        public string ReportViewerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SGP:ReportViewer:Url"];
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a aplicação angular deve exibir informações no console do browser.
        /// </summary>
        public bool EnableConsole
        {
            get
            {
                return ConfigurationManager.AppSettings["SGP:Diagnostics:EnableConsole"] == "true";
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a aplicação angular deve notificar via toast se algum erro foi logado no console.
        /// </summary>
        public bool NotifyOfConsoleErrors
        {
            get
            {
                return ConfigurationManager.AppSettings["SGP:Diagnostics:NotifyOfConsoleErrors"] == "true";
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a aplicação angular deve exibir todos os erros logados no console como toast de erro com informações adicionais.
        /// </summary>
        public bool ShowConsoleErrorsAsToast
        {
            get
            {
                return ConfigurationManager.AppSettings["SGP:Diagnostics:ShowConsoleErrorsAsToast"] == "true";
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a aplicação angular deve logar em console informações sobre a execução da aplicação.
        /// </summary>
        public bool LogDiagnosticMessagesToConsole
        {
            get
            {
                return ConfigurationManager.AppSettings["SGP:Diagnostics:LogDiagnosticMessagesToConsole"] == "true";
            }
        }

        /// <summary>
        /// Obtém o timeout de atividade para bloquear a tela.
        /// </summary>
        public object ActivityTimeout
        {
            get
            {
                return ConfigurationManager.AppSettings["SGP:ActivityTimeout"] ?? "20";
            }
        }

        #endregion
    }
}