using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Representa erros que ocorrem em operações do WebGuardian.
    /// </summary>
    [Serializable]
    public class SsoException : UserInvalidOperationException
    {        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SsoException"/>.
        /// </summary>
        public SsoException()
            : base(Texts.WebGuardianError)
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SsoException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        public SsoException(string message)
            : base(message)
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SsoException"/>.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="innerException">A exceção interna.</param>
        public SsoException(string message, Exception innerException)
            : base(message, innerException)
        {            
        }     

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SsoException"/>.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected SsoException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {       
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
            ExceptionHelper.ThrowIfNull("info", info);

            base.GetObjectData(info, context);
        }
    }
}
