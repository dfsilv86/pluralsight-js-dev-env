using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de uma implementação de IEnumerable que já foi paginada.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IPaginated : IEnumerable
    {
        /// <summary>
        /// Obtém a paginação utilizada.
        /// </summary>
        IPaging Paging { get; }

        /// <summary>
        /// Obtém o total de linhas existentes.
        /// </summary>
        int TotalCount { get; }
    }

    /// <summary>
    /// Define a interface de uma implementação de IEnumerable{T} que já foi paginada.
    /// </summary>
    /// <typeparam name="T">O tipo de dados do IEnumerable.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IPaginated<T> : IPaginated, IEnumerable<T>
    {
    }
}
