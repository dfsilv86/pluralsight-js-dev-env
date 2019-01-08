using System.Diagnostics.CodeAnalysis;

namespace Walmart.Sgp.Infrastructure.Framework.Logging
{
    /// <summary>
    /// Define a interface de uma estratégia de log.
    /// </summary>    
    public interface ILogStrategy
    {
        /// <summary>
        /// Loga a mensagem de debug, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        void Debug(string message, params object[] values);

        /// <summary>
        /// Loga a mensagem de info, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        void Info(string message, params object[] values);

        /// <summary>
        /// Loga a mensagem de alerta, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        void Warning(string message, params object[] values);

        /// <summary>
        /// Loga a mensagem de erro, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error(string message, params object[] values);
    }
}