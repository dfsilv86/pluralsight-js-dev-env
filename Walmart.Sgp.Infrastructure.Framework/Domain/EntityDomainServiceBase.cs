using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para serviços de domínio que utilizam uma entidade primária.
    /// </summary>
    /// <typeparam name="TEntity">A entidade que o serviço trabalha primariamente.</typeparam>
    /// <typeparam name="TMainDataGateway">O gateway de acesso a dados principal.</typeparam>
    public abstract class EntityDomainServiceBase<TEntity, TMainDataGateway>
        : DomainServiceBase<TMainDataGateway>, IDomainService<TEntity>
        where TEntity : IEntity, IAggregateRoot
        where TMainDataGateway : IDataGateway<TEntity>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EntityDomainServiceBase{TEntity, TMainDataGateway}"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        protected EntityDomainServiceBase(TMainDataGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém todos as entidades.
        /// </summary>
        /// <returns>As entidades.</returns>
        public virtual IEnumerable<TEntity> ObterTodos()
        {
            return MainGateway.FindAll();
        }

        /// <summary>
        /// Obtém todos as entidades definidas pela paginação.
        /// </summary>
        /// <param name="paging">A paginação a ser utilizada.</param>
        /// <returns>As entidades.</returns>
        public virtual IEnumerable<TEntity> ObterTodos(Paging paging)
        {
            return MainGateway.FindAll(paging);
        }

        /// <summary>
        /// Obtém a entidade pelo id informado.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A instância da entidade.</returns>
        public virtual TEntity ObterPorId(int id)
        {
            return MainGateway.FindById(id);
        }

        /// <summary>
        /// Obtém as entidades pelo id.
        /// </summary>
        /// <param name="ids">Os ids das entidades desejadas.</param>
        /// <returns>As entidades.</returns>
        public virtual IEnumerable<TEntity> ObterPorIds(params int[] ids)
        {
            return MainGateway.FindByIds(ids);
        }

        /// <summary>
        /// Remove a entidade com o id informado.
        /// </summary>
        /// <param name="id">O id da entidade a ser removida.</param>
        public virtual void Remover(int id)
        {
            MainGateway.Delete(id);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public virtual void Salvar(TEntity entidade)
        {
            Carimbar(entidade);

            if (entidade.IsNew)
            {
                MainGateway.Insert(entidade);
            }
            else
            {
                MainGateway.Update(entidade);
            }
        }

        /// <summary>
        /// Carimba a entidade se a mesma implementar IStampContainer.
        /// </summary>
        /// <param name="entidade">A entidade</param>
        private static void Carimbar(TEntity entidade)
        {
            var carimbo = entidade as IStampContainer;

            if (carimbo != null)
            {
                carimbo.Stamp();
            }
        }
        #endregion
    }
}
