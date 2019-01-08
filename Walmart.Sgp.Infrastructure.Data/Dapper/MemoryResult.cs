using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Resultado de consulta que utiliza a informação de paginação da consulta original e os dados de resultado da memória.
    /// </summary>
    /// <remarks>
    /// Deve ser utilizado em situações onde temos N linhas de resutlado do SQL, mas no mapeamento será registrado uma única linha agrupadora.
    /// Ver exemplo em LojaProcessosCarga que possui uma lista de ProcessoCarga na propriedade Cargas.
    /// </remarks>
    /// <typeparam name="TReturn">O tipo de resultado.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class MemoryResult<TReturn> : IPaginated<TReturn>
    {
        #region Fields
        private readonly IEnumerable<TReturn> m_result;
        #endregion

        #region Construtors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryResult{TReturn}"/>.
        /// </summary>
        /// <param name="queryResult">A consulta origional.</param>
        /// <param name="memoryResult">O resultado em memória.</param>
        public MemoryResult(IPaginated<TReturn> queryResult, IEnumerable<TReturn> memoryResult)
        {
            Paging = queryResult.Paging;
            TotalCount = queryResult.TotalCount;
            m_result = memoryResult;            
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a paginação utilizada.
        /// </summary>
        public IPaging Paging { get; private set; }

        /// <summary>
        /// Obtém o total de linhas existentes.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return m_result.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<TReturn> IEnumerable<TReturn>.GetEnumerator()
        {
            return m_result.GetEnumerator();
        }
        #endregion
    }
}