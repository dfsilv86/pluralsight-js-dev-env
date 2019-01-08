using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para ReturnSheetItemLoja utilizando o Dapper.
    /// </summary>
    public class DapperReturnSheetItemLojaGateway : EntityDapperDataGatewayBase<ReturnSheetItemLoja>, IReturnSheetItemLojaGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperReturnSheetItemLojaGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperReturnSheetItemLojaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ReturnSheetItemLoja", "IdReturnSheetItemLoja")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IdReturnSheetItemPrincipal", "IdItemDetalhe", "IdLoja", "PrecoVenda", "blAtivo" };
            }
        }

        /// <summary>
        /// Obter uma lista de RSIL por RSIP
        /// </summary>
        /// <param name="idReturnSheetItemPrincipal">O id.</param>
        /// <returns>Lista de RSIL</returns>
        public IEnumerable<ReturnSheetItemLoja> ObterPorIdReturnSheetItemPrincipal(int idReturnSheetItemPrincipal)
        {
            return this.Find("IdReturnSheetItemPrincipal = @IdReturnSheetItemPrincipal", new { IdReturnSheetItemPrincipal = idReturnSheetItemPrincipal });
        }

        /// <summary>
        /// Obtém lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="dsEstado">O UF para filtrar.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As lojas válidas para associação com item da ReturnSheet.</returns>
        public IEnumerable<ReturnSheetItemLoja> ObterLojasValidasItem(int cdItem, int cdSistema, int idReturnSheet, string dsEstado, Paging paging)
        {
            var args = new
            {
                cdItem,
                cdSistema,
                idReturnSheet,
                uf = dsEstado
            };

            return this.Resource.Query<ReturnSheetItemLoja>(Sql.ReturnSheetItemLoja.ObterLojasValidasItem, args)
                .AsPaging(paging);
        }

        /// <summary>
        /// Obtém a lista de lojas vinculada a uma return sheet e o item detalhe de saída.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="idItemDetalheSaida">O identificador do item detalhe de saída.</param>
        /// <returns>Retorna a lista de lojas vinculada a uma return sheet e o item detalhe de saída.</returns>
        public IEnumerable<ReturnSheetItemLoja> ObterLojasPorReturnSheetEItemDetalheSaida(int idReturnSheet, long idItemDetalheSaida)
        {
            var args = new
            {
                idReturnSheet,
                idItemDetalheSaida
            };

            return this.Resource.Query<ReturnSheetItemLoja>(Sql.ReturnSheetItemLoja.ObterLojasPorReturnSheetEItemDetalheSaida, args)
                .ToList();
        }

        /// <summary>
        /// Obtém estados que possuem lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna estados que possuem lojas válidas para associação com o item da ReturnSheet.</returns>
        public IEnumerable<string> ObterEstadosLojasValidasItem(int cdItem, int cdSistema)
        {
            return Resource.Query<string>(Sql.ReturnSheetItemLoja.ObterEstadosLojasValidasItem, new { cdItem, cdSistema }).ToList();
        }
    }
}
