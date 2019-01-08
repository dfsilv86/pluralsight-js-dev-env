using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para ReturnSheetItemPrincipal.
    /// </summary>
    public interface IReturnSheetItemPrincipalGateway : IDataGateway<ReturnSheetItemPrincipal>
    {
        /// <summary>
        /// Pesquisa ReturnSheetItemPrincipal por um IdReturnSheet.
        /// </summary>
        /// <param name="idReturnSheet">O ID do return sheet.</param>
        /// <param name="paging">O controle de paginação.</param>
        /// <returns>Uma lista de ReturnSheetItemPrincipal.</returns>
        IEnumerable<ReturnSheetItemPrincipal> PesquisarPorIdReturnSheet(int idReturnSheet, Paging paging);

        /// <summary>
        /// Obtem uma lista de ReturnSheetItemPrincipal por IdReturnSheet
        /// </summary>
        /// <param name="idReturnSheet">O IdReturnSheet</param>
        /// <returns>Uma lista de ReturnSheetItemPrincipal.</returns>
        IEnumerable<ReturnSheetItemPrincipal> ObterPorIdReturnSheet(int idReturnSheet);

        /// <summary>
        /// Insere (caso necessário) e retornar o identificador.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="idItemDetalhe">O identificador do item detalhe de saída.</param>
        /// <returns>Retorna o ReturnSheetItemPrincipal.</returns>
        ReturnSheetItemPrincipal Insert(int idReturnSheet, long idItemDetalhe);

        /// <summary>
        /// Obtém o identificador do ReturnSheetItemPrincipal.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="cdItem">O código do item detalhe de saída.</param>
        /// <returns>Retorna o identificador do ReturnSheetItemPrincipal.</returns>
        int ObterPorReturnSheetEItemDetalheSaida(int idReturnSheet, int cdItem);
    }
}