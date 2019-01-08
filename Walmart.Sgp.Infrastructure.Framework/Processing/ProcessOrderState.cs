using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Estado do processamento.
    /// </summary>
    public enum ProcessOrderState
    {
        /// <summary>
        /// Recém criado.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Ocorreu um erro durante a validação ou enfileiramento da ordem de execução.
        /// </summary>
        Error,

        /// <summary>
        /// Registrado para execução futura.
        /// </summary>
        Queued,

        /// <summary>
        /// Em execução no momento.
        /// </summary>
        IsExecuting,

        /// <summary>
        /// Execução falhou (o método de serviço retornou um erro).
        /// </summary>
        Failed,

        /// <summary>
        /// Execução terminou com sucesso.
        /// </summary>
        Finished,

        /// <summary>
        /// O resultado foi disponibilizado para consulta.
        /// </summary>
        ResultsAvailable,

        /// <summary>
        /// O resultado foi expurgado. (ocorre depois de um período pré determinado)
        /// </summary>
        ResultsExpunged
    }
}
