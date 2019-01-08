using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Exception utilizada quando é extraído o resultado do processamento em background quando a execução do processo ainda não foi finalizado.
    /// </summary>
    [Serializable]
    public class ProcessNotFinishedException : ProcessException
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessNotFinishedException"/>.
        /// </summary>
        public ProcessNotFinishedException()
            : base(Texts.ProcessingNotFinished)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessNotFinishedException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        public ProcessNotFinishedException(string message) 
            : base(message) 
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessNotFinishedException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="inner">A exceção interna.</param>
        public ProcessNotFinishedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessNotFinishedException"/>.
        /// </summary>
        /// <param name="processResult">As informações sobre o resultado do processamento.</param>
        public ProcessNotFinishedException(ProcessOrderResult processResult)
            : base(Texts.ProcessingNotFinished, processResult)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessNotFinishedException"/>.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ProcessNotFinishedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        {
        }
        #endregion
    }
}
