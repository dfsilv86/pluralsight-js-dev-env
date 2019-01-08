using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Reabastecimento.Roteirizacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para RoteiroPedido.
    /// </summary>
    public interface IRoteiroPedidoGateway : IDataGateway<RoteiroPedido>
    {
        /// <summary>
        /// Obtém os pedidos roteirizados pelos filtros
        /// </summary>
        /// <param name="dtPedido">Data do pedido</param>
        /// <param name="idDepartamento">ID do departamento</param>
        /// <param name="cdV9D">Código Vendor 9 Dig.</param>
        /// <param name="stPedido">Status do pedido (Autorizado(true) ou Não autorizado (false))</param>
        /// <param name="roteiro">Descrição do roteiro</param>
        /// <param name="paging">Parametro de paginação</param>
        /// <returns>Lista de PedidoRoteirizadoConsolidado</returns>
        IEnumerable<PedidoRoteirizadoConsolidado> ObterPedidosRoteirizados(DateTime dtPedido, int idDepartamento, long? cdV9D, bool? stPedido, string roteiro, Paging paging);

        /// <summary>
        /// Busca uma lista de SugestaoPedido
        /// </summary>
        /// <param name="idRoteiro">Id do Roteiro</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <returns>uma lista de SugestaoPedido</returns>
        IEnumerable<SugestaoPedido> BuscarSugestaoPedidoPorRoteiro(int idRoteiro, DateTime dtPedido);

        /// <summary>
        /// Busca os pedidos para serem autorizados.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>A lista de RoteiroPedidos.</returns>
        IEnumerable<RoteiroPedido> ObterRoteirosPedidosParaAutorizar(int idRoteiro, DateTime dtPedido);

        /// <summary>
        /// Busca uma lista de RoteiroPedido
        /// </summary>
        /// <param name="idRoteiro">Id do Roteiro</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <param name="paging">Parametro de Paginação.</param>
        /// <returns>uma lista de RoteiroPedido</returns>
        IEnumerable<RoteiroPedido> ObterRoteirosPedidosPorRoteiroEdtPedido(int idRoteiro, DateTime dtPedido, Paging paging);

        /// <summary>
        /// Busca uma lista de SugestaoPedido
        /// </summary>
        /// <param name="idRoteiro">Id do RoteiroPedido</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <returns>uma lista de SugestaoPedido</returns>
        IEnumerable<SugestaoPedido> BuscarSugestaoPedidoPorRoteiroItem(int idRoteiro, DateTime dtPedido, int idItemDetalhe);

        /// <summary>
        /// Obtém a quantidade de pedidos não autorizados para a data corrente.
        /// </summary>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <returns>Retorna a quantidade de pedidos não autorizados para a data corrente.</returns>
        int QtdPedidosNaoAutorizadosParaDataCorrente(int idRoteiro);

        /// <summary>
        /// Obtem um RoteiroPedido com dados de autorizacao e usuario populados.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Um RoteiroPedido com dados de autorizacao populados.</returns>
        RoteiroPedido ObterDadosAutorizacao(int idRoteiro, DateTime dtPedido);
    }
}