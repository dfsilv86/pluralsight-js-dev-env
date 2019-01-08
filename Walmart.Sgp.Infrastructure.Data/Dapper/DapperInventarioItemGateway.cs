using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para inventario item utilizando o Dapper.
    /// </summary>
    public class DapperInventarioItemGateway : EntityDapperDataGatewayBase<InventarioItem>, IInventarioItemGateway
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperInventarioItemGateway"/>
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperInventarioItemGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "InventarioItem", "IDInventarioItem")
        {
        }

        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new[]
                {
                    "IDInventario", 
                    "IDItemDetalhe", 
                    "dhUltimaContagem",
                    "qtItem", 
                    "dhAlteracao", 
                    "cdUsuarioAlteracao", 
                    "dhRecalculo", 
                    "blJustificado",
                    "dsComentario", 
                    "qtItemInicial", 
                    "blAteradoGA", 
                    "QtdAuditada", 
                    "AcaoTomada", 
                    "AuditadoPor", 
                    "dtAuditoria", 
                    "qtdInventariadaAuditada", 
                    "vlTotalDifAuditado"
                };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtém os itens de inventário de acordo com o filtro especificado.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>
        /// Os itens que satisfazem o filtro.
        /// </returns>
        public IEnumerable<InventarioItemSumario> ObterEstruturadoPorFiltro(InventarioItemFiltro filtro, Paging paging)
        {
            return Resource.Query<InventarioItemSumario, ItemDetalhe, Estoque, InventarioItemSumario>(
                Sql.InventarioItem.ObterEstruturadoPorFiltro,
                filtro,
                MapearInventarioItem,
                "SplitOn1,SplitOn2")
                .AsPaging(paging);
        }

        /// <summary>
        /// Obtém um item de inventário estruturado pelo id.
        /// </summary>
        /// <param name="id">O id do item de inventário.</param>
        /// <returns>O item do inventário.</returns>
        public InventarioItem ObterEstruturadoPorId(int id)
        {
            return Resource.Query<InventarioItem, ItemDetalhe, InventarioItem>(
                Sql.InventarioItem.ObterEstruturadoPorId,
                new { id },
                MapearInventarioItem,
                "SplitOn1").FirstOrDefault();
        }

        /// <summary>
        /// Atualiza o item de inventario.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="inventario">O inventario que o item pertence.</param>
        /// <param name="alteradoGa">Indica se é GA.</param>
        public void Atualizar(InventarioItem item, Inventario inventario, bool alteradoGa)
        {
            StoredProcedure.Execute(
                "PR_InventarioItemOperacao",
                new
                {
                    inventario.IDInventario,
                    item.IDItemDetalhe,
                    Qtd = item.qtItem,
                    CdUsuarioAlteracao = RuntimeContext.Current.User.Id,
                    AlteradoGa = alteradoGa,
                    inventario.IDLoja,
                    inventario.dhInventario,
                    Justificativa = string.IsNullOrWhiteSpace(item.dsComentario) ? null : item.dsComentario,
                    Operacao = "A"
                });
        }

        /// <summary>
        /// Insere o item no inventario.
        /// </summary>
        /// <param name="item">O item.</param>        
        /// <param name="alteradoGa">Indica se é GA.</param>
        public void Inserir(InventarioItem item, bool alteradoGa)
        {
            StoredProcedure.Execute(
                "PR_InventarioItemOperacao",
                new
                {
                    item.IDInventario,
                    item.IDItemDetalhe,
                    Qtd = item.qtItem,
                    Justificativa = item.dsComentario,
                    CdUsuarioAlteracao = RuntimeContext.Current.User.Id,
                    AlteradoGa = alteradoGa,
                    Operacao = "I"
                });
        }

        /// <summary>
        /// Exclui o item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        public void Remover(InventarioItem item)
        {
            StoredProcedure.Execute(
               "PR_InventarioItemOperacao",
               new
               {
                   item.IDInventario,
                   item.IDItemDetalhe,
                   Qtd = item.qtItem,                   
                   CdUsuarioAlteracao = RuntimeContext.Current.User.Id,
                   AlteradoGa = RuntimeContext.Current.User.IsGa,
                   Operacao = "E"
               });
        }

        private static InventarioItem MapearInventarioItem(InventarioItem inventarioItem, ItemDetalhe itemDetalhe)
        {
            inventarioItem.ItemDetalhe = itemDetalhe;
            return inventarioItem;
        }

        private static InventarioItemSumario MapearInventarioItem(InventarioItemSumario inventarioItem, ItemDetalhe itemDetalhe, Estoque estoque)
        {
            inventarioItem.ItemDetalhe = itemDetalhe;
            if (itemDetalhe != null && inventarioItem.IDItemDetalhe.HasValue)
            {
                itemDetalhe.IDItemDetalhe = inventarioItem.IDItemDetalhe.Value;
            }

            inventarioItem.Estoque = estoque;
            return inventarioItem;
        }

        #endregion
    }
}