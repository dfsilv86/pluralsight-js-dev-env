using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Define a interface de um serviço de background process.
    /// </summary>
    public interface IProcessingService
    {
        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <param name="serviceType">O tipo do serviço, conforme registrado na injeção de dependência.</param>
        /// <param name="methodName">O nome do método do serviço.</param>
        /// <param name="parameters">Os parâmetros do serviço.</param>
        /// <param name="exposedParameterNames">A lista de parâmetros considerados expostos (que podem ser exibidos em tela em uma visualização rápida do pedido de execução).</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        /// <example>
        /// IBackgroundProcessService backgroundProcessService;
        /// Dictionary&lt;string, object&gt; params = new Dictionary&lt;string, object&gt;();
        /// params["param1"] = 1;
        /// params["param2"] = DateTime.Now;
        /// var result = backgroundProcessService.Dispatch(typeof(IFooService), "DoSomething", params, new string[] { "param1" });
        /// return result;
        /// </example>
        ProcessOrderResult Dispatch(Type serviceType, string methodName, IReadOnlyDictionary<string, object> parameters, IReadOnlyList<string> exposedParameterNames);

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <typeparam name="TService">O tipo do serviço, conforme registrado na injeção de dependência.</typeparam>
        /// <typeparam name="TResult">O tipo do retorno do serviço.</typeparam>
        /// <param name="serviceCall">A chamada do serviço.</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        /// <example>
        /// IBackgroundProcessService backgroundProcessService;
        /// int param1;
        /// var result = backgroundProcessService.Dispatch((IFooService fooService) => fooService.DoSomething(param1, DateTime.Now));
        /// return result;
        /// </example>
        ProcessOrderResult<TResult> Dispatch<TService, TResult>(Expression<Func<TService, TResult>> serviceCall);

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <typeparam name="TResult">O tipo do retorno do serviço.</typeparam>
        /// <param name="serviceCall">A chamada do serviço.</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        /// <example>
        /// IBackgroundProcessService backgroundProcessService;
        /// int param1;
        /// var result = backgroundProcessService.Dispatch(() => this.MainService.DoSomething(param1, DateTime.Now));
        /// return result;
        /// </example>
        ProcessOrderResult<TResult> Dispatch<TResult>(Expression<Func<TResult>> serviceCall);

        /// <summary>
        /// Obtém o resultado do processamento, caso este tenha terminado sua execução (com ou sem erros).
        /// </summary>
        /// <param name="ticket">O ticket de processamento.</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        ProcessOrderResult GetProcessingResults(string ticket);

        /// <summary>
        /// Dispara a execução imediata do processamento.
        /// </summary>
        /// <param name="ticket">O ticket do processo.</param>
        /// <remarks>Para uso interno do executor de tarefas.</remarks>
        /// <returns>O processamento atualizado.</returns>
        ProcessOrder Run(string ticket);

        /// <summary>
        /// Obtém todos os processamentos do usuário.
        /// </summary>
        /// <param name="createdUserId">O id do usuário.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        IEnumerable<ProcessOrderModel> FindAllByUser(int? createdUserId, string processName, ProcessOrderState? state, Paging paging);

        /// <summary>
        /// Obtém os processamentos que devem aparecer na área de notificação.
        /// </summary>
        /// <param name="lastCheck">A data da última verificação.</param>
        /// <returns>Os processamentos que devem aparecer na área de notificação.</returns>
        IEnumerable<ProcessOrder> CheckNotifications(DateTime? lastCheck);

        /// <summary>
        /// Obtém o resumo do processamento.
        /// </summary>
        /// <param name="ticket">O ticket de processamento.</param>
        /// <returns>O processamento.</returns>
        ProcessOrderSummary GetByTicket(string ticket);

        /// <summary>
        /// Obtém os detalhes do processamento.
        /// </summary>
        /// <param name="ticket">O ticket de processamento.</param>
        /// <returns>O processamento detalhado.</returns>
        ProcessOrder GetDetailsByTicket(string ticket);

        /// <summary>
        /// Obtém os logs do processamento.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os logs.</returns>
        IEnumerable<AuditRecord<ProcessOrder>> GetLogsByTicket(string ticket, Paging paging);

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
        /// Obtém uma lista de nomes de processos conhecidos.
        /// </summary>
        /// <param name="createdUserId">O id do usuário criador.</param>
        /// <returns>
        /// Os nomes de processos e suas respectivas descrições.
        /// </returns>
        IEnumerable<object> GetProcessNames(int? createdUserId);

        /// <summary>
        /// Remove ordens de execução antigas.
        /// </summary>
        /// <param name="createdMachineName">O nome do servidor, ou null para todos servidores.</param>
        void Cleanup(string createdMachineName);
    }
}
