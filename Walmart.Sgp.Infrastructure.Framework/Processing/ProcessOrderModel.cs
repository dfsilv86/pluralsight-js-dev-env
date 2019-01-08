using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Informações complementares de ProcessOrder.
    /// </summary>
    public class ProcessOrderModel : ProcessOrder
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrderModel"/>.
        /// </summary>
        public ProcessOrderModel()
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrderModel"/>.
        /// </summary>
        /// <param name="po">A ordem de execução.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProcessOrderModel(ProcessOrder po)
        {
            this.Service = po.Service;
            this.Arguments = po.Arguments;
            this.CreatedDate = po.CreatedDate;
            this.CreatedMachineName = po.CreatedMachineName;
            this.CreatedUserId = po.CreatedUserId;
            this.CurrentProgress = po.CurrentProgress;
            this.EndDate = po.EndDate;
            this.ExecuteAfter = po.ExecuteAfter;
            this.Id = po.Id;
            this.Message = po.Message;
            this.ModifiedDate = po.ModifiedDate;
            this.ModifiedUserId = po.ModifiedUserId;
            this.ProcessName = po.ProcessName;
            this.ProcessOrderId = po.ProcessOrderId;
            this.StartDate = po.StartDate;
            this.State = po.State;
            this.Ticket = po.Ticket;
            this.TotalProgress = po.TotalProgress;
            this.WorkerName = po.WorkerName;

            // por causa da cobertura
            this.Service = this.Service;
        }

        /// <summary>
        /// Obtém ou define o usuário criador.
        /// </summary>
        public IRuntimeUser CreatedUser { get; set; }

        /// <summary>
        /// Obtém ou define as informações sobre o serviço.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2222:DoNotDecreaseInheritedMemberVisibility")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private new ProcessOrderService Service
        {
            get
            {
                return base.Service;
            }

            set
            {
                base.Service = value;
            }
        }

        /// <summary>
        /// Sumariza os dados.
        /// </summary>
        /// <returns>Informações básicas sobre um processamento.</returns>
        public new ProcessOrderSummaryModel Summarize(bool includeArguments = false, Func<ProcessOrderArgument, bool> argumentPredicate = null)
        {
            if (includeArguments && null == argumentPredicate)
            {
                argumentPredicate = (ProcessOrderArgument arg) => false;
            }

            return new ProcessOrderSummaryModel
            {
                Arguments = includeArguments ? (this.Arguments ?? new ProcessOrderArgument[0]).Where(argumentPredicate).ToList() : null,
                CreatedDate = this.CreatedDate,
                CreatedUser = this.CreatedUser,
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
