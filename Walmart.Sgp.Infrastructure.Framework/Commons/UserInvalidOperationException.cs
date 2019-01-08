using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.Commons
{
    /// <summary>
    /// Exceção que tem origem em alguma operação inválida realizada pelo usuário.
    /// </summary>
    [Serializable]
    public class UserInvalidOperationException : Exception
    {        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserInvalidOperationException"/>.
        /// </summary>
        public UserInvalidOperationException()            
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserInvalidOperationException"/>.
        /// </summary>
        /// <param name="message">A mensagem de erro.</param>
        public UserInvalidOperationException(string message)
            : base(message)
        {            
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserInvalidOperationException"/>.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="innerException">A exceção interna.</param>
        public UserInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {            
        }     

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserInvalidOperationException"/>.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected UserInvalidOperationException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {       
        }

        /// <summary>
        /// Verifica se a exceção informada é um tipo de exceção que tem origem em alguma operação inválida realizada pelo usuário.
        /// </summary>
        /// <param name="exception">A exceção.</param>
        /// <returns>True se é uma exceção de usua´rio, false no contrário.</returns>
        public static bool Is(Exception exception)
        {
            // InvalidCastException é retornado pelos operadores de conversão dos fixedvalues, por exemplo.
            return exception is UserInvalidOperationException || exception is InvalidCastException;
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