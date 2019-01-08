using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [DebuggerDisplay("{Sql}")]
    public class DapperQuery<TReturn> : IDapperQuery, IPaginated<TReturn>
    {
        #region Fields
        private readonly DatabaseContext m_dbContext;
        private readonly string m_originalSql;
        private IEnumerable<TReturn> m_underlyingEnumerable;
        private IDapperPagingStrategy m_pagingStrategy;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TReturn}"/>.
        /// </summary>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os argumentos para o comando SQL.</param>
        /// <param name="dbContext">O contexto do banco de dados.</param>
        /// <param name="commandType">O tipo de comando.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType)
        {
            m_originalSql = sql;
            Sql = sql;
            Args = args;
            m_dbContext = dbContext;
            Transaction = dbContext.Transaction;
            Connection = Transaction.Connection;
            CommandType = commandType;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o comando SQL.
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        /// Obtém os argumentos do comando SQL.
        /// </summary>
        public object Args { get; private set; }

        /// <summary>
        /// Obtém o tipo de comando.
        /// </summary>
        public CommandType CommandType { get; private set; }

        /// <summary>
        /// Obtém a paginação utilizada.
        /// </summary>
        public IPaging Paging
        {
            get
            {
                return (m_pagingStrategy ?? DapperSoftPagingStrategy.Default).Paging;
            }
        }

        /// <summary>
        /// Obtém o total de linhas existentes.
        /// </summary>
        public int TotalCount
        {
            get
            {
                return (m_pagingStrategy ?? DapperSoftPagingStrategy.Default).TotalCount;
            }
        }

        /// <summary>
        /// Obtém a transação.
        /// </summary>
        protected IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// Obtém a conexão.
        /// </summary>
        protected IDbConnection Connection { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Transforma a query em uma query paginada.
        /// </summary>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A versão paginada da query.</returns>
        public DapperQuery<TReturn> AsPaging(Paging paging)
        {
            var textQueryPagingStrategy = new DapperTextQueryPagingStrategy(
               m_dbContext,
               m_originalSql,
               Args,
               paging);

            Sql = textQueryPagingStrategy.CreatePagedSql();
            return AsPaging(textQueryPagingStrategy);
        }

        /// <summary>
        /// Prepara uma consulta paginada especificando os comandos Sql de um recurso para paginação e contagem total.
        /// </summary>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <param name="pagingCommand">O comando sql de paginação com placeholders para offset, limit e orderBy.</param>
        /// <param name="countCommand">O comando sql de contagem.</param>
        /// <returns>A query paginada.</returns>
        /// <remarks>O comando sql no recurso especificado por pagingCommandName pode utilizar o placeholder {0} para o offset, {1} para limit e {2} para o orderBy.</remarks>
        public DapperQuery<TReturn> AsPaging(Paging paging, CommandInfo pagingCommand, CommandInfo countCommand)
        {
            var textQueryPagingStrategy = new DapperTextQueryPagingStrategy(
               m_dbContext,
               m_originalSql,
               Args,
               paging,
               pagingCommand.Read(),
               countCommand.Read());

            Sql = textQueryPagingStrategy.CreatePagedSql();
            return AsPaging(textQueryPagingStrategy);
        }

        /// <summary>
        /// Prepara uma consulta paginada especificando uma estratégia de paginação.
        /// </summary>
        /// <param name="pagingStrategy">A estratégia de paginação.</param>
        /// <returns>A versão paginada da query.</returns>
        public DapperQuery<TReturn> AsPaging(IDapperPagingStrategy pagingStrategy)
        {
            if (null != m_pagingStrategy)
            {
                throw new InvalidOperationException(Texts.QueryAlreadyPaged);
            }

            if (null != pagingStrategy.Paging)
            {
                m_pagingStrategy = pagingStrategy;
            }

            return this;
        }

        /// <summary>
        /// Transforma a consulta para retornar o resultado em memória informado.
        /// </summary>
        /// <remarks>
        /// Serão utilizadas as informações de paginação da consulta atual.
        /// </remarks>
        /// <param name="memoryResult">O resultado em memória.</param>
        /// <returns>O resultado.</returns>
        public MemoryResult<TReturn> AsMemoryResult(IEnumerable<TReturn> memoryResult)
        {
            Perform();
            return new MemoryResult<TReturn>(this, memoryResult);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TReturn> GetEnumerator()
        {
            Perform();

            return m_underlyingEnumerable.GetEnumerator();
        }

        /// <summary>
        /// Realiza a consulta.
        /// </summary>
        public void Perform()
        {
            if (m_underlyingEnumerable == null)
            {
#if SQL_DIAGNOSTICS
                m_underlyingEnumerable = this.ExecuteWithDiagnostics(Execute);
#else
                m_underlyingEnumerable = Execute();
#endif

                if (m_pagingStrategy == null)
                {
                    m_pagingStrategy = DapperSoftPagingStrategy.Default;
                }
                else
                {
                    m_pagingStrategy.CountAll();
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>O resultado da query.</returns>
        protected virtual IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TReturn>(Sql, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout);
        }
        #endregion
    }

    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
    /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class DapperQuery<TFirst, TSecond, TReturn> : DapperQuery<TReturn>
    {
        #region Fields
        private Func<TFirst, TSecond, TReturn> m_map;
        private string m_splitOn;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TFirst, TSecond, TReturn}"/>.
        /// </summary>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os argumentos para o comando SQL.</param>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">O tipo de comando.</param>
        /// <param name="map">A função de mapeamento dos dados retornardos pela query.</param>
        /// <param name="splitOn">O nome das colunas que será utilizadas para marcar a separação entre os dados.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType, Func<TFirst, TSecond, TReturn> map, string splitOn)
            : base(sql, args, dbContext, commandType)
        {
            m_map = map;
            m_splitOn = splitOn;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>
        /// O resultado da query.
        /// </returns>
        protected override IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TFirst, TSecond, TReturn>(Sql, m_map, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout, splitOn: m_splitOn);
        }
        #endregion
    }

    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
    /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
    /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class DapperQuery<TFirst, TSecond, TThird, TReturn> : DapperQuery<TReturn>
    {
        #region Fields
        private Func<TFirst, TSecond, TThird, TReturn> m_map;
        private string m_splitOn;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TFirst, TSecond, TThird, TReturn}"/>.
        /// </summary>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os argumentos para o comando SQL.</param>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">O tipo de comando.</param>
        /// <param name="map">A função de mapeamento dos dados retornardos pela query.</param>
        /// <param name="splitOn">O nome das colunas que será utilizadas para marcar a separação entre os dados.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn)
            : base(sql, args, dbContext, commandType)
        {
            m_map = map;
            m_splitOn = splitOn;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>
        /// O resultado da query.
        /// </returns>
        protected override IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TFirst, TSecond, TThird, TReturn>(Sql, m_map, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout, splitOn: m_splitOn);
        }
        #endregion
    }

    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
    /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
    /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
    /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class DapperQuery<TFirst, TSecond, TThird, TFourth, TReturn> : DapperQuery<TReturn>
    {
        #region Fields
        private Func<TFirst, TSecond, TThird, TFourth, TReturn> m_map;
        private string m_splitOn;
        #endregion

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TFirst, TSecond, TThird, TFourth, TReturn}"/>.
        /// </summary>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os argumentos para o comando SQL.</param>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">O tipo de comando.</param>
        /// <param name="map">A função de mapeamento dos dados retornardos pela query.</param>
        /// <param name="splitOn">O nome das colunas que será utilizadas para marcar a separação entre os dados.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn)
            : base(sql, args, dbContext, commandType)
        {
            m_map = map;
            m_splitOn = splitOn;
        }

        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>
        /// O resultado da query.
        /// </returns>
        protected override IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(Sql, m_map, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout, splitOn: m_splitOn);
        }
    }

    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
    /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
    /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
    /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
    /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> : DapperQuery<TReturn>
    {
        #region Fields
        private Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> m_map;
        private string m_splitOn;
        #endregion

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TFirst, TSecond, TThird, TFourth, TFifth, TReturn}"/>.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="map">The map.</param>
        /// <param name="splitOn">The split on.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn)
            : base(sql, args, dbContext, commandType)
        {
            m_map = map;
            m_splitOn = splitOn;
        }

        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>
        /// O resultado da query.
        /// </returns>
        protected override IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Sql, m_map, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout, splitOn: m_splitOn);
        }
    }

    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
    /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
    /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
    /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
    /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
    /// <typeparam name="TSixth">O tipo dos sextos dados retornados.</typeparam>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> : DapperQuery<TReturn>
    {
        #region Fields
        private Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> m_map;
        private string m_splitOn;
        #endregion

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn}"/>.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>        
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="map">The map.</param>
        /// <param name="splitOn">The split on.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, string splitOn)
            : base(sql, args, dbContext, commandType)
        {
            m_map = map;
            m_splitOn = splitOn;
        }

        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>
        /// O resultado da query.
        /// </returns>
        protected override IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Sql, m_map, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout, splitOn: m_splitOn);
        }
    }

    /// <summary>
    /// Representa uma query a ser executada via Dapper.
    /// </summary>
    /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
    /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
    /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
    /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
    /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
    /// <typeparam name="TSixth">O tipo dos sextos dados retornados.</typeparam>
    /// <typeparam name="TSeventh">O tipo dos sétimos dados retornados.</typeparam>
    /// <typeparam name="TReturn">O tipo de retorno da query.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> : DapperQuery<TReturn>
    {
        #region Fields
        private Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> m_map;
        private string m_splitOn;
        #endregion

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperQuery{TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn}"/>.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="map">The map.</param>
        /// <param name="splitOn">The split on.</param>
        public DapperQuery(string sql, object args, DatabaseContext dbContext, CommandType commandType, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, string splitOn)
            : base(sql, args, dbContext, commandType)
        {
            m_map = map;
            m_splitOn = splitOn;
        }

        /// <summary>
        /// Executa a query.
        /// </summary>
        /// <returns>
        /// O resultado da query.
        /// </returns>
        protected override IEnumerable<TReturn> Execute()
        {
            return Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Sql, m_map, Args, Transaction, commandType: CommandType, commandTimeout: DapperProxy.DefaultCommandTimeout, splitOn: m_splitOn);
        }
    }
}