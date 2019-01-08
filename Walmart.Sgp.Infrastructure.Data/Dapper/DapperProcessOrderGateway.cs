using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para process order utilizando o Dapper.
    /// </summary>
    public class DapperProcessOrderGateway : EntityDapperDataGatewayBase<ProcessOrder>, IProcessOrderGateway
    {
        #region Fields
        private readonly IProcessOrderArgumentGateway m_processOrderArgumentGateway;
        private readonly IProcessOrderServiceGateway m_processOrderServiceGateway;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperProcessOrderGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        /// <param name="processOrderArgumentGateway">O gateway de ProcessOrderArgument.</param>
        /// <param name="processOrderServiceGateway">O gateway de ProcessOrderService.</param>
        public DapperProcessOrderGateway(ApplicationDatabases databases, IProcessOrderArgumentGateway processOrderArgumentGateway, IProcessOrderServiceGateway processOrderServiceGateway)
            : base(databases.Wlmslp, "ProcessOrder", "ProcessOrderId")
        {
            m_processOrderArgumentGateway = processOrderArgumentGateway;
            m_processOrderServiceGateway = processOrderServiceGateway;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperProcessOrderGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperProcessOrderGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ProcessOrder", "ProcessOrderId")
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get { return new string[] { "Ticket", "ProcessName", "State", "CurrentProgress", "TotalProgress", "Message", "CreatedDate", "ModifiedDate", "CreatedUserId", "ModifiedUserId", "ExecuteAfter", "StartDate", "EndDate", "WorkerName", "CreatedMachineName" }; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public override void Insert(ProcessOrder entity)
        {
            base.Insert(entity);

            entity.Service.ProcessOrderId = entity.ProcessOrderId;

            m_processOrderServiceGateway.Insert(entity.Service);

            ChildrenHelper.Insert<ProcessOrderArgument>(
                entity.Arguments,
                this.m_processOrderArgumentGateway,
                (a) =>
                {
                    a.ProcessOrderId = entity.ProcessOrderId;
                });
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <remarks>
        /// Será atualizada a entidade que possui o Id informado no modelo.
        /// </remarks>
        public override void Update(ProcessOrder entity)
        {
            base.Update(
                "CurrentProgress=@CurrentProgress,EndDate=@EndDate,Message=@Message,ModifiedDate=@ModifiedDate,ModifiedUserId=@ModifiedUserId,StartDate=@StartDate,State=@State,TotalProgress=@TotalProgress,WorkerName=@WorkerName",
                "ProcessOrderId=@ProcessOrderId",
                new 
                { 
                    entity.ProcessOrderId,
                    entity.CurrentProgress,
                    entity.EndDate,
                    entity.Message,
                    entity.ModifiedDate,
                    entity.ModifiedUserId,
                    entity.StartDate,
                    entity.State,
                    entity.TotalProgress,
                    entity.WorkerName,
                });

            entity.Service.ProcessOrderId = entity.ProcessOrderId;

            m_processOrderServiceGateway.Update(entity.Service);
        }

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>
        public override void Delete(int id)
        {
            m_processOrderServiceGateway.Delete("ProcessOrderId=@id", new { id });
            m_processOrderArgumentGateway.Delete("ProcessOrderId=@id", new { id });
            base.Delete(id);
        }

        /// <summary>
        /// Obtém o detalhamento sobre o ticket de processamento.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <returns>O ProcessOrder.</returns>
        public ProcessOrder GetProcessOrderDetail(string ticket)
        {
            Dictionary<int, ProcessOrderModel> result = new Dictionary<int, ProcessOrderModel>();

            Resource.Query<ProcessOrder, ProcessOrderService, MemoryRuntimeUser, ProcessOrderArgument, ProcessOrderModel>(
                Sql.ProcessOrder.GetProcessOrderDetail,
                new { ticket },
                (po, pos, user, poa) => MapProcessOrderArgument(result, po, pos, user, poa),
                "SplitOn1,SplitOn2,SplitOn3").Perform();

            return result.Values.SingleOrDefault();
        }

        /// <summary>
        /// Obtém o próximo ticket para executar, marcando com o nome do worker.
        /// </summary>
        /// <param name="workerName">O nome do worker que irá executar o processo.</param>
        /// <param name="serviceTypeName">O nome do tipo de serviço que pode ser executado.</param>
        /// <param name="serviceMethodName">O nome do método de serviço que pode ser executado.</param>
        /// <param name="createdUserId">O id do usuário criador dos processamentos que podem ser executados.</param>
        /// <param name="machineName">O nome do servidor que irá executar o processo.</param>
        /// <returns>O ticket.</returns>
        public string GetNextTicketToRun(string workerName, string serviceTypeName, string serviceMethodName, int? createdUserId, string machineName)
        {
            return Resource.QueryOne<string>(Sql.ProcessOrder.GetNextTicketToRun, new { workerName, serviceTypeName, serviceMethodName, createdUserId, machineName });
        }

        /// <summary>
        /// Obtém os processamentos que devem aparecer na área de notificação.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="lastCheck">A data da última verificação.</param>
        /// <returns>
        /// Os processamentos que devem aparecer na área de notificação.
        /// </returns>
        public IEnumerable<ProcessOrder> CheckNotifications(int userId, DateTime? lastCheck)
        {
            DateTime oneHourAgo = DateTime.Now.AddHours(-1);

            lastCheck = lastCheck ?? DateTime.Today.AddDays(-30);

            DateTime modifiedDate = oneHourAgo < lastCheck.Value ? oneHourAgo : lastCheck.Value;

            IPaginated<ProcessOrder> orders = (IPaginated<ProcessOrder>)this.Find("CreatedUserId=@userId AND ModifiedDate>=@modifiedDate", new { userId, modifiedDate }, new Paging(0, 4, "ModifiedDate DESC"));

            ////var summarized = orders.Select(order => order.Summarize()).ToList();

            return new MemoryResult<ProcessOrder>(orders, orders);
        }

        /// <summary>
        /// Obtém todos os processamentos do usuário.
        /// </summary>
        /// <param name="currentUserId">O id do usuário efetuando a consulta.</param>
        /// <param name="isAdministrator">Se o usuário efetuando a consulta é administrador.</param>
        /// <param name="createdUserId">O id do usuário.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        public IEnumerable<ProcessOrderModel> FindAllByUser(int currentUserId, bool isAdministrator, int? createdUserId, string processName, ProcessOrderState? state, Framework.Domain.Paging paging)
        {
            string stateNameCreated = Texts.ProcessOrderStateFixedValueCreated,
                stateNameQueued = Texts.ProcessOrderStateFixedValueQueued,
                stateNameError = Texts.ProcessOrderStateFixedValueError,
                stateNameIsExecuting = Texts.ProcessOrderStateFixedValueIsExecuting,
                stateNameFailed = Texts.ProcessOrderStateFixedValueFailed,
                stateNameFinished = Texts.ProcessOrderStateFixedValueFinished,
                stateNameResultsAvailable = Texts.ProcessOrderStateFixedValueResultsAvailable,
                stateNameResultsExpunged = Texts.ProcessOrderStateFixedValueResultsExpunged;

            Dictionary<int, ProcessOrderModel> result = new Dictionary<int, ProcessOrderModel>();

            var paginated = Resource.Query<ProcessOrder, ProcessOrderService, MemoryRuntimeUser, ProcessOrderArgument, ProcessOrderModel>(
                Sql.ProcessOrder.GetProcessOrderDetail,
                new 
                { 
                    currentUserId, 
                    isAdministrator, 
                    createdUserId, 
                    processName, 
                    state,
                    stateNameCreated,
                    stateNameQueued,
                    stateNameError,
                    stateNameIsExecuting,
                    stateNameFailed,
                    stateNameFinished,
                    stateNameResultsAvailable,
                    stateNameResultsExpunged,
                },
                (po, pos, user, poa) => MapProcessOrderArgument(result, po, pos, user, poa),
                "SplitOn1,SplitOn2,SplitOn3").AsPaging(paging, Sql.ProcessOrder.FindAllByUser_Paging, Sql.ProcessOrder.FindAllByUser_Count);

            paginated.Perform();

            var orders = result.Values;

            var summarized = orders; ////.Select(order => order.Summarize(true, argument => argument.IsExposed)).ToList();

            return new MemoryResult<ProcessOrderModel>(paginated, summarized);
        }

        /// <summary>
        /// Obtém todas os processamentos que podem ser removidos do sistema.
        /// </summary>
        /// <param name="cutoffDate">A data de corte.</param>
        /// <param name="createdMachineName">O nome do servidor, ou null para todos servidores.</param>
        /// <returns>
        /// Os processamentos.
        /// </returns>
        public IEnumerable<ProcessOrder> FindAllReadyToBeRemoved(DateTime cutoffDate, string createdMachineName)
        {
            return Resource.Query<ProcessOrder>(Sql.ProcessOrder.FindAllReadyToBeRemoved, new { cutoffDate, createdMachineName });
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado e preenche apenas as propriedades projetadas.
        /// </summary>
        /// <typeparam name="ProcessOrder">The type of the rocess order.</typeparam>
        /// <param name="projection">As propriedades a serem projetadas. Exemplo: Username, Email</param>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        public override IEnumerable<ProcessOrder> Find<ProcessOrder>(string projection, string filter, object filterArgs)
        {
            // Override aqui por causa do NOLOCK
            var sql = "SELECT ProcessOrderId, {0} FROM ProcessOrder WITH (NOLOCK) WHERE {1}".With(projection, filter);

            return Command.Query<ProcessOrder>(sql, filterArgs);
        }

        /// <summary>
        /// Mapeia as entidades do ProcessOrder.
        /// </summary>
        /// <param name="result">O dicionário contendo os ProcessOrderModel já mapeados.</param>
        /// <param name="po">O ProcessOrder.</param>
        /// <param name="pos">O ProcessOrderService.</param>
        /// <param name="user">O Usuário.</param>
        /// <param name="poa">O ProcessOrderArgument.</param>
        /// <returns>O ProcessOrderModel existente ajustado com estas informações, ou um ProcessOrderModel novo caso este ProcessOrder ainda não tenha sido mapeado.</returns>
        internal static ProcessOrderModel MapProcessOrderArgument(Dictionary<int, ProcessOrderModel> result, ProcessOrder po, ProcessOrderService pos, IRuntimeUser user, ProcessOrderArgument poa)
        {
            if (!result.ContainsKey(po.ProcessOrderId))
            {
                po.Service = pos;
                result[po.ProcessOrderId] = new ProcessOrderModel(po);
                result[po.ProcessOrderId].CreatedUser = user;
                result[po.ProcessOrderId].Service.ProcessOrderId = po.ProcessOrderId;
            }

            var processOrder = result[po.ProcessOrderId];

            if (!poa.IsNew && !string.IsNullOrWhiteSpace(poa.Name))
            {
                processOrder.Arguments.Add(poa);
                poa.ProcessOrderId = processOrder.ProcessOrderId;
            }

            return result[po.ProcessOrderId];
        }

        #endregion
    }
}
