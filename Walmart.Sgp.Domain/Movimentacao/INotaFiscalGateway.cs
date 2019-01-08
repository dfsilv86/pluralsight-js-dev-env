using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um table data gateway para nota fiscal.
    /// </summary>
    public interface INotaFiscalGateway
    {
        /// <summary>
        /// Obtém uma nota fiscal pelo seu id e retorna a nota fiscal com informações das entidades associadas.
        /// </summary>
        /// <param name="idNotaFiscal">O id de nota fiscal.</param>
        /// <returns>A NotaFiscal com informações de Loja, Fornecedor e Bandeira.</returns>
        NotaFiscal ObterEstruturadoPorId(int idNotaFiscal);

        /// <summary>
        /// Pesquisa detalhe de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        IEnumerable<NotaFiscal> PesquisarPorFiltros(NotaFiscalFiltro filtro, Paging paging);

        /// <summary>
        /// Pesquisa ultimas entradas de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="idItemDetalhe">O ID do item</param>
        /// <param name="idLoja">O ID da loja</param>        
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As entradas</returns>
        IEnumerable<NotaFiscalConsolidado> PesquisarUltimasEntradasPorFiltro(long idItemDetalhe, int idLoja, DateTime dtSolicitacao, Paging paging);

        /// <summary>
        /// Pesquisa os custos do item.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja</param>
        /// <param name="cdItem">O codigo do item</param>
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <returns>Os custos do item.</returns>
        NotaFiscalItemCustosConsolidado ObterCustosPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao);

        /// <summary>
        /// Verifica se existe notas pendentes para o item informado
        /// </summary>
        /// <param name="cdLoja">O codigo da loja</param>
        /// <param name="cdItem">O codigo do item</param>
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <returns>Verdadeiro ou falso</returns>
        bool ExisteNotasPendentesPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao);

        /// <summary>
        /// Pesquisa de custos de notas fiscais pelos filtros informados
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens</returns>
        IEnumerable<CustoNotaFiscal> PesquisarCustosPorFiltros(NotaFiscalFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém o último item de nota fiscal recebido na loja para o item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>O item na última nota fiscal da loja.</returns>
        NotaFiscalItem ObterItemNaUltimaNotaRecebidaDaLoja(int idItemDetalhe, int idLoja);

        /// <summary>
        /// Obtém os itens da nota fiscal com o id informado.
        /// </summary>
        /// <param name="idNotaFiscal">O id da nota fiscal.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens da nota fiscal.</returns>
        IEnumerable<NotaFiscalItem> ObterItensDaNotaFiscal(int idNotaFiscal, Paging paging);
    }
}
