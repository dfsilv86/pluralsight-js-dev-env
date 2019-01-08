using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para movimentacao data utilizando o Dapper.
    /// </summary>
    public class DapperMovimentacaoGateway : DapperDataGatewayBase<MovimentacaoService>, IMovimentacaoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperMovimentacaoGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperMovimentacaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Seleciona o extrato de movimentação de produto.
        /// </summary>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dataInicio">A data de inicio.</param>
        /// <param name="dataFim">A data de fim.</param>
        /// <param name="idItemDetalhe">O id de item.</param>
        /// <param name="tipoMovimento">O tipo de movimento.</param>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>Os items do extato.</returns>
        public IEnumerable<ItemExtrato> SelExtratoProdutoMovimentacao(int idLoja, DateTime dataInicio, DateTime dataFim, long idItemDetalhe, string tipoMovimento, int? idInventario)
        {
            var args = new
            {
                IDLOJA = idLoja,
                DTINI = dataInicio,
                DTFIM = dataFim,
                IDITEM = idItemDetalhe,
                TIPOMOVIMENTO = tipoMovimento,
                IdInventario = idInventario
            };

            return StoredProcedure.Query<ItemExtrato>("PR_SelExtratoProdutoMovimentacao", args);
        }

        /// <summary>
        /// Obtém a movimentação pelo id.
        /// </summary>
        /// <param name="idMovimentacao">O id da movimentação.</param>
        /// <returns>A movimentação.</returns> 
        public Movimentacao ObterEstruturadoPorId(int idMovimentacao)
        {
            Movimentacao result = null;

            this.Resource.Query<Movimentacao, ItemDetalhe, Departamento, ItemDetalhe, Departamento, Movimentacao>(
                Sql.Movimentacao.ObterEstruturadoPorId,
                new { idMovimentacao },
                (movimentacao, item, itemDepartamento, itemTransf, itemTransfDepartamento) =>
                {
                    if (result == null)
                    {
                        result = movimentacao;

                        item.IDDepartamento = itemDepartamento.IDDepartamento;
                        item.Departamento = itemDepartamento;
                        
                        result.IDItem = item.IDItemDetalhe;
                        result.Item = item;

                        itemTransf.IDDepartamento = itemTransfDepartamento.IDDepartamento;
                        itemTransf.Departamento = itemTransfDepartamento;

                        result.IdItemTransferencia = itemTransf.IDItemDetalhe;
                        result.ItemTransferencia = itemTransf;
                    }

                    return result;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4").Perform();

            return result;
        }

        /// <summary>
        /// Obtém as datas de quebra para o item na loja informada.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item desejado.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>As data de quebra.</returns>
        public IEnumerable<DateTime> ObterDatasDeQuebra(int idItemDetalhe, int idLoja)
        {
            return Resource.Query<DateTime>(Sql.Movimentacao.ObterDatasDeQuebra, new { idItemDetalhe, idLoja });
        }
        #endregion
    }
}
