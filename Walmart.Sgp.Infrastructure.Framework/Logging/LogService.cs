using System;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.Logging
{  
    /// <summary>
    /// Serviço que centraliza funcionalidades relacionadas a log.
    /// </summary>
    public static class LogService
    {
        #region Fields
        private static ILogStrategy s_strategy = new ConsoleLogStrategy();
        #endregion

        #region Methods
        /// <summary>
        /// Inicializa o serviço.
        /// </summary>
        /// <param name="strategy">A estrategía de log a ser utilizada.</param>
        public static void Initialize(ILogStrategy strategy)
        {
            ExceptionHelper.ThrowIfNull("strategy", strategy);
            s_strategy = strategy;
        }

        /// <summary>
        /// Loga a mensagem de debug, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public static void Debug(string message, params object[] values)
        {
            s_strategy.Debug(message, values);
        }

        /// <summary>
        /// Loga a mensagem de informação, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public static void Info(string message, params object[] values)
        {
            s_strategy.Info(message, values);
        }

        /// <summary>
        /// Loga a mensagem de alerta, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public static void Warning(string message, params object[] values)
        {
            s_strategy.Warning(message, values);
        }

        /// <summary>
        /// Loga a mensagem de erro, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public static void Error(string message, params object[] values)
        {
            s_strategy.Error(message, values);
        }        
        #endregion
    }
}
