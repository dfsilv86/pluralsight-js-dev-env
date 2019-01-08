using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Informações complementares de ProcessOrder.
    /// </summary>
    public class ProcessOrderSummaryModel : ProcessOrderSummary
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrderSummaryModel"/>.
        /// </summary>
        public ProcessOrderSummaryModel()
        {
        }

        /// <summary>
        /// Obtém ou define o usuário criador.
        /// </summary>
        public IRuntimeUser CreatedUser { get; set; }
    }
}
