using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para sugestao pedido.
    /// </summary>
    public interface ISugestaoPedidoGateway : IDataGateway<SugestaoPedido>
    {
        /// <summary>
        /// Pesquisa sugestões de pedidos pelos filtros informados.
        /// </summary>
        /// <param name="request">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As sugestões de pedido.</returns>
        IEnumerable<SugestaoPedidoModel> PesquisarPorFiltros(SugestaoPedidoFiltro request, Paging paging);

        /// <summary>
        /// Obtém a lista de sugestões de pedido de um fornecedor em uma loja.
        /// </summary>
        /// <param name="idFornecedorParametro">O id do parâmetro de fornecedor.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>As sugestões de pedido.</returns>
        IEnumerable<SugestaoPedidoModel> PesquisarPorFornecedorParametroELoja(long idFornecedorParametro, int idLoja);

        /// <summary>
        /// Consolida o pedido mínimo do fornecedor.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idFornecedorParametro">O id do parâmetro do fornecedor.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="blAdicionaPack">Se adiciona packs.</param>
        /// <remarks>blAdicionaPack vem da informação se o usuário atual é administrador.</remarks>
        void ConsolidarPedidoMinimo(int idLoja, long idFornecedorParametro, DateTime dtPedido, bool blAdicionaPack);

        /// <summary>
        /// Consolida o pedido mínimo do fornecedor para itens XDOC.
        /// </summary>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="idFornecedorParametro">O id do parâmetro do fornecedor.</param>
        void ConsolidarPedidoMinimoXDoc(DateTime dtPedido, int idCD, long idFornecedorParametro);

        /// <summary>
        /// Obtém uma sugestão pedido com dados do item detalhe do pedido e da loja.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestao pedido.</param>
        /// <returns>A sugestão pedido.</returns>
        /// <remarks>Usado pelo processo de alterar sugestão pedido.</remarks>
        SugestaoPedido ObterEstruturado(int idSugestaoPedido);

        /// <summary>
        /// Conta quantas sugestões de pedido existem para os filtros informados.
        /// </summary>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <returns>O número de registros.</returns>
        int ContarPorDataPedidoLojaEDepartamento(DateTime dtPedido, int cdSistema, int idLoja, int idDepartamento);

        /// <summary>
        /// Verifica se existe grade aberta para a data e item informado.
        /// </summary>
        /// <param name="idItemDetalheSaida">O item a pesquisar.</param>
        /// <param name="idFornecedorParametro">O id fornecedor parametro.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Se existe ou não grade aberta.</returns>
        bool VerificaItemSaidaGradeAberta(int idItemDetalheSaida, int idFornecedorParametro, int cdSistema, DateTime dtPedido);
    }
}
