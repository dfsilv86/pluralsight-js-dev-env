using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de SugestaoPedidoCD.
    /// </summary>
    public interface ISugestaoPedidoCDService : IDomainService<SugestaoPedidoCD>
    {
        /// <summary>
        /// Obtém um SugestaoPedidoCD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade SugestaoPedidoCD.</returns>
        SugestaoPedidoCD ObterPorIdEstruturado(long id);

        /// <summary>
        /// Pesquisar SugestaoPedidoCD com base nos filtros
        /// </summary>
        /// <param name="filtro">Filtro da pesquisa.</param>
        /// <param name="paging">Parametro de paginacao.</param>
        /// <returns>Retorna lista de SugestaoPedidoCD.</returns>
        IEnumerable<SugestaoPedidoCD> Pesquisar(SugestaoPedidoCDFiltro filtro, Paging paging);

        /// <summary>
        /// Valida DataCancelamento de um SugestaoPedidoCD
        /// </summary>
        /// <param name="sugestaoPedidoCD">Sugestão Pedido CD</param>
        /// <returns>SpecResult contendo o resultado.</returns>
        SpecResult ValidarDataCancelamento(SugestaoPedidoCD sugestaoPedidoCD);

        /// <summary>
        /// Valida DataEnvio de um SugestaoPedidoCD
        /// </summary>
        /// <param name="sugestaoPedidoCD">Sugestão Pedido CD</param>
        /// <returns>SpecResult contendo o resultado.</returns>
        SpecResult ValidarDataEnvio(SugestaoPedidoCD sugestaoPedidoCD);

        /// <summary>
        /// Realiza a finalização dos pedidos de sugestão CD.
        /// </summary>
        /// <param name="sugestoesPedidoCD">A lista contendo os pedidos que serão finalizados.</param>
        void FinalizarPedidos(IEnumerable<SugestaoPedidoCD> sugestoesPedidoCD);

        /// <summary>
        /// Salvar uma lista de SugestaoPedidoCD.
        /// </summary>
        /// <param name="sugestoesPedidoCD">A lista contendo os pedidos que serão salvos.</param>
        void SalvarVarios(IEnumerable<SugestaoPedidoCD> sugestoesPedidoCD);
    }
}