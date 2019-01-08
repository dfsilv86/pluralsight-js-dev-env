using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Classe utilitária para auxiliar no trabalho com entidades filhas de um EntityDapperDataGatewayBase de IAggregateRoot.
    /// </summary>
    public static class ChildrenHelper
    {
        #region Methods

        /// <summary>
        /// Insere a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="itemsToInsert">A lista de entidades filhas que devem ser salvas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="setFK">A func para realizar a setagem da FK do pai na filha.</param>
        public static void Insert<TChildEntity>(IEnumerable<TChildEntity> itemsToInsert, IDataGateway<TChildEntity> gateway, Action<TChildEntity> setFK)
          where TChildEntity : IEntity
        {
            foreach (var b in itemsToInsert)
            {
                setFK(b);
                gateway.Insert(b);
            }
        }

        /// <summary>
        /// Insere a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="itemsToInsert">A lista de entidades filhas que devem ser salvas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="setFK">A func para realizar a setagem da FK do pai na filha.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        public static void Insert<TChildEntity>(IEnumerable<TChildEntity> itemsToInsert, IDataGateway<TChildEntity> gateway, Action<TChildEntity> setFK, IAuditStrategy auditStrategy)
          where TChildEntity : IEntity
        {
            foreach (var b in itemsToInsert)
            {
                setFK(b);
                gateway.Insert(b);
                auditStrategy.DidInsert(b);
            }
        }

        /// <summary>
        /// Atualiza a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <remarks>
        /// Será realizada uma sincronia entre os filhos exitentes no banco e os que serão salvos, fazendo insert, update e delete conforme necessário.
        /// </remarks>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="oldList">A lista de entidades filhas atualmente salvas no banco.</param>
        /// <param name="newList">A lista de entidades filhas que devem ser salvas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="setFK">A func para realizar a setagem da FK do pai na filha.</param>
        public static void Sync<TChildEntity>(IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList, IDataGateway<TChildEntity> gateway, Action<TChildEntity> setFK)
            where TChildEntity : IEntity
        {
            var itemsToInsert = GetItemsToInsert(oldList, newList);
            var itemsToUpdate = GetItemsToUpdate(oldList, newList);
            var itemsToDelete = GetItemsToDelete(oldList, newList);

            Delete(itemsToDelete, gateway);
            Update(itemsToUpdate, gateway);
            Insert(itemsToInsert, gateway, setFK);
        }

        /// <summary>
        /// Atualiza a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <remarks>
        /// Será realizada uma sincronia entre os filhos exitentes no banco e os que serão salvos, fazendo insert, update e delete conforme necessário.
        /// </remarks>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="oldList">A lista de entidades filhas atualmente salvas no banco.</param>
        /// <param name="newList">A lista de entidades filhas que devem ser salvas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="setFK">A func para realizar a setagem da FK do pai na filha.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        public static void Sync<TChildEntity>(IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList, IDataGateway<TChildEntity> gateway, Action<TChildEntity> setFK, IAuditStrategy auditStrategy)
            where TChildEntity : IEntity
        {
            var itemsToInsert = GetItemsToInsert(oldList, newList);
            var itemsToUpdate = GetItemsToUpdate(oldList, newList);
            var itemsToDelete = GetItemsToDelete(oldList, newList);

            Delete(itemsToDelete, gateway, auditStrategy);
            Update(itemsToUpdate, gateway, auditStrategy);
            Insert(itemsToInsert, gateway, setFK, auditStrategy);
        }

        /// <summary>
        /// Atualiza a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <remarks>
        /// Será realizada uma sincronia entre os filhos exitentes no banco e os que serão salvos, fazendo insert e delete conforme necessário.
        /// </remarks>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="oldList">A lista de entidades filhas atualmente salvas no banco.</param>
        /// <param name="newList">A lista de entidades filhas que devem ser salvas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="setFK">A func para realizar a setagem da FK do pai na filha.</param>
        public static void SyncNoUpdate<TChildEntity>(IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList, IDataGateway<TChildEntity> gateway, Action<TChildEntity> setFK)
            where TChildEntity : IEntity
        {
            var itemsToInsert = GetItemsToInsert(oldList, newList);
            var itemsToDelete = GetItemsToDelete(oldList, newList);

            Delete(itemsToDelete, gateway);
            Insert(itemsToInsert, gateway, setFK);
        }

        /// <summary>
        /// Atualiza a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <remarks>
        /// Será realizada uma sincronia entre os filhos exitentes no banco e os que serão salvos, fazendo insert, update e delete conforme necessário.
        /// </remarks>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="updateSets">A cláusula SET definindo quais propriedades serão atualizadas. Exemplo: Username = @NewUsername, Email = @Email.</param>
        /// <param name="oldList">A lista de entidades filhas atualmente salvas no banco.</param>
        /// <param name="newList">A lista de entidades filhas que devem ser salvas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="setFK">A func para realizar a setagem da FK do pai na filha.</param>
        public static void Sync<TChildEntity>(string updateSets, IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList, IDataGateway<TChildEntity> gateway, Action<TChildEntity> setFK)
            where TChildEntity : IEntity
        {
            var itemsToDelete = GetItemsToDelete(oldList, newList);
            Delete(itemsToDelete, gateway);

            var itemsToUpdate = GetItemsToUpdate(oldList, newList);
            Update(updateSets, itemsToUpdate, gateway);

            var itemsToInsert = GetItemsToInsert(oldList, newList);
            Insert(itemsToInsert, gateway, setFK);
        }

        /// <summary>
        /// Exclui a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="itemsToDelete">A lista de entidades filhas que devem ser excluídas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        public static void Delete<TChildEntity>(IEnumerable<TChildEntity> itemsToDelete, IDataGateway<TChildEntity> gateway)
           where TChildEntity : IEntity
        {
            foreach (var b in itemsToDelete)
            {
                gateway.Delete(b.Id);
            }
        }

        /// <summary>
        /// Exclui a lista de filhos utilizando o gateway informado.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade filha.</typeparam>
        /// <param name="itemsToDelete">A lista de entidades filhas que devem ser excluídas no banco.</param>
        /// <param name="gateway">O gateway das entidades filhas.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        public static void Delete<TChildEntity>(IEnumerable<TChildEntity> itemsToDelete, IDataGateway<TChildEntity> gateway, IAuditStrategy auditStrategy)
           where TChildEntity : IEntity
        {
            foreach (var b in itemsToDelete)
            {
                auditStrategy.WillDelete(b);
                gateway.Delete(b.Id);
            }
        }

        /// <summary>
        /// Obtém os itens que devem ser inseridos ao comparar a lista de itens antigos e a lista de novos itens.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade.</typeparam>
        /// <param name="oldList">A lista de itens antigos.</param>
        /// <param name="newList">A lista com os novos itens.</param>
        /// <returns>Os itens a serem inseridos.</returns>
        public static IEnumerable<TChildEntity> GetItemsToInsert<TChildEntity>(IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList)
            where TChildEntity : IEntity
        {
            return newList.Where(b => !oldList.Contains(b)).ToList();
        }

        /// <summary>
        /// Obtém os itens que devem ser atualizados ao comparar a lista de itens antigos e a lista de novos itens.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade.</typeparam>
        /// <param name="oldList">A lista de itens antigos.</param>
        /// <param name="newList">A lista com os novos itens.</param>
        /// <returns>Os itens a serem atualizados.</returns>
        public static IEnumerable<TChildEntity> GetItemsToUpdate<TChildEntity>(IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList)
            where TChildEntity : IEntity
        {
            return newList.Where(b => oldList.Contains(b)).ToList();
        }

        /// <summary>
        /// Obtém os itens que devem ser excluídos ao comparar a lista de itens antigos e a lista de novos itens.
        /// </summary>
        /// <typeparam name="TChildEntity">O tipo da entidade.</typeparam>
        /// <param name="oldList">A lista de itens antigos.</param>
        /// <param name="newList">A lista com os novos itens.</param>
        /// <returns>Os itens a serem excluídos.</returns>
        public static IEnumerable<TChildEntity> GetItemsToDelete<TChildEntity>(IEnumerable<TChildEntity> oldList, IEnumerable<TChildEntity> newList)
            where TChildEntity : IEntity
        {
            return oldList.Where(b => !newList.Contains(b)).ToList();
        }
        #endregion

        #region Helpers
        private static void Update<TChildEntity>(IEnumerable<TChildEntity> itemsToUpdate, IDataGateway<TChildEntity> gateway)
            where TChildEntity : IEntity
        {
            foreach (var b in itemsToUpdate)
            {
                gateway.Update(b);
            }
        }

        private static void Update<TChildEntity>(IEnumerable<TChildEntity> itemsToUpdate, IDataGateway<TChildEntity> gateway, IAuditStrategy auditStrategy)
            where TChildEntity : IEntity
        {
            foreach (var b in itemsToUpdate)
            {
                gateway.Update(b);
                auditStrategy.DidUpdate(b);
            }
        }

        private static void Update<TChildEntity>(string sets, IEnumerable<TChildEntity> itemsToUpdate, IDataGateway<TChildEntity> gateway)
            where TChildEntity : IEntity
        {
            foreach (var b in itemsToUpdate)
            {
                gateway.Update(sets, b);
            }
        }
        #endregion
    }
}
