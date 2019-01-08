using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Especificação referente a se o processo pode ser executado.
    /// </summary>
    public class ProcessOrderCanBeExecutedSpec : SpecBase<ProcessOrder>
    {
        /// <summary>
        /// Verifica se o process order informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O process order.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo process order.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ProcessOrder target)
        {
            if ((target.State == ProcessOrderState.Created || target.State == ProcessOrderState.Queued) && !string.IsNullOrWhiteSpace(target.WorkerName))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.ProcessOrderIsNotInAnExecutableState);
        }
    }
}
