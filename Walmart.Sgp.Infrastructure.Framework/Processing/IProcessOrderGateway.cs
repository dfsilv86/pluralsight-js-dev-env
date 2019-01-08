using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Define a interface de um table data gateway para ProcessOrder.
    /// </summary>
    public interface IProcessOrderGateway : IDataGateway<ProcessOrder>
    {
        /// <summary>
        /// Obtém o detalhamento sobre o ticket de processamento.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <returns>O ProcessOrder.</returns>
        ProcessOrder GetProcessOrderDetail(string ticket);

        /// <summary>
        /// Obtém o próximo ticket para executar, marcando com o nome do worker.
        /// </summary>
        /// <param name="workerName">O nome do worker que irá executar o processo.</param>
        /// <param name="serviceTypeName">O nome do tipo de serviço que pode ser executado.</param>
        /// <param name="serviceMethodName">O nome do método de serviço que pode ser executado.</param>
        /// <param name="createdUserId">O id do usuário criador dos processamentos que podem ser executados.</param>
        /// <param name="machineName">O nome do servidor que irá executar o processo.</param>
        /// <returns>O ticket.</returns>
        string GetNextTicketToRun(string workerName, string serviceTypeName, string serviceMethodName, int? createdUserId, string machineName);

        /// <summary>
        /// Obtém os processamentos que devem aparecer na área de notificação.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="lastCheck">A data da última verificação.</param>
        /// <returns>Os processamentos que devem aparecer na área de notificação.</returns>
        IEnumerable<ProcessOrder> CheckNotifications(int userId, DateTime? lastCheck);

        /// <summary>
        /// Obtém todos os processamentos do usuário.
        /// </summary>
        /// <param name="currentUserId">O id do usuário efetuando a consulta.</param>
        /// <param name="isAdministrator">Se o usuário efetuando a consulta é administrador.</param>
        /// <param name="createdUserId">O id do usuário que criou o processo.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        IEnumerable<ProcessOrderModel> FindAllByUser(int currentUserId, bool isAdministrator, int? createdUserId, string processName, ProcessOrderState? state, Paging paging);

        /// <summary>
        /// Obtém todas os processamentos que podem ser removidos do sistema.
        /// </summary>
        /// <param name="cutoffDate">A data de corte.</param>
        /// <param name="createdMachineName">O nome do servidor, ou null para todos servidores.</param>
        /// <returns>Os processamentos.</returns>
        IEnumerable<ProcessOrder> FindAllReadyToBeRemoved(DateTime cutoffDate, string createdMachineName);
    }
}
