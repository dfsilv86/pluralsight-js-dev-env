using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface básica de um serviço de domínio.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDomainService<TEntity>
        where TEntity : IAggregateRoot
    {
        /// <summary>
        /// Obtém todos as entidades.
        /// </summary>
        /// <returns>As entidades.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<TEntity> ObterTodos();

        /// <summary>
        /// Obtém todos as entidades definidas pela paginação.
        /// </summary>
        /// <param name="paging">A paginação a ser utilizada.</param>
        /// <returns>As entidades.</returns>
        IEnumerable<TEntity> ObterTodos(Paging paging);

        /// <summary>
        /// Obtém a entidade pelo id informado.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A instância da entidade.</returns>
        TEntity ObterPorId(int id);

        /// <summary>
        /// Obtém as entidades pelo id.
        /// </summary>
        /// <param name="ids">Os ids das entidades desejadas.</param>
        /// <returns>As entidades.</returns>
        IEnumerable<TEntity> ObterPorIds(params int[] ids);

        /// <summary>
        /// Remove a entidade com o id informado.
        /// </summary>
        /// <param name="id">O id da entidade a ser removida.</param>
        void Remover(int id);

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        void Salvar(TEntity entidade);
    }
}
