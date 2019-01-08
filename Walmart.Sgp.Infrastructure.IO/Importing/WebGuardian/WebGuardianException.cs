using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Representa erros que ocorrem em operações do WebGuardian.
    /// </summary>
    [Serializable]
    public class WebGuardianException : SsoException
    {
        private const string ErrorTranslationKey = "WebGuardianErrorCode";
                
        private readonly int m_errorCode;
        private readonly string m_originalMessage;

        private readonly IDictionary m_data;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianException"/>.
        /// </summary>
        public WebGuardianException()
            : base(Texts.WebGuardianError)
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        public WebGuardianException(string message)
            : base(message)
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianException"/>.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="innerException">A exceção interna.</param>
        public WebGuardianException(string message, Exception innerException)
            : base(message, innerException)
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianException"/>.
        /// </summary>
        /// <param name="status">O status de retorno do WebGuardian.</param>        
        public WebGuardianException(Status status)
            : base(GlobalizationHelper.GetText(ErrorTranslationKey + status.CodigoRetorno, false) ?? status.Retorno)
        {
            m_errorCode = status.CodigoRetorno;
            m_originalMessage = status.Retorno;                         
            m_data = new Dictionary<string, object>()
            {
                { "OriginalMessage", m_originalMessage },
                { "ErrorCode", m_errorCode }
            };
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianException"/>.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected WebGuardianException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
            m_originalMessage = info.GetString("OriginalMessage");
            m_errorCode = info.GetInt32("ErrorCode");
        }

        /// <summary>
        /// Obtém o código de erro.
        /// </summary>
        public int ErrorCode
        {
            get { return m_errorCode; }
        }

        /// <summary>
        /// Obtém a mensagem de erro original do WebGuardian.
        /// </summary>
        public string OriginalMessage
        {
            get { return m_originalMessage; }
        }

        /// <summary>
        /// Obtém informações adicionais sobre o erro.
        /// </summary>
        public override IDictionary Data
        {
            get
            {
                return m_data ?? base.Data;
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ErrorCode", m_errorCode);
            info.AddValue("OriginalMessage", m_originalMessage);            
            base.GetObjectData(info, context);
        }
    }
}
