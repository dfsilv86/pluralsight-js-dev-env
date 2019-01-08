using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Walmart.Sgp.Infrastructure.Framework.Logging;

namespace Walmart.Sgp.Infrastructure.IO.Logging
{
    /// <summary>
    /// Estratégia de log para o log4net.
    /// </summary>
    public class Log4netLogStrategy : ILogStrategy
    {
        #region Fields
        private readonly ILog m_logger;
        #endregion

        #region Constructors        
        /// <summary>
        /// Inicia os membros estáticos da classe <see cref="Log4netLogStrategy"/>.
        /// </summary>
        static Log4netLogStrategy()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Log4netLogStrategy"/>.
        /// </summary>
        /// <param name="loggerName">O nome do logger.</param>
        public Log4netLogStrategy(string loggerName)
        {
            m_logger = LogManager.GetLogger(loggerName);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loga a mensagem de debug, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Debug(string message, params object[] values)
        {
            m_logger.DebugFormat(CultureInfo.InvariantCulture, message, values);
        }

        /// <summary>
        /// Loga a mensagem de informação, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Info(string message, params object[] values)
        {
            m_logger.InfoFormat(CultureInfo.InvariantCulture, message, values);
        }

        /// <summary>
        /// Loga a mensagem de alerta, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Warning(string message, params object[] values)
        {
            m_logger.WarnFormat(CultureInfo.InvariantCulture, message, values);
        }

        /// <summary>
        /// Loga a mensagem de erro, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Error(string message, params object[] values)
        {
            m_logger.ErrorFormat(CultureInfo.InvariantCulture, message, values);
        }
        #endregion
    }
}
