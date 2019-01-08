using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Logging
{
    /// <summary>
    /// Uma estratégia de log para o Console.
    /// </summary>
    public class ConsoleLogStrategy : ILogStrategy
    {
        /// <summary>
        /// Loga a mensagem de debug, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Debug(string message, params object[] values)
        {
            Console.WriteLine(message, values);
        }

        /// <summary>
        /// Loga a mensagem de info, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Info(string message, params object[] values)
        {
            Console.WriteLine(message, values);
        }

        /// <summary>
        /// Loga a mensagem de alerta, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Warning(string message, params object[] values)
        {
            Console.WriteLine(message, values);
        }

        /// <summary>
        /// Loga a mensagem de erro, formatando a mensagem.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores.</param>
        public void Error(string message, params object[] values)
        {
            Console.WriteLine(message, values);
        }
    }
}
