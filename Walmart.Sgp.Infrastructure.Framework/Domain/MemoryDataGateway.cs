using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Uma implementação experimental de IDataGateway em memória.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    public class MemoryDataGateway<TEntity> : IDataGateway<TEntity>
        where TEntity : IEntity
    {
        #region Fields
        private static readonly Regex ReplaceLikeParameterRegex = new Regex(" LIKE @([a-z]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ReplaceParametersRegex = new Regex("@([a-z]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex GetProjectionsFromSets = new Regex(@"([a-z]+)\s*=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ReplaceFixedValuesRegex = new Regex(@"([a-z]+)\s+=\s*@([a-z]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Dictionary<Type, int> m_lastId = new Dictionary<Type, int>();
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryDataGateway{TEntity}"/>
        /// </summary>
        public MemoryDataGateway()
        {
            Entities = new List<TEntity>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define as entidades.
        /// </summary>
        public IList<TEntity> Entities { get; set; }
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
            return Find(filter, filterArgs).Count();
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
            return filters.Select(f => Count(f, filtersArgs));
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
            var existingEntity = this.FindById(id);
            Entities.Remove(existingEntity);
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
            var toDelete = Find(filter, filterArgs).ToList();

            foreach (var e in toDelete)
            {
                Entities.Remove(e);
            }
        }

        /// <summary>
        /// Pesquisa uma entidade pelo id.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A entidade caso exista uma com id informado, caso contrário null.</returns>
        public TEntity FindById(int id)
        {
            return Entities.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Obtém as entidades pelo id.
        /// </summary>
        /// <param name="ids">Os isd dsa entidades desejadas.</param>
        /// <returns>As entidades.</returns>
        public IEnumerable<TEntity> FindByIds(params int[] ids)
        {
            return Entities.Where(e => ids.Contains(e.Id));
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
            string where = ConvertFilterToDynamicLinq(filter, filterArgs);

            return Entities.Where(where);
        }

        /// <summary>
        /// Pesquisa as entidades paginadas que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// As entidades localizadas e paginadas.
        /// </returns>
        public IEnumerable<TEntity> Find(string filter, object filterArgs, Paging paging)
        {
            return Find(filter, filterArgs).Skip(paging.Offset).Take(paging.Limit);
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
            return Find(filter, filterArgs);
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
            var entities = Find(projection, filter, filterArgs);

            return EntityReflectionHelper<TEntity>.CopyProperties<TModel>(projection, entities);
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
            return Find<TModel>(projection, filter, filterArgs)
                .OrderBy(paging.OrderBy)
                .Skip(paging.Offset)
                .Take(paging.Limit);
        }

        /// <summary>
        /// Retorna todas as entidades.
        /// </summary>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        public IEnumerable<TEntity> FindAll()
        {
            return Entities;
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
            return Entities
                .OrderBy(paging.OrderBy)
                .Skip(paging.Offset)
                .Take(paging.Limit);
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
            entity.Id = GetNextId(entity);
            Entities.Add(entity);

            // Para cada propriedade lista que é do tipo de uma entidade, e não é um aggregate root também, popula o id para 
            // simular a inserção deles também.
            ForEachChildrenEntityProperty(
                entity,
                (p, child) => child.Id = GetNextId(child),
                (p, children) =>
                {
                    foreach (IEntity c in children)
                    {
                        c.Id = GetNextId(c);
                    }
                });
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
            var existingEntity = this.FindById(entity.Id);
            Entities.Remove(existingEntity);

            ForEachChildrenEntityProperty(
                entity,
                (p, child) =>
                {
                    if (child.IsNew)
                    {
                        child.Id = GetNextId(child);
                    }
                },
                (p, children) =>
                {
                    foreach (var child in children)
                    {
                        if (child.IsNew)
                        {
                            child.Id = GetNextId(child);
                        }
                    }
                });

            Entities.Add(entity);
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
            var entities = Find(filter, args);
            EntityReflectionHelper<TEntity>.CopyProperties(sets, args, entities);
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
        public void Update<TModel>(string sets, TModel model)
            where TModel : IEntity
        {
            var existingEntity = this.FindById(model.Id);
            var matches = GetProjectionsFromSets.Matches(sets);
            var projections = new List<string>();

            foreach (Match m in matches)
            {
                projections.Add(m.Groups[1].Value);
            }

            EntityReflectionHelper<TEntity>.CopyProperties(string.Join(", ", projections), model, existingEntity);
        }
        #endregion

        #region Helpers
        private static string ConvertFilterToDynamicLinq(string filter, object filterArgs)
        {
            var where = filter;

            if (filterArgs != null)
            {
                var argsType = filterArgs.GetType();

                // Like.
                where = ReplaceLikeParameterRegex.Replace(
                    where,
                    (m) =>
                    {
                        var p = EntityReflectionHelper<TEntity>.GetProperty(argsType, m.Groups[1].Value, filter);

                        return ".Contains(\"{0}\")".With(p.GetValue(filterArgs));
                    });

                // FixedValues.
                where = ReplaceFixedValuesRegex.Replace(
                    where,
                    (m) =>
                    {
                        var p = EntityReflectionHelper<TEntity>.GetProperty(argsType, m.Groups[2].Value, filter);

                        if (typeof(FixedValuesBase<string>).IsAssignableFrom(p.PropertyType))
                        {
                            var value = p.GetValue(filterArgs).ToString();

                            if (value == null)
                            {
                                return "{0}.Value = null".With(m.Groups[1].Value);
                            }

                            return "{0}.Value = \"{1}\"".With(m.Groups[1].Value, value);
                        }

                        return m.Value;
                    });

                // Parameter values.
                where = ReplaceParametersRegex.Replace(
                    where,
                    (m) =>
                    {
                        var p = EntityReflectionHelper<TEntity>.GetProperty(argsType, m.Groups[1].Value, filter);
                        var rawValue = p.GetValue(filterArgs);

                        if (rawValue == null)
                        {
                            return "null";
                        }
                        else
                        {
                            if (p.PropertyType == typeof(DateTime))
                            {
                                return "DateTime({0:yyyy,MM,dd,HH,mm,ss})".With(rawValue);
                            }
                            else
                            {
                                var value = rawValue.ToString();

                                if (p.PropertyType == typeof(string))
                                {
                                    return "\"{0}\"".With(value);
                                }

                                return value;
                            }
                        }
                    });
            }

            return where;
        }

        private static void ForEachChildrenEntityProperty(TEntity entity, Action<PropertyInfo, IEntity> singleEntityChild, Action<PropertyInfo, IEnumerable<IEntity>> listEntitiesChild)
        {
            var childrenEntities = EntityReflectionHelper<TEntity>.DiscoverChildrenEntitiesProperites();

            foreach (var l in childrenEntities)
            {
                var child = l.GetValue(entity);
                var childAsList = child as IEnumerable<IEntity>;

                if (childAsList == null)
                {
                    var childAsEntity = child as IEntity;

                    if (childAsEntity != null)
                    {
                        singleEntityChild(l, childAsEntity);
                    }
                }
                else
                {
                    listEntitiesChild(l, childAsList);
                }
            }
        }

        private int GetNextId(object entity)
        {
            var key = entity.GetType();

            if (!m_lastId.ContainsKey(key))
            {
                m_lastId.Add(key, 0);
            }

            return ++m_lastId[key];
        }
        #endregion        
    }
}
