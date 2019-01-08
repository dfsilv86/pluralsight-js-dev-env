using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Representa o retorno mapeado de uma query Dapper.
    /// </summary>
    /// <typeparam name="TInput">O tipo de objeto que será mapeado.</typeparam>
    /// <typeparam name="TOutput">O tipo de objeto que será retornado após o mapeamento.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class DapperResult<TInput, TOutput> : IPaginated<TOutput>
    {
        private IPaginated<TInput> m_paginated;
        private Func<TInput, TOutput> m_mapper;
        private IEnumerable<TOutput> m_mapped;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperResult{TInput,TOutput}"/>.
        /// </summary>
        /// <param name="paginated">A lista contendo os objetos que serão mapeados.</param>
        /// <param name="mapper">O delegate que realiza o mapeamento.</param>
        public DapperResult(IPaginated<TInput> paginated, Func<TInput, TOutput> mapper)
        {
            m_paginated = paginated;
            m_mapper = mapper;
        }

        /// <summary>
        /// Obtém Paging.
        /// </summary>
        public IPaging Paging
        {
            get
            {
                return m_paginated.Paging;
            }
        }

        /// <summary>
        /// Obtém TotalCount.
        /// </summary>
        public int TotalCount
        {
            get
            {
                return m_paginated.TotalCount;
            }
        }

        /// <summary>
        /// Retorna o enumerador que itera através da coleção.
        /// </summary>
        /// <returns>O enumerador que pode ser utilizado para iterar pela coleção.</returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return Map().GetEnumerator();
        }

        /// <summary>
        /// Retorna o enumerador que itera através da coleção.
        /// </summary>
        /// <returns>O enumerador que pode ser utilizado para iterar pela coleção.</returns>
        IEnumerator<TOutput> IEnumerable<TOutput>.GetEnumerator()
        {
            return Map().GetEnumerator();
        }

        private IEnumerable<TOutput> Map()
        {
            if (m_mapped == null)
            {
                var mapped = new List<TOutput>();

                foreach (var item in m_paginated)
                {
                    mapped.Add(m_mapper(item));
                }

                m_mapped = mapped;
            }

            return m_mapped;
        }
    }
}
