using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um table data gateway para inventario item.
    /// </summary>
    public interface IInventarioItemGateway : IDataGateway<InventarioItem>
    {
        /// <summary>
        /// Obtém os itens de inventário de acordo com o filtro especificado.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens que satisfazem o filtro.</returns>
        IEnumerable<InventarioItemSumario> ObterEstruturadoPorFiltro(InventarioItemFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém um item de inventário estruturado pelo id.
        /// </summary>
        /// <param name="id">O id do item de inventário.</param>
        /// <returns>O item do inventário.</returns>
        InventarioItem ObterEstruturadoPorId(int id);

        /// <summary>
        /// Atualiza o item de inventario.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="inventario">O inventario que o item pertence.</param>
        /// <param name="alteradoGa">Indica se é GA.</param>
        void Atualizar(InventarioItem item, Inventario inventario, bool alteradoGa);

        /// <summary>
        /// Insere o item no inventario.
        /// </summary>
        /// <param name="item">O item.</param>        
        /// <param name="alteradoGa">Indica se é GA.</param>
        void Inserir(InventarioItem item, bool alteradoGa);

        /// <summary>
        /// Exclui o item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        void Remover(InventarioItem item);
    }
}