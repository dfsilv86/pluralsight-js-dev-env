using System;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Representa uma estratégia de paginação.
    /// </summary>
    public interface IDapperPagingStrategy
    {
        /// <summary>
        /// Obtém a paginação.
        /// </summary>
        IPaging Paging { get; }

        /// <summary>
        /// Obtém o total de registros contados pelo método CountAll()
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Conta o total de registros.
        /// </summary>
        /// <returns>A número total de registros.</returns>
        int CountAll();
    }
}
