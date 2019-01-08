using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de ReturnSheetItemLoja.
    /// </summary>
    public interface IReturnSheetItemLojaService : IDomainService<ReturnSheetItemLoja>
    {
        /// <summary>
        /// Obtem uma lista de ReturnSheetItemLoja por idReturnSheetItemPrincipal
        /// </summary>
        /// <param name="idReturnSheetItemPrincipal">O idReturnSheetItemPrincipal.</param>
        /// <returns>Uma lista de ReturnSheetItemLoja.</returns>
        IEnumerable<ReturnSheetItemLoja> ObterPorIdReturnSheetItemPrincipal(int idReturnSheetItemPrincipal);

        /// <summary>
        /// Obtém lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="dsEstado">UF para filtrar.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As lojas válidas para associação com item da ReturnSheet.</returns>
        IEnumerable<ReturnSheetItemLoja> ObterLojasValidasItem(int cdItem, int cdSistema, int idReturnSheet, string dsEstado, Paging paging);

        /// <summary>
        /// Obtém estados que possuem lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna estados que possuem lojas válidas para associação com o item da ReturnSheet.</returns>
        IEnumerable<string> ObterEstadosLojasValidasItem(int cdItem, int cdSistema);
    }
}