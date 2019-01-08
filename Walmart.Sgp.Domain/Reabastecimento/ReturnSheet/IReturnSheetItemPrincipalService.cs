using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de ReturnSheetItemPrincipal.
    /// </summary>
    public interface IReturnSheetItemPrincipalService : IDomainService<ReturnSheetItemPrincipal>
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
        /// Realiza o vínculo entre as lojas e os itens de uma ReturnSheet.
        /// </summary>
        /// <param name="lojasAlteradas">A lista de lojas que foram alteradas.</param>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="precoVenda">O preço de venda a ser aplicado para todas as lojas.</param>
        void SalvarLojas(IEnumerable<ReturnSheetItemLoja> lojasAlteradas, int idReturnSheet, decimal? precoVenda);

        /// <summary>
        /// Exclusão lógica de ReturnSheetItemPrincipal.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="cdItem">O código do item detalhe de saída.</param>
        void Remover(int idReturnSheet, int cdItem);
    }
}