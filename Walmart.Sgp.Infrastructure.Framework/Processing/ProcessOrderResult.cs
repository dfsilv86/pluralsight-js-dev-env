using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Representa o resultado de um processo registrado na infraestrutura de processamento.
    /// </summary>
    [Serializable]
    public class ProcessOrderResult
    {
        private object m_result;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessOrderResult"/>.
        /// </summary>
        /// <param name="result">O resultado do processamento, se já executado.</param>
        /// <param name="process">O processo.</param>
        public ProcessOrderResult(object result, ProcessOrder process)
        {
            this.ProcessOrder = process;
            this.m_result = result;
        }

        /// <summary>
        /// Obtém o processo.
        /// </summary>
        public ProcessOrder ProcessOrder { get; private set; }

        /// <summary>
        /// Obtém o resultado.
        /// </summary>
        /// <returns>O resultado.</returns>
        public object UnwrapResult()
        {
            return this.m_result;
        }
    }
}
