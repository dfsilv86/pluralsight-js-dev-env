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
    /// Exception utilizada quando é extraído o resultado do processamento quando ocorreu um erro durante a validação ou execução do processo.
    /// </summary>
    [Serializable]
    public class ProcessException : Exception
    {
        #region Fields
        private readonly ProcessOrderResult m_processResult;
        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessException"/>.
        /// </summary>
        public ProcessException()
            : base(Texts.ProcessOrderStateErrorMessage)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        public ProcessException(string message) 
            : base(message) 
        { 
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="inner">A exceção interna.</param>
        public ProcessException(string message, Exception inner)
            : base(message, inner) 
        { 
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="processResult">As informações sobre o resultado do processamento.</param>
        public ProcessException(string message, ProcessOrderResult processResult)
            : this(message)
        {
            m_processResult = processResult;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessException"/>.
        /// </summary>
        /// <param name="processResult">As informações sobre o resultado do processamento.</param>
        public ProcessException(ProcessOrderResult processResult)
            : this()
        {
            m_processResult = processResult;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessException"/>.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ProcessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            m_processResult = (ProcessOrderResult)info.GetValue("ProcessResult", typeof(ProcessOrderResult));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o resultado do processo.
        /// </summary>
        public ProcessOrderResult ProcessResult
        {
            get
            {
                return m_processResult;
            }
        }
        #endregion

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ProcessResult", this.ProcessResult);

            base.GetObjectData(info, context);
        }
    }
}
