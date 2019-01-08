using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um serviço de manutenção de estoque.
    /// </summary>
    public interface IEstoqueService
    {
        /// <summary>
        /// Obtém a informação mais recente sobre custo de item do estoque das lojas.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A informação mais recente de custos. (conforme Estoque.dtRecebimento)</returns>
        IEnumerable<Estoque> PesquisarUltimoCustoDoItemPorLoja(int idItemDetalhe, int? idLoja, Paging paging);

        /// <summary>
        /// Obtém a lista de ids de item detalhe que são itens de entrada de um determinado item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>A lista de ids de itens de entrada.</returns>
        /// <remarks>Consulta recursiva no banco, está com OPTION MAXRECURSION 10 no momento.</remarks>
        IEnumerable<int> ObterOrigemItem(int idItemDetalhe);

        /// <summary>
        /// Obtém os últimos 5 recebimentos do item ou de suas entradas.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 recebimentos.</returns>
        /// <remarks>Como a consulta utiliza também os itens de entrada do item informado, é possível que uma NotaFiscal possua mais de um NotaFiscalItem. O conjunto de todos NotaFiscalItem deve ter os 5 itens.</remarks>
        IEnumerable<NotaFiscal> ObterOsCincoUltimosRecebimentosDoItemPorLoja(int idItemDetalhe, int idLoja);

        /// <summary>
        /// Obtém os últimos 5 custo do item na loja
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 custos.</returns>
        IEnumerable<CustoMaisRecente> ObterOsCincoUltimosCustosDoItemPorLoja(int cdItem, int idLoja);

        /// <summary>
        /// Obtém os custos mais recentes de itens relacionados a um item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>Os custos.</returns>
        IEnumerable<CustoItemRelacionadoResponse> ObterUltimoCustoDeItensRelacionadosNaLoja(int idItemDetalhe, int idLoja);

        /// <summary>
        /// Obtém o custo contábil mais recente de um item em uma loja.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>O custo contábil mais recente.</returns>
        decimal ObterUltimoCustoContabilItem(int idItemDetalhe, int idLoja);

        /// <summary>
        /// Realiza o ajuste do estoque.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajudado.</param>
        void Ajustar(Estoque estoque);

        /// <summary>
        /// Realiza a movimentação do tipo MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">A movimentação do tipo MTR.</param>
        void RealizarMtr(MovimentacaoMtr movimentacaoMtr);
    }
}
