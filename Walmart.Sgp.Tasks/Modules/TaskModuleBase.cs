using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LightInject;
using TaskManager.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Tasks.Modules
{
    /// <summary>
    /// Classe base para implementações de ITaskModule.
    /// </summary>
    public abstract class TaskModuleBase : ITaskModule
    {
        #region Fields
        private string m_name;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TaskModuleBase"/>
        /// </summary>
        protected TaskModuleBase()
        {
            m_name = GetType().Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a configuração adicional da tarefa com informações oriundas da seção da tarefa no arquivo xml de configuração.
        /// </summary>
        /// <param name="configuration">O elemento de configuração da tarefa.</param>
        public void Configure(XElement configuration)
        {
            Log("Configuration started...");
            PerformConfigure(configuration);
            Log("Configuration done.");
        }

        /// <summary>
        /// Executa a tarefa.
        /// </summary>
        /// <returns>
        /// True se ainda possui dados a serem processados, false no contrário.
        /// </returns>
        public bool Execute()
        {            
            var config = new TasksConfig();

            using (var container = config.Start())
            {
                Log("Execution started...");
                var result = PerformExecute(container);
                Log("Execution finished. Has more work? {0}", result);

                container.GetInstance<ApplicationDatabases>().Wlmslp.Transaction.Commit();

                return result;
            }                       
        }

        /// <summary>
        /// Realiza a configuração adicional da tarefa com informações oriundas da seção da tarefa no arquivo xml de configuração.
        /// </summary>
        /// <param name="configuration">O elemento de configuração da tarefa.</param>
        protected virtual void PerformConfigure(XElement configuration)
        {
        }

        /// <summary>
        /// Executa a tarefa.
        /// </summary>
        /// <param name="container">O container DI.</param>
        /// <returns>
        /// True se ainda possui dados a serem processados, false no contrário.
        /// </returns>
        protected abstract bool PerformExecute(ServiceContainer container);

        /// <summary>
        /// Registra log da mensagem informada.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="args">Os argumentos da mensagem.</param>
        protected void Log(string message, params object[] args)
        {
            Console.WriteLine("[{0}] {1}", m_name, message.With(args));
        }
        #endregion
    }
}
