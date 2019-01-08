using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    using UpdateDelegate = System.Action<int?, int?, string>;
    
    /// <summary>
    /// O contexto de execução de um processo.
    /// </summary>
    public class MemoryProcessingContext : IProcessingContext
    {
        private UpdateDelegate m_update;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryProcessingContext"/>.
        /// </summary>
        public MemoryProcessingContext()
            : this(null)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryProcessingContext"/>.
        /// </summary>
        /// <param name="update">O delegate para realizar a alteração das informações.</param>
        public MemoryProcessingContext(UpdateDelegate update)
        {
            m_update = update;
        }

        /// <summary>
        /// Define o progresso e a mensagem de status da ordem de execução vigente, se houver alguma.
        /// </summary>
        /// <param name="currentProgress">O progresso atual, ou null para não modificar a informação atual na tabela.</param>
        /// <param name="totalProgress">O progresso total, ou null para não modificar a informação atual na tabela.</param>
        /// <param name="message">A mensagem, ou null para não modificar a informação atual na tabela.</param>
        /// <remarks>
        /// Cada chamada a SetProgress implica em um UPDATE na tabela ProcessOrder e a inserção de um registro de log na tabela ProcessOrderLog.
        /// </remarks>
        public void SetProgress(int? currentProgress, int? totalProgress, string message)
        {
            if (null != m_update)
            {
                m_update(currentProgress, totalProgress, message);
            }
        }
    }
}
