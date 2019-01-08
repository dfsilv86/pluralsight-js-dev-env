using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Uma proxy para simplicação de acesso ao Dapper que obtém os comandos de arquivos SQL embedados.
    /// <remarks>
    /// Os arquivos SQL devem estar na pasta SQL deste projeto e seguem o padrão nome da entidade.nome do comando.sql
    /// </remarks>    
    /// </summary>
    /// <typeparam name="TResource">O tipo da classe associada ao resource.</typeparam>
    public sealed class ResourceDapperProxy<TResource> : DapperProxy
    {
        #region Fields
        private string m_resourceName;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ResourceDapperProxy{TResource}"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados que será utilizado.</param>
        public ResourceDapperProxy(DatabaseContext dbContext)
            : base(dbContext, CommandType.Text)
        {
            m_resourceName = typeof(TResource).Name.Replace("Service", string.Empty);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executa um comando SQL.
        /// </summary>
        /// <param name="sql">O nome do comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>
        /// Normalmente a quantidade de registros afetados.
        /// </returns>
        public override int Execute(string sql, object args)
        {
            return base.Execute(GetSqlFromResource(sql), args);
        }

        /// <summary>
        /// Executa a consulta que retorna um único valor.
        /// </summary>
        /// <typeparam name="T">O tipo do dado retornado.</typeparam>
        /// <param name="sql">O nome do comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>
        /// O valor resultante da consulta.
        /// </returns>
        public override T ExecuteScalar<T>(string sql, object args)
        {
            return base.ExecuteScalar<T>(GetSqlFromResource(sql), args);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo T.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados retornados.</typeparam>
        /// <param name="sql">O nome do comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>
        /// A coleção de T.
        /// </returns>
        public override DapperQuery<T> Query<T>(string sql, object args)
        {
            return base.Query<T>(GetSqlFromResource(sql), args);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst e TSecond em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>
        /// A coleção de TReturn.
        /// </returns>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        public override DapperQuery<TFirst, TSecond, TReturn> Query<TFirst, TSecond, TReturn>(string sql, object args, Func<TFirst, TSecond, TReturn> map, string splitOn)
        {
            return base.Query<TFirst, TSecond, TReturn>(GetSqlFromResource(sql), args, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>
        /// A coleção de TReturn.
        /// </returns>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        public override DapperQuery<TFirst, TSecond, TThird, TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn)
        {
            return base.Query<TFirst, TSecond, TThird, TReturn>(GetSqlFromResource(sql), args, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
        /// <typeparam name="TFirst">O tipo dos primeiros dados retornados.</typeparam>
        /// <typeparam name="TSecond">O tipo dos segundos dados retornados.</typeparam>
        /// <typeparam name="TThird">O tipo dos terceiros dados retornados.</typeparam>
        /// <typeparam name="TFourth">O tipo dos quartos dados retornados.</typeparam>
        /// <typeparam name="TReturn">O tipo que será retornado pelo método.</typeparam>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <param name="map">A função para mapeamento de TFirst, TSecond e TThird em TReturn.</param>
        /// <param name="splitOn">O nome da coluna que será utilizada para separar os retornos de TFirst, TSecond e TSecond. Como a maioria das tabelas do sistema não utilizam o mesmo nome de coluna para as PKs, então serão necessário projetar o mesmo nome de PK para cada tabela. Veja o exemplo em Sql.Permissoes.Pesquisar.sql.</param>
        /// <returns>
        /// A coleção de TReturn.
        /// </returns>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        public override DapperQuery<TFirst, TSecond, TThird, TFourth, TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn)
        {
            return base.Query<TFirst, TSecond, TThird, TFourth, TReturn>(GetSqlFromResource(sql), args, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
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
        /// <returns>
        /// A coleção de TReturn.
        /// </returns>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        public override DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn)
        {
            return base.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(GetSqlFromResource(sql), args, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
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
        /// <returns>
        /// A coleção de TReturn.
        /// </returns>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        public override DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, string splitOn)
        {
            return base.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(GetSqlFromResource(sql), args, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna a coleção do tipo TReturn.
        /// </summary>
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
        /// <returns>
        /// A coleção de TReturn.
        /// </returns>
        /// <remarks>
        /// Utilize esse método quando desejar retornar valores de um JOIN em uma das propriedades de TReturn.
        /// </remarks>
        public override DapperQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, object args, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, string splitOn)
        {
            return base.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(GetSqlFromResource(sql), args, map, splitOn);
        }

        /// <summary>
        /// Executa a consulta com os parâmetros informados e retorna o primeiro registro do tipo T localizado.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados retornados.</typeparam>
        /// <param name="sql">O nome comando SQL.</param>
        /// <param name="args">Os parâmetros para o comando. Pode ser um objeto anônimo.</param>
        /// <returns>
        /// O tipo T localizado ou nulo caso não exista registro.
        /// </returns>
        public override T QueryOne<T>(string sql, object args)
        {
            return base.QueryOne<T>(GetSqlFromResource(sql), args);
        }

        private string GetSqlFromResource(string commandName)
        {
            return SqlResourceReader.Read(m_resourceName, commandName);
        }
        #endregion
    }
}
