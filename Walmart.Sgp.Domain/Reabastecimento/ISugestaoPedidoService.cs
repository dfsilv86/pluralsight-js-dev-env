using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um serviço de sugestao pedido.
    /// </summary>
    public interface ISugestaoPedidoService : IDomainService<SugestaoPedido>
    {
        /// <summary>
        /// Pesquisa sugestões de pedidos pelos filtros informados.
        /// </summary>
        /// <param name="request">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>
        /// As sugestões de pedido.
        /// </returns>
        IEnumerable<SugestaoPedidoModel> PesquisarPorFiltros(SugestaoPedidoFiltro request, Paging paging);

        /// <summary>
        /// Obtém uma sugestão pedido com dados do item detalhe do pedido e da loja.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestao pedido.</param>
        /// <returns>A sugestão pedido.</returns>
        /// <remarks>Usado pelo processo de alterar sugestão pedido.</remarks>
        SugestaoPedido ObterEstruturado(int idSugestaoPedido);

        /// <summary>
        /// Salva uma lista de sugestões de pedido.
        /// </summary>
        /// <param name="valores">As sugestões.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Resultado da operação.</returns>
        AlterarSugestoesResponse AlterarSugestoes(IEnumerable<SugestaoPedidoModel> valores, int idLoja, DateTime dtPedido);

        /*
        /// <summary>
        /// Altera uma sugestão de pedido.
        /// </summary>
        /// <param name="valores">A sugestão.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dtPedido">The dt pedido.</param>
        /// <returns>O resultado da operação.</returns>
        AlterarSugestaoResponse AlterarSugestao(SugestaoPedidoModel valores, int idDepartamento, int idLoja, DateTime dtPedido);
         * */

        /// <summary>
        /// Valida uma alteração de uma sugestão de pedido.
        /// </summary>
        /// <param name="alteracao">Os dados alterados da sugestão de pedido (vlEstoque e qtdPackCompra).</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>O resultado da validação.</returns>
        SpecResult ValidarAlteracaoSugestao(SugestaoPedidoModel alteracao, int idLoja, int idDepartamento, DateTime dtPedido);

        /// <summary>
        /// Verifica se o usuário pode autorizar pedido.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O status (L - a loja não permite, D - desabilitado, A - autorizado, P - pendente).</returns>
        /// <remarks>TODO: fixed value</remarks>
        string ObterStatusAutorizarPedido(DateTime dtPedido, int idLoja, int idDepartamento, int cdSistema);

        /// <summary>
        /// Autoriza o pedido.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O status resultante (L - a loja não permite, D - desabilitado, A - autorizado, P - pendente).</returns>
        /// <remarks>TODO: fixed value</remarks>
        string AutorizarPedido(DateTime dtPedido, int idLoja, int idDepartamento, int cdSistema);

        /// <summary>
        /// Obtém informações de sugestão pedido com informações de alçada relacionadas.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestão pedido.</param>
        /// <returns>A sugestão de pedido.</returns>
        SugestaoPedidoModel ObterEstruturadoComAlcada(int idSugestaoPedido);

        /// <summary>
        /// Obtém os logs de auditoria de sugestão pedido.
        /// </summary>
        /// <param name="filter">O filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os logs de auditoria.</returns>
        IEnumerable<AuditRecord<SugestaoPedido>> ObterLogs(AuditFilter filter, Paging paging);   

        /// <summary>
        /// Obtém a quantidade de sugestões de pedidos para a data informada.
        /// </summary>
        /// <param name="dia">A data das sugestões de pedidos.</param>
        /// <returns>A quantidade de sugestões pedidos.</returns>
        QuantidadeSugestaoPedido ObterQuantidade(DateTime dia);
    }
}
