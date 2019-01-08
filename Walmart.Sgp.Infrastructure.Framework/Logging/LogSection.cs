using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Logging
{
    /// <summary>
    /// Permite a criação de uma seção de log que é registrada o seu início no momento da criação da instância e é registrado o seu fim no momento do dispose.
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    /// using(var section1 = new LogSection("section 1"))
    /// {
    ///     section1.Log("Teste 1");
    ///     using(var section2 = new LogSection("section 2", section1))
    ///     {
    ///         section2.Log("Teste 2.1");
    ///         section2.Log("Teste 2.2");
    ///     }
    /// }
    /// </code>
    /// O resultado no arquivo de log será:
    /// &lt;SECTION 1&gt;
    ///     Teste 1.
    ///     &lt;SECTION 2&gt;
    ///         Teste 2.1.
    ///         Teste 2.2.
    ///     &lt;/SECTION 2&gt;    
    /// &lt;/SECTION 2&gt;    
    /// </example>
    /// </remarks>
    public sealed class LogSection : IDisposable
    {
        #region Fields
        private string m_name;
        private int m_deep;
        private int m_childrenDeep;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LogSection"/>.
        /// </summary>
        /// <param name="name">O nome da seção.</param>
        public LogSection(string name) : this(name, null)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LogSection"/>.
        /// </summary>
        /// <param name="name">O nome da seção.</param>
        /// <param name="parentSection">A seção pai.</param>
        public LogSection(string name, LogSection parentSection)
        {
            m_name = name.ToUpperInvariant();
            m_deep = parentSection == null ? 0 : parentSection.m_deep + 1;
            m_childrenDeep = m_deep + 1;

            LogDeep(m_deep, "<{0}>", m_name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Registra a mensagem de log na seção.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="values">Os valores da mensagem.</param>
        public void Log(string message, params object[] values)
        {
            LogDeep(m_childrenDeep, "{0}".With(message), values);
        }

        /// <summary>
        /// Realiza o dispose.
        /// </summary>
        public void Dispose()
        {
            LogDeep(m_deep, "</{0}>", m_name);
        } 

        private static void LogDeep(int deep, string message, params object[] values)
        {
            var fullMessage = message.With(values);

            foreach (var line in fullMessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Distinct())
            {
                LogService.Info("{0}{1}".With(string.Empty.PadRight(deep, '\t'), line));
            }
        }
        #endregion
    }
}
