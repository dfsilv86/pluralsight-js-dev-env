using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Processo executado na infraestrutura de processamento.
    /// </summary>
    [DebuggerDisplay("ProcessOrder {Ticket} ({State}) (created by userid {CreatedUserId}), currently is {CurrentProgress} of {TotalProgress} ({Message})")]
    [Serializable]
    public class ProcessOrder : EntityBase, IStampContainer
    {
        #region Fields

        /// <summary>
        /// O nome de worker usado quando o processo é executado imediatamente.
        /// </summary>
        public static readonly string ImmediateWorkerName = "Immediate";

        /// <summary>
        /// O nome de worker usado quando o processo vai ser removido e não havia nenhum workername definido (caso de erro logo após a criação).
        /// </summary>
        public static readonly string ExpungeWorkerName = "Expunge";

        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrder"/>.
        /// </summary>
        public ProcessOrder()
        {
            this.Service = new ProcessOrderService();
            this.Arguments = new List<ProcessOrderArgument>();
            this.CreatedDate = DateTime.Now;
            this.CreatedUserId = RuntimeContext.Current.User.Id;
            this.Ticket = Guid.NewGuid().ToString();
            this.State = ProcessOrderState.Created;
            this.CurrentProgress = 0;
            this.TotalProgress = 0;
        }
        #endregion

        #region properties

        /// <summary>
        /// Obtém ou define o id da ordem de execução.
        /// </summary>
        public int ProcessOrderId { get; set; }

        /// <summary>
        /// Obtém ou define o id da ordem de execução.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.ProcessOrderId;
            }
            
            set
            {
                this.ProcessOrderId = value;
            }
        }

        /// <summary>
        /// Obtém ou define o ticket do processamento.
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// Obtém ou define o nome pelo qual o client reconhece este processamento.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Obtém ou define o estado atual do processamento.
        /// </summary>
        public ProcessOrderState State { get; set; }

        /// <summary>
        /// Obtém ou define o progresso atual do processamento.
        /// </summary>
        public int CurrentProgress { get; set; }

        /// <summary>
        /// Obtém ou define o progresso total do processamento.
        /// </summary>
        public int TotalProgress { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de status atual do processamento.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Obtém ou define lista de argumentos (parâmetros) do processo.
        /// </summary>
        public IList<ProcessOrderArgument> Arguments { get; protected internal set; }

        /// <summary>
        /// Obtém ou define a data e hora de criação.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Obtém ou define a data e hora da última modificação.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Obtém ou define o usuário criador.
        /// </summary>
        public int CreatedUserId { get; set; }

        /// <summary>
        /// Obtém ou define o usuário que realizou a última modificação.
        /// </summary>
        public int? ModifiedUserId { get; set; }

        /// <summary>
        /// Obtém ou define para qual data o processamento foi agendado para iniciar.
        /// </summary>
        /// <remarks>Em caso de null, o processo pode ser iniciado assim que houver um worker disponível.</remarks>
        public DateTime? ExecuteAfter { get; set; }

        /// <summary>
        /// Obtém ou define a data em que o processamento iniciou.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Obtém ou define a data em que o processamento terminou.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /////// <summary>
        /////// Obtém ou define a quantidade de processos deste tipo que estavam em execução no momento em que este processamento iniciou.
        /////// </summary>
        ////public int? StartConcurrency { get; set; }

        /////// <summary>
        /////// Obtém ou define a quantidade de processos deste tipo que estavam em execução no momento em que este processamento terminou.
        /////// </summary>
        ////public int? EndConcurrency { get; set; }

        /// <summary>
        /// Obtém ou define o worker que está executando o processo.
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// Obtém o quanto o processo avançou em %.
        /// </summary>
        public decimal CurrentProgressPercentage
        {
            get
            {
                return this.TotalProgress < 1 ? 0 : ((decimal)this.CurrentProgress / (decimal)this.TotalProgress) * 100m;
            }
        }

        #region IStampContainer implementation

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        DateTime IStampContainer.DhCriacao
        {
            get
            {
                return this.CreatedDate;
            }

            set
            {
                this.CreatedDate = value;
            }
        }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        DateTime? IStampContainer.DhAtualizacao
        {
            get
            {
                return this.ModifiedDate;
            }

            set
            {
                this.ModifiedDate = value;
            }
        }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        /// <exception cref="NotSupportedException">Foi informado null para o usuário de criação.</exception>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        int? IStampContainer.CdUsuarioCriacao
        {
            get
            {
                return this.CreatedUserId;
            }

            set
            {
                if (!value.HasValue)
                {
                    throw new NotSupportedException(Texts.CannotModifyCreatedUserId);
                }

                this.CreatedUserId = value.Value;
            }
        }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        int? IStampContainer.CdUsuarioAtualizacao
        {
            get
            {
                return this.ModifiedUserId;
            }

            set
            {
                this.ModifiedUserId = value;
            }
        }

        #endregion

        /// <summary>
        /// Obtém ou define as informações sobre o serviço.
        /// </summary>
        public ProcessOrderService Service { get; set; }

        /// <summary>
        /// Obtém ou define o nome do servidor onde a ordem de execução foi criada.
        /// </summary>
        public string CreatedMachineName { get; set; }

        #endregion

        /// <summary>
        /// Sumariza os dados.
        /// </summary>
        /// <returns>Informações básicas sobre um processamento.</returns>
        public ProcessOrderSummary Summarize(bool includeArguments = false, Func<ProcessOrderArgument, bool> argumentPredicate = null)
        {
            if (includeArguments && null == argumentPredicate)
            {
                argumentPredicate = (ProcessOrderArgument arg) => false;
            }

            return new ProcessOrderSummary
            {
                Arguments = includeArguments ? (this.Arguments ?? new ProcessOrderArgument[0]).Where(argumentPredicate).ToList() : null,
                CreatedDate = this.CreatedDate,
                CreatedUserId = this.CreatedUserId,
                CurrentProgress = this.CurrentProgress,
                EndDate = this.EndDate,
                ExecuteAfter = this.ExecuteAfter,
                Message = this.Message,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                ProcessName = this.ProcessName,
                ProcessOrderId = this.ProcessOrderId,
                StartDate = this.StartDate,
                State = this.State,
                Ticket = this.Ticket,
                TotalProgress = this.TotalProgress
            };
        }
    }
}
