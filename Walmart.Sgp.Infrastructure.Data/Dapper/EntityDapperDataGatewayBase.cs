using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Data;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Classe base para table data gateways utilizando o Dapper que utilizam uma entidade primária.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    public abstract class EntityDapperDataGatewayBase<TEntity> : DapperDataGatewayBase<TEntity>, IDataGateway<TEntity>
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
        /// Inicia uma nova instância da classe <see cref="EntityDapperDataGatewayBase{TEntity}"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados.</param>
        /// <param name="tableName">O nome da tabela.</param>
        /// <param name="idColumnName">O nome da coluna Id (PK).</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected EntityDapperDataGatewayBase(DatabaseContext dbContext, string tableName, string idColumnName = "Id")
            : base(dbContext)
        {
            m_tableName = tableName;
            m_idColumnName = idColumnName;

            EntityTableModelRegistry.RegisterEntityTableModel(typeof(TEntity), tableName, idColumnName);
#if DEBUG
            if (null == typeof(TEntity).GetProperty(idColumnName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy))
            {
                throw new InvalidOperationException(Texts.CouldNotFindEntityPropertyWithName.With(this.GetType().Name, typeof(TEntity).GetType().Name, idColumnName));
            }

            // Verifica se a coluna ID foi usada no ColumnsName.
            if (ColumnsName.Contains(idColumnName))
            {
                throw new InvalidOperationException(Texts.IdColumnShouldNotBeUsedOnColumnsName.With(GetType().Name, idColumnName));
            }
#endif
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EntityDapperDataGatewayBase{TEntity}"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados a ser utilizado.</param>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected EntityDapperDataGatewayBase(DatabaseContext dbContext)
            : this(dbContext, typeof(TEntity).Name)
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
        /// Pesquisa uma entidade pelo id.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A entidade caso exista uma com id informado, caso contrário null.</returns>
        public virtual TEntity FindById(int id)
        {
            return Find("{0} = @id".With(m_idColumnName), new { id }).FirstOrDefault();
        }

        /// <summary>
        /// Obtém as entidades pelo id.
        /// </summary>
        /// <param name="ids">Os isd dsa entidades desejadas.</param>
        /// <returns>As entidades.</returns>
        public virtual IEnumerable<TEntity> FindByIds(params int[] ids)
        {
            // TODO: tratar lista de ids muito grande em razão dos limites do SQL Server IN CLAUSE.
            return Find("{0} IN @ids".With(m_idColumnName), new { ids });
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
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual IEnumerable<TEntity> Find(string projection, string filter, object filterArgs)
        {
            return Find<TEntity>(projection, filter, filterArgs);
        }

        /// <summary>
        /// Retorna todas as entidades.
        /// </summary>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        public virtual IEnumerable<TEntity> FindAll()
        {
            var sql = "SELECT {0}, {1} FROM {2}".With(m_idColumnName, BuildColumnsProjection(), m_tableName);

            return Command.Query<TEntity>(sql, null);
        }

        /// <summary>
        /// Retorna as entidades utilizando a paginação.
        /// </summary>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        public IEnumerable<TEntity> FindAll(Paging paging)
        {
            var sql = "SELECT {0}, {1} FROM {2}".With(m_idColumnName, BuildColumnsProjection(), m_tableName);

            return Command.Query<TEntity>(sql, null).AsPaging(paging);
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado e preenche apenas as propriedades projetadas.
        /// </summary>
        /// <typeparam name="TModel">O modelo a ser utilizado ao invés de toda a entidade. Útil quando é necessário mapear  um modelo de objeto mais enxuto, como um DTO ou uma ViewModel.</typeparam>
        /// <param name="projection">As propriedades a serem projetadas. Exemplo: Username, Email</param>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>        
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public virtual IEnumerable<TModel> Find<TModel>(string projection, string filter, object filterArgs)
        {
            var sql = "SELECT {0}, {1} FROM {2} WHERE {3}".With(m_idColumnName, projection, m_tableName, filter);

            return Command.Query<TModel>(sql, filterArgs);
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado e preenche apenas as propriedades projetadas.
        /// </summary>
        /// <typeparam name="TModel">O modelo a ser utilizado ao invés de toda a entidade. Útil quando é necessário mapear  um modelo de objeto mais enxuto, como um DTO ou uma ViewModel.</typeparam>
        /// <param name="projection">As propriedades a serem projetadas. Exemplo: Username, Email</param>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        public IEnumerable<TModel> Find<TModel>(string projection, string filter, object filterArgs, Paging paging)
        {
            var sql = "SELECT {0}, {1} FROM {2} WHERE {3}".With(m_idColumnName, projection, m_tableName, filter);

            return Command.Query<TModel>(sql, filterArgs).AsPaging(paging);
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active. Expressões válidas em uma cláusula where SQL são aceitas, como LIKE.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual IEnumerable<TEntity> Find(string filter, object filterArgs)
        {
            return Find(BuildColumnsProjection(), filter, filterArgs);
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado com paginação.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// As entidades localizadas e paginadas.
        /// </returns>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual IEnumerable<TEntity> Find(string filter, object filterArgs, Paging paging)
        {
            return ((DapperQuery<TEntity>)Find(BuildColumnsProjection(), filter, filterArgs)).AsPaging(paging);
        }

        /// <summary>
        /// Contas as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// A quantidade de entidades.
        /// </returns>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        public virtual long Count(string filter, object filterArgs)
        {
            var sql = BuildSelectCount(filter);

            return Command.ExecuteScalar<long>(sql, filterArgs);
        }

        /// <summary>
        /// Contas as entidades que correspondem a cada um dos filtros informados e retorna uma lista com os valores.
        /// </summary>
        /// <param name="filtersArgs">Os argumentos para os filtros. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <param name="filters">Os filtros no formato: Username = @Username AND Active = @Active.</param>        
        /// <returns>
        /// A lista com as quantidades de entidades.
        /// </returns>
        public IEnumerable<long> Count(object filtersArgs, params string[] filters)
        {
            var selectCounts = filters.Select(f => BuildSelectCount(f));
            var sql = String.Join(" UNION ALL ", selectCounts); 

            return Command.Query<long>(sql, filtersArgs);
        }

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser excluído não exista.</exception>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>        
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        public virtual void Delete(int id)
        {
            var sql = "DELETE FROM {0} WHERE {1} = @Id".With(m_tableName, m_idColumnName);

            Run(() =>
            {
                if (Command.Execute(sql, new { Id = id }) == 0)
                {
                    throw new InvalidOperationException(Texts.EntityNotDeleted.With(m_tableName, id));
                }
            });
        }

        /// <summary>
        /// Exclui as entidades que corresponderem ao filtro informado.
        /// </summary>
        /// <param name="filter">A cláusula WHERE definindo quais entidades serão excluídas. Exemplo: Name = @Name.</param>
        /// <param name="filterArgs">O objeto anônimo com os argumentos para o filtro. Exemplo: new { Name = "Test" }.</param>
        /// <remarks>
        /// Excluirá todos os registros que corresponderem ao filtro.
        /// </remarks>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        public virtual void Delete(string filter, object filterArgs)
        {
            var sql = "DELETE FROM {0} WHERE {1}".With(m_tableName, filter);

            Run(() =>
            {
                Command.Execute(sql, filterArgs);
            });
        }

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual void Insert(TEntity entity)
        {
            var projections = BuildColumnsProjection();
            var parameters = BuildColumnsParameter();
            PerformInsert(entity, projections, parameters);            
        }

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="projections">As projeções dos campos a serem considerados no insert.</param>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public virtual void Insert(string projections, TEntity entity)
        {
            var parameters = "@{0}".With(String.Join(", @", projections.Replace(" ", string.Empty).Split(',')));
            PerformInsert(entity, projections, parameters);
        }       

        /// <summary>
        /// Insere as novas entidade em lote, mas, por razões de performance, não preenche as propriedades Id dos novos registros criados.
        /// </summary>
        /// <param name="entities">As entidades a serem inseridas.</param>
        /// <remarks>
        /// Novos registros serão criados no banco.
        /// </remarks>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            //// Inserção em lote:  http://stackoverflow.com/a/6500834/956886
            var projections = BuildColumnsProjection();
            var parameters = BuildColumnsParameter();

            var sql = "INSERT INTO {0} ({1}) VALUES({2})".With(m_tableName, projections, parameters);

            Run(() =>
            {
                Command.Execute(sql, entities);
            });
        }

        /// <summary>
        /// Atualiza uma entidade existente utilizando um modelo.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="sets">A cláusula SET definindo quais propriedades serão atualizadas. Exemplo: Username = @NewUsername, Email = @Email.</param>
        /// <param name="model">O modelo a ser utilizado ao invés de toda a entidade. Útil quando é necessário mapear  um modelo de objeto mais enxuto, como um DTO ou uma ViewModel.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser atualizado não exista.</exception>
        /// <remarks>
        /// Será atualizada a entidade que possui o Id informado no modelo.
        /// </remarks>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual void Update<TModel>(string sets, TModel model)
            where TModel : IEntity
        {
            var sql = "UPDATE {0} SET {1} WHERE {2} = @Id".With(m_tableName, sets, m_idColumnName);

            Run(() =>
            {
                if (Command.Execute(sql, model) == 0)
                {
                    throw new InvalidOperationException(Texts.EntityNotUpdated.With(m_tableName, model.Id));
                }
            });
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser atualizado não exista.</exception>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        public virtual void Update(TEntity entity)
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
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public virtual void Update(string sets, string filter, object args)
        {
            var sql = "UPDATE {0} SET {1} WHERE {2}".With(m_tableName, sets, filter);

            Run(() =>
            {
                Command.Execute(sql, args);
            });
        }
        #endregion

        #region Helpers
        private static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:
                        if (ex.Message.Contains("DELETE"))
                        {
                            throw new UserInvalidOperationException(Texts.ForeignKeyViolationExceptionMessage);
                        }

                        throw;

                    case 8152:
                        throw new UserInvalidOperationException(Texts.FieldLengthLimitExceeded);
                }

                throw;
            }
        }

        private string BuildColumnsProjection()
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

        private string BuildSelectCount(string filter)
        {
            return "SELECT COUNT(1) FROM {0} WHERE {1}".With(m_tableName, filter);
        }

        private void PerformInsert(TEntity entity, string projections, string parameters)
        {
            // SCOPE_IDENTITY(): http://stackoverflow.com/a/8270264/956886
            var sql = "INSERT INTO {0} ({1}) VALUES({2});SELECT CAST(SCOPE_IDENTITY() as int)".With(m_tableName, projections, parameters);

            Run(() =>
            {
                entity.Id = Command.QueryOne<int>(sql, entity);
            });
        }
        #endregion        
    }
}
