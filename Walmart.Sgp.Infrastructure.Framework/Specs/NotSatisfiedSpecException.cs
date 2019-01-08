using System;
using System.Runtime.Serialization;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Exceção lançada quando uma especificação não é satisfeita.
    /// </summary>
    [Serializable]
    public class NotSatisfiedSpecException : UserInvalidOperationException
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotSatisfiedSpecException"/>
        /// </summary>
        public NotSatisfiedSpecException()
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotSatisfiedSpecException"/>.
        /// </summary>
        /// <param name="reason">A razão da especificação não ser satisfeita.</param>
        public NotSatisfiedSpecException(string reason)
            : base(reason)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotSatisfiedSpecException"/>.
        /// </summary>
        /// <param name="reason">A razão da especificação não ser satisfeita.</param>
        /// <param name="innerException">A exceção que deu origem a essa exceção.</param>
        public NotSatisfiedSpecException(string reason, Exception innerException)
            : base(reason, innerException)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotSatisfiedSpecException"/>.
        /// </summary>
        /// <param name="serializationInfo">A informação de serialização.</param>
        /// <param name="streamingContext">O contexto de streaming.</param>
        protected NotSatisfiedSpecException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
        #endregion
    }
}
