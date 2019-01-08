#if ADO_BENCHMARK
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway base para entidades utilizand o ADO .NET.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    public abstract class EntityAdoDataGatewayBase<TEntity> : AdoDataGatewayBase<TEntity>, IDataGateway<TEntity>
        where TEntity : IEntity
    {
        #region Fields
        private static ConcurrentDictionary<string, string> s_columnsProjection = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> s_columnsParameter = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> s_columnsSet = new ConcurrentDictionary<string, string>();
        private string m_tableName;
        private string m_idColumnName;
        #endregion

        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EntityAdoDataGatewayBase{TEntity}"/>.
        /// </summary>
        /// <param name="transaction">A transação.</param>
        /// <param name="tableName">O nome da tabela.</param>
        /// <param name="idColumnName">O nome da coluna id.</param>
        protected EntityAdoDataGatewayBase(SqlTransaction transaction, string tableName, string idColumnName = "Id")
            : base(transaction)
        {
            m_tableName = tableName;
            m_idColumnName = idColumnName;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EntityAdoDataGatewayBase{TEntity}"/>.
        /// </summary>
        /// <param name="transaction">A transação.</param>
        protected EntityAdoDataGatewayBase(SqlTransaction transaction)
            : this(transaction, typeof(TEntity).Name)
        {
        }
        #endregion

        #region Properties                          
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected abstract IEnumerable<string> ColumnsName { get; }
        #endregion

        #region Methods        
        /// <summary>
        /// Contas as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// A quantidade de entidades.
        /// </returns>
        public long Count(string filter, object filterArgs)
        {
            var sql = "SELECT COUNT(1) FROM {0} WHERE {1}".With(m_tableName, filter);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, filterArgs);

            return (int)cmd.ExecuteScalar();
        }

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>
        public void Delete(int id)
        {
            var cmd = CreateCommand();
            cmd.CommandText = "DELETE FROM {0} WHERE {1} = @Id".With(m_tableName, m_idColumnName);
            cmd.Parameters.Add(new SqlParameter("@id", id));

            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new InvalidOperationException(Texts.EntityNotDeleted.With(m_tableName, id));
            }
        }

        /// <summary>
        /// Exclui as entidades que corresponderem ao filtro informado.
        /// </summary>
        /// <param name="filter">A cláusula WHERE definindo quais entidades serão excluídas. Exemplo: Name = @Name.</param>
        /// <param name="filterArgs">O objeto anônimo com os argumentos para o filtro. Exemplo: new { Name = "Test" }.</param>
        /// <remarks>
        /// Excluirá todos os registros que corresponderem ao filtro.
        /// </remarks>
        public void Delete(string filter, object filterArgs)
        {
            var sql = "DELETE FROM {0} WHERE {1}".With(m_tableName, filter);

            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, filterArgs);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active. Expressões válidas em uma cláusula where SQL são aceitas, como LIKE.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        public IEnumerable<TEntity> Find(string filter, object filterArgs)
        {
            return Find(BuildColumnsProjection(), filter, filterArgs);
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Não implementado.</exception>
        public IEnumerable<TEntity> Find(string filter, object filterArgs, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado e preenche apenas as propriedades projetadas.
        /// </summary>
        /// <param name="projection">As propriedades a serem projetadas. Exemplo: Username, Email</param>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        public IEnumerable<TEntity> Find(string projection, string filter, object filterArgs)
        {
            return Find<TEntity>(projection, filter, filterArgs);
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado e preenche apenas as propriedades projetadas.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="projection">As propriedades a serem projetadas. Exemplo: Username, Email</param>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        public IEnumerable<TModel> Find<TModel>(string projection, string filter, object filterArgs)
        {
            var sql = "SELECT {0}, {1} FROM {2} WHERE {3}".With(m_idColumnName, projection, m_tableName, filter);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, filterArgs);

            return Map<TModel>(cmd, projection);
        }

        /// <summary>
        /// Retorna todas as entidades.
        /// </summary>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        public IEnumerable<TEntity> FindAll()
        {
            var projection = BuildColumnsProjection();
            var sql = "SELECT {0}, {1} FROM {2}".With(m_idColumnName, projection, m_tableName);
            var cmd = CreateCommand();
            cmd.CommandText = sql;

            return Map<TEntity>(cmd, projection);
        }

        /// <summary>
        /// Obtém pelo id.
        /// </summary>
        /// <param name="id">O id da entidad desejada.</param>
        /// <returns>A entidade.</returns>
        public TEntity FindById(int id)
        {
            return Find("{0} = @Id".With(m_idColumnName), new { Id = id }).FirstOrDefault();
        }

        /// <summary>
        /// Insere as novas entidade em lote, mas, por razões de performance, não preenche as propriedades Id dos novos registros criados.
        /// </summary>
        /// <param name="entities">As entidades a serem inseridas.</param>
        /// <remarks>
        /// Novos registros serão criados no banco.
        /// </remarks>
        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                Insert(e);
            }
        }

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public void Insert(TEntity entity)
        {
            var projections = BuildColumnsProjection();
            var parameters = BuildColumnsParameter();
            var entityType = typeof(TEntity);

            // SCOPE_IDENTITY(): http://stackoverflow.com/a/8270264/956886
            var cmd = CreateCommand();
            cmd.CommandText = "INSERT INTO {0} ({1}) VALUES({2});SELECT CAST(SCOPE_IDENTITY() as int)".With(m_tableName, projections, parameters);

            foreach (var c in ColumnsName)
            {
                var property = entityType.GetProperty(c, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                var propertyValue = property.GetValue(entity);

                if (propertyValue == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@{0}".With(c), DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@{0}".With(c), propertyValue));
                }
            }

            entity.Id = (int)cmd.ExecuteScalar();
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        public void Update(TEntity entity)
        {
            Update(BuildColumnsSet(), entity);
        }

        /// <summary>
        /// Atualiza as entidades que corresponderem ao filtro informado.
        /// </summary>
        /// <param name="sets">A cláusula SET definindo quais propriedades serão atualizadas. Exemplo: Username = @NewUsername, Email = @Email.</param>
        /// <param name="filter">A cláusula WHERE definindo quais entidades serão atualizadas. Exemplo: Email = @OldEmail AND Name LIKE 'TEST%'.</param>
        /// <param name="args">O objeto anônimo com os argumentos tanto para sets quanto para filter. Exemplo: new { NewUsername = "xpto", Email = "xpto@xpto.com.br", OldEmail = "old@xpto.com.br" }.</param>
        /// <remarks>
        /// Atualizará todos os registros que corresponderem ao filtro.
        /// </remarks>
        public void Update(string sets, string filter, object args)
        {
            var sql = "UPDATE {0} SET {1} WHERE {2}".With(m_tableName, sets, filter);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, args);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Atualiza uma entidade existente utilizando um modelo.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="sets">A cláusula SET definindo quais propriedades serão atualizadas. Exemplo: Username = @NewUsername, Email = @Email.</param>
        /// <param name="model">O modelo a ser utilizado ao invés de toda a entidade. Útil quando é necessário mapear  um modelo de objeto mais enxuto, como um DTO ou uma ViewModel.</param>
        /// <remarks>
        /// Será atualizada a entidade que possui o Id informado no modelo.
        /// </remarks>
        public void Update<TModel>(string sets, TModel model) where TModel : IEntity
        {
            var sql = "UPDATE {0} SET {1} WHERE {2} = @Id".With(m_tableName, sets, m_idColumnName);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, model);

            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new InvalidOperationException(Texts.EntityNotUpdated.With(m_tableName, model.Id));
            }
        }
        #endregion

        #region Helpers                   
        /// <summary>
        /// Cria as projeções de colunas.
        /// </summary>
        /// <returns>As projeções.</returns>
        protected string BuildColumnsProjection()
        {
            if (!s_columnsProjection.ContainsKey(m_tableName))
            {
                s_columnsProjection.TryAdd(m_tableName, string.Join(", ", ColumnsName));
            }

            return s_columnsProjection[m_tableName];
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        private string BuildColumnsParameter()
        {
            if (!s_columnsParameter.ContainsKey(m_tableName))
            {
                s_columnsParameter.TryAdd(m_tableName, string.Join(", ", ColumnsName.Select(c => "@{0}".With(c))));
            }

            return s_columnsParameter[m_tableName];
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        private string BuildColumnsSet()
        {
            if (!s_columnsSet.ContainsKey(m_tableName))
            {
                s_columnsSet.TryAdd(m_tableName, string.Join(", ", ColumnsName.Select(c => "{0} = @{0}".With(c))));
            }

            return s_columnsSet[m_tableName];
        }
        #endregion
    }
}
#endif