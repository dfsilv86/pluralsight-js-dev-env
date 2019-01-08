using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para ReturnSheetItemLoja.
    /// </summary>
    public interface IReturnSheetItemLojaGateway : IDataGateway<ReturnSheetItemLoja>
    {        
        /// <summary>
        /// Obter uma lista de RSIL por RSIP
        /// </summary>
        /// <param name="idReturnSheetItemPrincipal">O id.</param>
        /// <returns>Lista de RSIL</returns>
        IEnumerable<ReturnSheetItemLoja> ObterPorIdReturnSheetItemPrincipal(int idReturnSheetItemPrincipal);

        /// <summary>
        /// Obtém lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="dsEstado">UF para flitrar.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As lojas válidas para associação com item da ReturnSheet.</returns>
        IEnumerable<ReturnSheetItemLoja> ObterLojasValidasItem(int cdItem, int cdSistema, int idReturnSheet, string dsEstado, Paging paging);

        /// <summary>
        /// Obtém a lista de lojas vinculada a uma return sheet e o item detalhe de saída.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="idItemDetalheSaida">O identificador do item detalhe de saída.</param>
        /// <returns>Retorna a lista de lojas vinculada a uma return sheet e o item detalhe de saída.</returns>
        IEnumerable<ReturnSheetItemLoja> ObterLojasPorReturnSheetEItemDetalheSaida(int idReturnSheet, long idItemDetalheSaida);

        /// <summary>
        /// Obtém estados que possuem lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna estados que possuem lojas válidas para associação com o item da ReturnSheet.</returns>
        IEnumerable<string> ObterEstadosLojasValidasItem(int cdItem, int cdSistema);
    }
}