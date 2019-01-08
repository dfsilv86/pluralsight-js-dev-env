using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Representa um erro que ocorre durante a leitura de um arquivo de inventário.
    /// </summary>
    [Serializable]
    public class ImportacaoArquivoInventarioException : Exception
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        public ImportacaoArquivoInventarioException()
            : this(Texts.ImportLogArquivoInvalido, Texts.InvalidFile, (string)null)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        public ImportacaoArquivoInventarioException(string message)
            : this(Texts.ImportLogArquivoInvalido, message, (string)null)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="inner">A exceção interna.</param>
        public ImportacaoArquivoInventarioException(string message, Exception inner)
            : this(Texts.ImportLogArquivoInvalido, message, inner)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        /// <param name="action">A ação que ocasionou o erro.</param>
        /// <param name="message">A mensagem de erro.</param>
        public ImportacaoArquivoInventarioException(string action, string message)
            : base(message)
        {
            this.Action = action;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        /// <param name="action">A ação que ocasionou o erro.</param>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="inner">A exceção interna.</param>
        public ImportacaoArquivoInventarioException(string action, string message, Exception inner)
            : base(message, inner)
        {
            this.Action = action;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        /// <param name="action">A ação que ocasionou o erro.</param>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="message2">A mensagem de crítica.</param>
        public ImportacaoArquivoInventarioException(string action, string message, string message2)
            : base(message)
        {
            this.Action = action;
            this.Message2 = message2;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImportacaoArquivoInventarioException"/>.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ImportacaoArquivoInventarioException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.Action = info.GetString("Action");
            this.Message2 = info.GetString("Message2");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém a ação que ocasionou o erro.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Obtém a mensagem de crítica.
        /// </summary>
        public string Message2 { get; private set; }

        #endregion

        #region Methods

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

            info.AddValue("Action", this.Action);
            info.AddValue("Message2", this.Message2);

            base.GetObjectData(info, context);
        }

        #endregion
    }
}
