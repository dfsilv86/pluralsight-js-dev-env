using System.Diagnostics.CodeAnalysis;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// A estratégia recebe os valores de paginação externos.
    /// </summary>
    public class DapperSoftPagingStrategy : IDapperPagingStrategy
    {
        /// <summary>
        /// Instância padrão.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DapperSoftPagingStrategy Default = new DapperSoftPagingStrategy(new Paging
        {
            Limit = int.MaxValue,
            Offset = 0
        })
        {
            TotalCount = int.MaxValue
        };

        private readonly IPaging m_paging;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSoftPagingStrategy"/>
        /// </summary>
        /// <param name="paging">A paginação.</param>
        public DapperSoftPagingStrategy(IPaging paging)
        {
            m_paging = paging;            
        }

        /// <summary>
        /// Obtém a paginação.
        /// </summary>
        public IPaging Paging
        {
            get
            {
                return m_paging;
            }
        }

        /// <summary>
        /// Obtém ou define a quantidade total de registros.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Conta a quantidade total de registros.
        /// </summary>
        /// <returns>A quantidade total de registros.</returns>
        public int CountAll()
        {
            return TotalCount;
        }
    }
}
