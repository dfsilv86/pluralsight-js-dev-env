using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Reabastecimento.Roteirizacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de RoteiroPedido.
    /// </summary>
    public interface IRoteiroPedidoService : IDomainService<RoteiroPedido>
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
        /// Autoriza um roteiro.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">Data do pedido.</param>
        void AutorizarPedidos(int idRoteiro, DateTime dtPedido);

        /// <summary>
        /// Calcula o TotalPedido com base no campo informado
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="usarQtdRoteiroRA">Se TRUE: QtdRoteiroRA, se FALSE: qtdPackCompra)</param>
        /// <returns>O total.</returns>
        int CalcularTotalRoteiro(int idRoteiro, DateTime dtPedido, bool usarQtdRoteiroRA);

        /// <summary>
        /// Calcula o Pedido com base no campo informado
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="usarQtdRoteiroRA">Se TRUE: QtdRoteiroRA, se FALSE: qtdPackCompra)</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <returns>O total.</returns>
        int CalcularTotalRoteiroItem(int idRoteiro, DateTime dtPedido, bool usarQtdRoteiroRA, int idItemDetalhe);

        /// <summary>
        /// Busca uma lista de RoteiroPedido
        /// </summary>
        /// <param name="idRoteiro">Id do Roteiro</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <param name="paging">Parametro de Paginação.</param>
        /// <returns>uma lista de RoteiroPedido</returns>
        IEnumerable<RoteiroPedido> ObterRoteirosPedidosPorRoteiroEdtPedido(int idRoteiro, DateTime dtPedido, Paging paging);

        /// <summary>
        /// Obtém a quantidade de pedidos não autorizados para a data corrente.
        /// </summary>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <returns>Retorna a quantidade de pedidos não autorizados para a data corrente.</returns>
        int QtdPedidosNaoAutorizadosParaDataCorrente(int idRoteiro);

        /// <summary>
        /// Retorna um RoteiroPedido com informacoes de Autorizacao DO ROTEIRO.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Um RoteiroPedido.</returns>
        RoteiroPedido ObterDadosAutorizacaoRoteiro(int idRoteiro, DateTime dtPedido);
    }
}