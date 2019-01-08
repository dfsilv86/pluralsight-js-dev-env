using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Representa a configuração de uma paginação.
    /// </summary>    
    [DebuggerDisplay("Paging: {Offset}, {Limit}, {OrderBy}")]
    public class Paging : IPaging
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Paging"/>.
        /// </summary>
        /// <param name="offset">O início da paginação. A primeira linha de resultado é 0.</param>
        /// <param name="limit">A quantidade de linhas retornadas.</param>
        /// <param name="orderBy">A ordenação.</param>
        public Paging(int offset, int limit, string orderBy = null)
        {
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Paging"/>
        /// </summary>
        /// <remarks>
        /// Paginação para as 10 primeiras linhas.
        /// </remarks>
        public Paging()
            : this(0, 10)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Paging"/>.
        /// </summary>
        /// <param name="orderBy">A ordenação.</param>
        public Paging(string orderBy)
        {
            Offset = 0;

            // TODO: ver pq ao deixar apenas int.MaxValue a consulta do sql não retorna valores.
            Limit = int.MaxValue - 1;
            OrderBy = orderBy;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o índice de início da paginação.
        /// <remarks>
        /// A primeira linha de resultado é 0
        /// </remarks>
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade de linhas retornadas.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Obtém ou define a ordenação.
        /// </summary>
        public string OrderBy { get; set; }
        #endregion
    }
}
