using System;
using System.Data;
using Dapper;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// A estratégia de paginação utilizada para consultas sql.
    /// </summary>
    public class DapperTextQueryPagingStrategy : IDapperPagingStrategy
    {
        private const string DefaultPagingSql = @"
SELECT  *
FROM    ( 
			SELECT    
				ROW_NUMBER() OVER ( ORDER BY {3} ) AS RowNum, 
				__INTERNAL.*
			FROM      
				({0}) __INTERNAL
        ) AS RowConstrainedResult
WHERE   RowNum >= {1}
    AND RowNum < {2} 
ORDER BY RowNum";

        private readonly DatabaseContext m_dbContext;
        private readonly string m_pagingSql;
        private readonly string m_originalSql;
        private readonly Paging m_paging;
        private readonly object m_args;
        private string m_totalCountSql = "SELECT COUNT(1) FROM ({0}) __INTERNAL";        

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperTextQueryPagingStrategy"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados.</param>
        /// <param name="originalSql">O sql original.</param>
        /// <param name="args">Os parâmetros da consulta.</param>
        /// <param name="paging">A paginação</param>
        public DapperTextQueryPagingStrategy(
            DatabaseContext dbContext, 
            string originalSql, 
            object args, 
            Paging paging)
        {
            m_dbContext = dbContext;
            m_paging = paging;
            m_originalSql = originalSql;
            m_args = args;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperTextQueryPagingStrategy"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados.</param>
        /// <param name="originalSql">O sql original.</param>
        /// <param name="args">Os parâmetros da consulta.</param>
        /// <param name="paging">A paginação</param>
        /// <param name="pagingSql">O Sql que faz a paginação</param>
        /// <param name="totalCountSql">O Sql que faz a contagem total.</param>
        public DapperTextQueryPagingStrategy(
            DatabaseContext dbContext, 
            string originalSql, 
            object args, 
            Paging paging, 
            string pagingSql,
            string totalCountSql)
            : this(dbContext, originalSql, args, paging)
        {
            m_totalCountSql = totalCountSql;
            m_pagingSql = pagingSql;
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
        /// Obtém o número de registros contatos pelo método CountAll().
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Conta a quantidade total de registros.
        /// </summary>
        /// <returns>A quantidade total de registros.</returns>
        public int CountAll()
        {
            DapperProxy proxy = new DapperProxy(m_dbContext, CommandType.Text);

            return TotalCount = proxy.ExecuteScalar<int>(m_totalCountSql.With(m_originalSql), m_args);
        }

        /// <summary>
        /// Cria a consulta SQL paginada.
        /// </summary>
        /// <returns>A consulta com paginação.</returns>
        public string CreatePagedSql()
        {
            if (null == Paging)
            {
                return m_originalSql;
            }

            var rows = PrepareRowNumbers();
            return null == m_pagingSql ? DefaultPagingSql.With(m_originalSql, rows.Item1, rows.Item2, m_paging.OrderBy) :
                m_pagingSql.With(rows.Item1, rows.Item2, m_paging.OrderBy);            
        }

        /// <summary>
        /// Valida parâmetros de paginação e determina o número da row inicial e final para uma operação de paginação.
        /// </summary>        
        /// <returns>Tupla contendo a row inicial e final.</returns>
        private System.Tuple<int, int> PrepareRowNumbers()
        {
            int firstRow = Paging.Offset + 1;
            int lastRow = Paging.Limit == int.MaxValue ? Paging.Limit : firstRow + Paging.Limit;

            if (String.IsNullOrEmpty(Paging.OrderBy))
            {
                m_paging.OrderBy = "(select 1)";
            }

            return new Tuple<int, int>(firstRow, lastRow);
        }
    }
}
