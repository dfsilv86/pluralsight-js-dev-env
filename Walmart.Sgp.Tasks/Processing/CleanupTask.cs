using System;
using LightInject;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Tasks.Modules;

namespace Walmart.Sgp.Tasks.Processing
{
    /// <summary>
    /// Implementação de ITaskModule referente ao expurgo de dados antigos.
    /// </summary>
    public class CleanupTask : TaskModuleBase
    {
        private string m_createdMachineName;

        /// <summary>
        /// Realiza a configuração adicional da tarefa com informações oriundas da seção da tarefa no arquivo xml de configuração.
        /// </summary>
        /// <param name="configuration">O elemento de configuração da tarefa.</param>
        protected override void PerformConfigure(System.Xml.Linq.XElement configuration)
        {
            m_createdMachineName = "(local)";

            var processOrder = configuration.Element("processOrder");

            if (null != processOrder)
            {
                m_createdMachineName = processOrder.Attribute("createdMachineName").Value.ToString();
            }

            if (string.IsNullOrWhiteSpace(m_createdMachineName))
            {
                m_createdMachineName = null;
            }
            else if (m_createdMachineName.ToLower() == "(local)")
            {
                m_createdMachineName = Environment.MachineName;
            }
        }

        /// <summary>
        /// Executa a tarefa.
        /// </summary>
        /// <param name="container">O container DI.</param>
        /// <returns>
        /// True se ainda possui dados a serem processados, false no contrário.
        /// </returns>
        protected override bool PerformExecute(ServiceContainer container)
        {
            var originalContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext()
            {
                User = new MemoryRuntimeUser
                {
                    UserName = "backgroundprocess",
                    FullName = "Background Process",
                    RoleName = "Guest",
                }
            };

            var processingService = container.GetInstance<IProcessingService>();

            processingService.Cleanup(m_createdMachineName);

            return true;
        }
    }
}
