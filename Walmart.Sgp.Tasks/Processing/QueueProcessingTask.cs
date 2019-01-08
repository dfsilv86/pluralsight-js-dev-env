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
    /// Implementação de ITaskModule referente a execução de uma tarefa relacionada a alerta.
    /// </summary>
    public class QueueProcessingTask : TaskModuleBase
    {
        private string m_serviceTypeName;
        private string m_serviceMethodName;
        private int? m_createdUserId;
        private string m_createdMachineName;

        /// <summary>
        /// Realiza a configuração adicional da tarefa com informações oriundas da seção da tarefa no arquivo xml de configuração.
        /// </summary>
        /// <param name="configuration">O elemento de configuração da tarefa.</param>
        protected override void PerformConfigure(System.Xml.Linq.XElement configuration)
        {
            m_serviceTypeName = null;
            m_serviceMethodName = null;
            m_createdUserId = null;
            m_createdMachineName = "(local)";

            var service = configuration.Element("service");
            var processOrder = configuration.Element("processOrder");

            if (null != service)
            {
                m_serviceTypeName = service.Attribute("type").Value.ToString();
                m_serviceMethodName = service.Attribute("methodName").Value.ToString();
            }

            if (null != processOrder)
            {
                int tmp2 = 0;
                string tmp = processOrder.Attribute("createdUserId").Value.ToString();

                if (null != tmp && int.TryParse(tmp, out tmp2))
                {
                    m_createdUserId = tmp2;
                }

                m_createdMachineName = processOrder.Attribute("createdMachineName").Value.ToString();
            }

            if (0 == m_createdUserId)
            {
                m_createdUserId = null;
            }

            if (string.IsNullOrWhiteSpace(m_serviceTypeName))
            {
                m_serviceTypeName = null;
            }

            if (string.IsNullOrWhiteSpace(m_serviceMethodName))
            {
                m_serviceMethodName = null;
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

            string workerName = Guid.NewGuid().ToString();

            var processingService = container.GetInstance<IProcessingService>();
            var usuarioService = container.GetInstance<IUsuarioService>();
            var papelService = container.GetInstance<IPapelService>();

            string ticket = processingService.GetNextTicketToRun(workerName, m_serviceTypeName, m_serviceMethodName, m_createdUserId, m_createdMachineName);

            if (null != ticket)
            {
                try
                {
                    var processOrder = processingService.GetDetailsByTicket(ticket);

                    TipoPermissao tipoPermissao = TipoPermissao.PorBandeira;

                    if (processOrder.Service.StoreId.HasValue)
                    {
                        tipoPermissao = TipoPermissao.PorLoja;
                    }

                    var user = usuarioService.ObterPorId(processOrder.CreatedUserId);
                    var papel = papelService.ObterPorId(processOrder.Service.RoleId);

                    // TODO: faltam as permissoes
                    RuntimeContext.Current = new MemoryRuntimeContext()
                    {
                        User = new MemoryRuntimeUser
                        {
                            Email = user.Email,
                            FullName = user.FullName,
                            IsAdministrator = papel.IsAdmin ?? false,
                            IsGa = papel.IsGa ?? false,
                            IsHo = papel.IsHo ?? false,
                            UserName = user.UserName,
                            Id = processOrder.CreatedUserId,
                            RoleId = processOrder.Service.RoleId,
                            StoreId = processOrder.Service.StoreId,
                            BandeiraId = processOrder.Service.BandeiraId,
                            TipoPermissao = tipoPermissao,
                        }
                    };

                    processingService.Run(ticket);
                }
                finally
                {
                    RuntimeContext.Current = originalContext;
                }
            }

            return null != ticket;
        }
    }
}
