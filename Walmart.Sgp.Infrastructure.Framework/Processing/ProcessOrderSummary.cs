using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Processo executado na infraestrutura de processamento.
    /// </summary>
    public class ProcessOrderSummary : EntityBase
    {
        /// <summary>
        /// Obtém ou define a lista de argumentos (parâmetros) do processo.
        /// </summary>
        public IList<ProcessOrderArgument> Arguments { get; protected internal set; }

        /// <summary>
        /// Obtém ou define a data e hora de criação.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Obtém ou define o usuário criador.
        /// </summary>
        public int CreatedUserId { get; set; }
        
        /// <summary>
        /// Obtém ou define CreatedUserFullname.
        /// </summary>
        public string CreatedUserFullname { get; set; }

        /// <summary>
        /// Obtém ou define CreatedUsername.
        /// </summary>
        public string CreatedUsername { get; set; }

        /// <summary>
        /// Obtém ou define o progresso atual do processamento.
        /// </summary>
        public int CurrentProgress { get; set; }

        /// <summary>
        /// Obtém ou define a data em que o processamento terminou.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Obtém ou define para qual data o processamento foi agendado para iniciar.
        /// </summary>
        /// <remarks>Em caso de null, o processo pode ser iniciado assim que houver um worker disponível.</remarks>
        public DateTime? ExecuteAfter { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de status atual do processamento.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Obtém ou define a data e hora da última modificação.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Obtém ou define o usuário que realizou a última modificação.
        /// </summary>
        public int? ModifiedUserId { get; set; }

        /// <summary>
        /// Obtém ou define o nome pelo qual o client reconhece este processamento.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Obtém ou define a data em que o processamento iniciou.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Obtém ou define o estado atual do processamento.
        /// </summary>
        public ProcessOrderState State { get; set; }

        /// <summary>
        /// Obtém ou define StateName.
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// Obtém ou define o ticket do processamento.
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// Obtém ou define o progresso total do processamento.
        /// </summary>
        public int TotalProgress { get; set; }

        /// <summary>
        /// Obtém ou define o id da ordem de execução.
        /// </summary>
        public int ProcessOrderId { get; set; }

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
    }
}
