using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Representa o resultado (de um tipo conhecido) de um processo registrado na infraestrutura de processamento.
    /// </summary>
    /// <typeparam name="TResult">O tipo do resultado.</typeparam>
    public class ProcessOrderResult<TResult> : ProcessOrderResult
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrderResult{TResult}"/>.
        /// </summary>
        /// <param name="result">O resultado do processamento, se já executado.</param>
        /// <param name="process">O processo.</param>
        public ProcessOrderResult(TResult result, ProcessOrder process)
            : base(result, process)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrderResult{TResult}"/>.
        /// </summary>
        /// <param name="processResult">O resultado a ser copiado.</param>
        public ProcessOrderResult(ProcessOrderResult processResult)
            : base((TResult)processResult.UnwrapResult(), processResult.ProcessOrder)
        {
        }

        /// <summary>
        /// Obtém o resultado (de um tipo específico).
        /// </summary>
        /// <returns>O resultado.</returns>
        public new TResult UnwrapResult()
        {
            // Quando sabemos que ocorreu um erro.
            if (this.ProcessOrder.State == ProcessOrderState.Error || this.ProcessOrder.State == ProcessOrderState.Failed)
            {
                throw new ProcessException(this);
            }

            // Quando ainda está na fila ou em execução (na API pode retornar como um alerta)
            if (this.ProcessOrder.State != ProcessOrderState.Finished && this.ProcessOrder.State != ProcessOrderState.ResultsAvailable)
            {
                throw new ProcessNotFinishedException(this);
            }

            return (TResult)base.UnwrapResult();
        }
    }
}
