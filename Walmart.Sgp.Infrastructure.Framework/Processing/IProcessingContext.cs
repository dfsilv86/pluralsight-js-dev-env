using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Define a interface de um contexto de execução de um processo.
    /// </summary>
    public interface IProcessingContext
    {
        /// <summary>
        /// Define o progresso e a mensagem de status da ordem de execução vigente, se houver alguma.
        /// </summary>
        /// <param name="currentProgress">O progresso atual, ou null para não modificar a informação atual na tabela.</param>
        /// <param name="totalProgress">O progresso total, ou null para não modificar a informação atual na tabela.</param>
        /// <param name="message">A mensagem, ou null para não modificar a informação atual na tabela.</param>
        /// <remarks>
        /// Cada chamada a SetProgress implica em um UPDATE na tabela ProcessOrder e a inserção de um registro de log na tabela ProcessOrderLog.
        /// </remarks>
        void SetProgress(int? currentProgress, int? totalProgress, string message);
    }
}
