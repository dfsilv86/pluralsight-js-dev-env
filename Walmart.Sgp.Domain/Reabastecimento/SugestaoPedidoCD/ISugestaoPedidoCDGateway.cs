using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para SugestaoPedidoCD.
    /// </summary>
    public interface ISugestaoPedidoCDGateway : IDataGateway<SugestaoPedidoCD>
    {
        /// <summary>
        /// Obtém um SugestaoPedidoCD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade SugestaoPedidoCD.</returns>
        SugestaoPedidoCD ObterPorIdEstruturado(long id);

        /// <summary>
        /// Verifica se existe grade aberta para a data e item informado.
        /// </summary>
        /// <param name="idItemDetalheSaida">O item a pesquisar.</param>
        /// <param name="idFornecedorParametro">O id fornecedor parametro.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Se existe ou não grade aberta.</returns>
        bool VerificaItemSaidaGradeAbertaSugestaoCD(int idItemDetalheSaida, int idFornecedorParametro, int cdSistema, DateTime dtPedido);

        /// <summary>
        /// Pesquisar SugestaoPedidoCD com base nos filtros
        /// </summary>
        /// <param name="filtro">Filtro da busca.</param>
        /// <param name="paging">Parametro de paginacao.</param>
        /// <returns>Retorna lista de SugestaoPedidoCD.</returns>
        IEnumerable<SugestaoPedidoCD> Pesquisar(SugestaoPedidoCDFiltro filtro, Paging paging);

        /// <summary>
        /// Verificar se existem sugestões finalizadas para o mesmo item detalhe de saída para a mesma data, departamento e CD.
        /// </summary>
        /// <param name="idSugestaoPedidoCD">O identificador da SugestaoPedidoCD.</param>
        /// <returns>Retorna true caso existam sugestões finalizadas para o mesmo item detalhe de saída para a mesma data, departamento e CD, do contrário retorna false.</returns>
        bool ExisteSugestoesFinalizadasMesmoItemDetalheSaida(long idSugestaoPedidoCD);

        /// <summary>
        /// Pequisar sugestões por filtro.
        /// </summary>
        /// <param name="dtSolicitacao">Data de solicitacao do pedido.</param>
        /// <param name="idDepartamento">ID do departamento.</param>
        /// <param name="idCD">ID do CD.</param>
        /// <param name="idItem">ID do item. (Entrada ou Saida)</param>
        /// <param name="idFornecedorParametro">ID do FornecedorParametro.</param>
        /// <param name="statusPedido">Filtrar por status do pedido: 0 - Nao finalizado, 1 - Finalizado, 2 - Todos</param>
        /// <returns>Retorna sugestões encontradas com base no filtro.</returns>
        IEnumerable<SugestaoPedidoCD> ObterSugestoesPorFiltro(DateTime dtSolicitacao, int? idDepartamento, int? idCD, int? idItem, int? idFornecedorParametro, int? statusPedido);
    }
}