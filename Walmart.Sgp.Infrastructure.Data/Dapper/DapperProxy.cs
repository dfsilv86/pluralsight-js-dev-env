using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Uma proxy para simplicação de acesso ao Dapper.
    /// </summary>
    public class DapperProxy
    {
        #region Fields
        private DatabaseContext m_dbContext;
        private CommandType m_commandType;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia os membros estáticos da classe <see cref="DapperProxy"/>.
        /// </summary>
        static DapperProxy()
        {
#if CI
            DefaultCommandTimeout = 300;
#else
            DefaultCommandTimeout = 60;
#endif
            var allTypes = typeof(Usuario).Assembly.GetTypes();
            AddTypeHandlerToFixedValue<string>(allTypes);
            AddTypeHandlerToFixedValue<int>(allTypes);
            AddTypeHandlerToFixedValue<char?>(allTypes);
            AddTypeHandlerToFixedValue<short?>(allTypes);
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperProxy"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>
        /// <param name="commandType">O tipo do comando.</param>
        public DapperProxy(DatabaseContext dbContext, CommandType commandType)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            m_dbContext = dbContext;
            m_commandType = commandType;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o timeout padrão para a execução dos comandos.
        /// </summary>
        public static int? DefaultCommandTimeout { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo T.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados retornados.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>A coleção de T.</returns>
        public virtual DapperQuery<T> Query<T>(string sql, object args)
        {
            return new DapperQuery<T>(sql, args, m_dbContext, m_commandType);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst e TSecond em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>A coleção de TReturn.</returns>
        public virtual DapperQuery<TFirst, TSecond, TReturn> Query<TFirst, TSecond, TReturn>(string sql, object args, Func<TFirst, TSecond, TReturn> map, string splitOn)
        {
            return new DapperQuery<TFirst, TSecond, TReturn>(sql, args, m_dbContext, m_commandType, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>A coleção de TReturn.</returns>
        public virtual DapperQuery<TFirst, TSecond, TThird, TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn)
        {
            return new DapperQuery<TFirst, TSecond, TThird, TReturn>(sql, args, m_dbContext, m_commandType, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>A coleção de TReturn.</returns>
        public virtual DapperQuery<TFirst, TSecond, TThird, TFourth, TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn)
        {
            return new DapperQuery<TFirst, TSecond, TThird, TFourth, TReturn>(sql, args, m_dbContext, m_commandType, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
        /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>A coleção de TReturn.</returns>
        public virtual DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn)
        {
            return new DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, args, m_dbContext, m_commandType, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
        /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
        /// <typeparam name="TSixth">O tipo dos sextos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>A coleção de TReturn.</returns>
        public virtual DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, string splitOn)
        {
            return new DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, args, m_dbContext, m_commandType, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
        /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
        /// <typeparam name="TSixth">O tipo dos sextos dados retornados.</typeparam>
        /// <typeparam name="TSeventh">O tipo dos sétimos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>A coleção de TReturn.</returns>
        public virtual DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, string splitOn)
        {
            return new DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, args, m_dbContext, m_commandType, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna o primeiro registro do tipo T localizado.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados retornados.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>O tipo T localizado ou nulo caso não exista registro.</returns>
        public virtual T QueryOne<T>(string sql, object args)
        {
#if SQL_DIAGNOSTICS
            return this.ExecuteWithDiagnostics(
                sql,
                args,
                () => m_dbContext.Connection.Query<T>(sql, args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout).SingleOrDefault());
#else
            return m_dbContext.Connection.Query<T>(sql, args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout).SingleOrDefault();
#endif
        }

        /// <summary>
        /// Executa a consulta que retorna um único valor.
        /// </summary>
        /// <typeparam name="T">O tipo do dado retornado.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>O valor resultante da consulta.</returns>
        public virtual T ExecuteScalar<T>(string sql, object args)
        {
#if SQL_DIAGNOSTICS
            return this.ExecuteWithDiagnostics(
                sql,
                args,
                () => m_dbContext.Connection.ExecuteScalar<T>(sql, args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout));
#else
            return m_dbContext.Connection.ExecuteScalar<T>(sql, args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout);
#endif
        }

        /// <summary>
        /// Executa um comando SQL.
        /// </summary>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>Normalmente a quantidade de registros afetados.</returns>
        public virtual int Execute(string sql, object args)
        {
#if SQL_DIAGNOSTICS
            return this.ExecuteWithDiagnostics(
                sql,
                args,
                () => m_dbContext.Connection.Execute(sql, args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout));
#else
            return m_dbContext.Connection.Execute(sql, args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout);
#endif
        }

        /// <summary>
        /// Executa um comando que retorna múltiplos resultados, sem relacionamento entre si.
        /// </summary>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
        /// <typeparam name="TFifth">O tipo dos quintos dados retornados.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>Uma lista de resultados.</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>> QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth>(string sql, object args)
        {
            global::Dapper.SqlMapper.GridReader reader;
#if SQL_DIAGNOSTICS
            reader = this.ExecuteWithDiagnostics(
                sql,
                args,
                () => m_dbContext.Connection.QueryMultiple("PR_SelecionarRelacionamentos", args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout));
#else
            reader = m_dbContext.Connection.QueryMultiple("PR_SelecionarRelacionamentos", args, m_dbContext.Transaction, commandType: m_commandType, commandTimeout: DefaultCommandTimeout);
#endif
            return new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>>(reader.Read<TFirst>(), reader.Read<TSecond>(), reader.Read<TThird>(), reader.Read<TFourth>(), reader.Read<TFifth>());
        }

        private static void AddTypeHandlerToFixedValue<TUnderlyingValue>(Type[] allTypes)
        {
            var fixedValueBaseType = typeof(FixedValuesBase<TUnderlyingValue>);
            var fixedValueBaseTypes = allTypes.Where(t => fixedValueBaseType.IsAssignableFrom(t));

            foreach (var t in fixedValueBaseTypes)
            {
                SqlMapper.AddTypeHandler(t, new FixedValuesTypeHandler(t));
            }
        }
        #endregion
    }
}
