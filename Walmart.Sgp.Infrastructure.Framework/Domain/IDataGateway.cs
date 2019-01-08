using System.Collections.Generic;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface para as classes de acesso a dados utilizando o pattern Table Data Gateway.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    public interface IDataGateway<TEntity>
        where TEntity : IEntity
    {
        #region Methods
        /// <summary>
        /// Pesquisa uma entidade pelo id.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A entidade caso exista uma com id informado, caso contrário null.</returns>
        TEntity FindById(int id);

        /// <summary>
        /// Obtém as entidades pelo id.
        /// </summary>
        /// <param name="ids">Os ids das entidades desejadas.</param>
        /// <returns>As entidades.</returns>
        IEnumerable<TEntity> FindByIds(params int[] ids);

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        IEnumerable<TEntity> Find(string filter, object filterArgs);

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado com paginação.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// As entidades localizadas e paginadas.
        /// </returns>
        IEnumerable<TEntity> Find(string filter, object filterArgs, Paging paging);

        /// <summary>
        /// Retorna todas as entidades.
        /// </summary>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        IEnumerable<TEntity> FindAll();

        /// <summary>
        /// Retorna as entidades utilizando a paginação.
        /// </summary>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        IEnumerable<TEntity> FindAll(Paging paging);

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado e preenche apenas as propriedades projetadas.
        /// </summary>
        /// <param name="projection">As propriedades a serem projetadas. Exemplo: Username, Email</param>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        IEnumerable<TEntity> Find(string projection, string filter, object filterArgs);

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
        IEnumerable<TModel> Find<TModel>(string projection, string filter, object filterArgs);

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
        IEnumerable<TModel> Find<TModel>(string projection, string filter, object filterArgs, Paging paging);

        /// <summary>
        /// Contas as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// A quantidade de entidades.
        /// </returns>
        long Count(string filter, object filterArgs);

        /// <summary>
        /// Contas as entidades que correspondem a cada um dos filtros informados e retorna uma lista com os valores.
        /// </summary>
        /// <param name="filtersArgs">Os argumentos para os filtros. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <param name="filters">Os filtros no formato: Username = @Username AND Active = @Active.</param>        
        /// <returns>
        /// A lista com as quantidades de entidades.
        /// </returns>
        IEnumerable<long> Count(object filtersArgs, params string[] filters);

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        void Insert(TEntity entity);

        /// <summary>
        /// Insere as novas entidade em lote, mas, por razões de performance, não preenche as propriedades Id dos novos registros criados.
        /// </summary>
        /// <param name="entities">As entidades a serem inseridas.</param>
        /// <remarks>
        /// Novos registros serão criados no banco.
        /// </remarks>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        void Update(TEntity entity);

        /// <summary>
        /// Atualiza uma entidade existente utilizando um modelo.
        /// </summary>
        /// <typeparam name="TModel">O tipo do modelo.</typeparam>
        /// <param name="sets">A cláusula SET definindo quais propriedades serão atualizadas. Exemplo: Username = @NewUsername, Email = @Email.</param>
        /// <param name="model">O modelo a ser utilizado ao invés de toda a entidade. Útil quando é necessário mapear  um modelo de objeto mais enxuto, como um DTO ou uma ViewModel.</param>
        /// <remarks>
        /// Será atualizada a entidade que possui o Id informado no modelo.
        /// </remarks>
        void Update<TModel>(string sets, TModel model) 
            where TModel : IEntity;

        /// <summary>
        /// Atualiza as entidades que corresponderem ao filtro informado.
        /// </summary>
        /// <param name="sets">A cláusula SET definindo quais propriedades serão atualizadas. Exemplo: Username = @NewUsername, Email = @Email.</param>
        /// <param name="filter">A cláusula WHERE definindo quais entidades serão atualizadas. Exemplo: Email = @OldEmail AND Name LIKE 'TEST%'.</param>
        /// <param name="args">O objeto anônimo com os argumentos tanto para sets quanto para filter. Exemplo: new { NewUsername = "xpto", Email = "xpto@xpto.com.br", OldEmail = "old@xpto.com.br" }.</param>
        /// <remarks>
        /// Atualizará todos os registros que corresponderem ao filtro.
        /// </remarks>
        void Update(string sets, string filter, object args);

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>
        void Delete(int id);

        /// <summary>
        /// Exclui as entidades que corresponderem ao filtro informado.
        /// </summary>
        /// <param name="filter">A cláusula WHERE definindo quais entidades serão excluídas. Exemplo: Name = @Name.</param>
        /// <param name="filterArgs">O objeto anônimo com os argumentos para o filtro. Exemplo: new { Name = "Test" }.</param>
        /// <remarks>
        /// Excluirá todos os registros que corresponderem ao filtro.
        /// </remarks>
        void Delete(string filter, object filterArgs);
        #endregion
    }
}
