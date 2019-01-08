using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um movimentacao service.
    /// </summary>
    public interface IMovimentacaoService
    {
        /// <summary>
        /// Seleciona o extrato de movimentação de produto.
        /// </summary>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dataInicio">A data de inicio.</param>
        /// <param name="dataFim">A data de fim.</param>
        /// <param name="idItemDetalhe">O id de item.</param>
        /// <param name="tipoMovimento">O tipo de movimento.</param>
        /// <param name="idInventario">O id de inventario.</param>
        /// <remarks>
        ///  O parâmetro 'tipoMovimento' serve para trazer de forma separada os tipos de movimentações que são ajuste de inventário (I)
        ///  e o que são movimentações normais (N) (Venda, Quebra, etc.).
        ///  Se for necessário trazer todos os tipos de movimentações, passe esse parâmetro com o valor branco.
        /// </remarks>
        /// <returns>Os items do extato.</returns>
        IEnumerable<ItemExtrato> RelExtratoProdutoMovimentacao(int idLoja, DateTime dataInicio, DateTime dataFim, long idItemDetalhe, string tipoMovimento, int? idInventario);

        /// <summary>
        /// Obtém a movimentação pelo id.
        /// </summary>
        /// <param name="idMovimentacao">O id da movimentação.</param>
        /// <returns>A movimentação.</returns>
        Movimentacao ObterEstruturadoPorId(int idMovimentacao);

        /// <summary>
        /// Obtém as datas de quebra para o item na loja informada.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item desejado.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>As data de quebra.</returns>
        IEnumerable<DateTime> ObterDatasDeQuebra(int idItemDetalhe, int idLoja);
    }
}