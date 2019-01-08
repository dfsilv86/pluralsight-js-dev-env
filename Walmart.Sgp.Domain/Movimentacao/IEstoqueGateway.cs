using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um table data gateway para estoque.
    /// </summary>
    public interface IEstoqueGateway
    {
        /// <summary>
        /// Obtém a informação mais recente sobre custo de item do estoque das lojas.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="tipoPermissao">O tipo de permissão do usuário.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A informação mais recente de custos. (conforme Estoque.dtRecebimento)</returns>
        IEnumerable<Estoque> PesquisarUltimoCustoDoItemPorLoja(int idItemDetalhe, int? idLoja, int idUsuario, TipoPermissao tipoPermissao, Paging paging);

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
        /// Obtém a informação mais recente sobre custo de item da nota fiscal e do estoque em uma loja.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A informação mais recente de custos. (conforme NotaFiscal.dtRecebimento e Estoque.dtRecebimento)</returns>
        CustoMaisRecente ObterUltimoCustoDoItemNaLoja(int idItemDetalhe, int idLoja);

        /// <summary>
        /// Obtém o custo contábil mais recente de um item em uma loja.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>O custo contábil mais recente.</returns>
        decimal ObterUltimoCustoContabilItem(int idItemDetalhe, int idLoja);

        /// <summary>
        /// Realiza o ajuste do estoque manipulado.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajustado.</param>
        void AjusteEstoqueManipulado(Estoque estoque);

        /// <summary>
        /// Realiza o ajuste do estoque receituário.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajustado.</param>
        void AjustarEstoqueReceituario(Estoque estoque);

        /// <summary>
        /// Realiza o ajuste do estoque direto.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajustado.</param>
        void AjustarEstoqueDireto(Estoque estoque);

        /// <summary>
        /// Realiza o ajuste do estoque manipulado para MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">O estoque a ser ajustado.</param>
        void AjusteEstoqueManipulado(MovimentacaoMtr movimentacaoMtr);

        /// <summary>
        /// Realiza o ajuste do estoque receituário para MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">O estoque a ser ajustado.</param>
        void AjustarEstoqueReceituario(MovimentacaoMtr movimentacaoMtr);

        /// <summary>
        /// Realiza o ajuste do estoque direto para MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">O estoque a ser ajustado.</param>
        void AjustarEstoqueDireto(MovimentacaoMtr movimentacaoMtr);
    }
}
