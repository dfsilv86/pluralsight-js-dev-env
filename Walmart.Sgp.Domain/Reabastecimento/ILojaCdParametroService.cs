using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para um serviço de parâmetro de loja/CD.
    /// </summary>
    public interface ILojaCdParametroService : IDomainService<LojaCdParametro>
    {
        /// <summary>
        /// Pesquisa parâmetros de loja/CD pelos filtros informados.
        /// </summary>
        /// <param name="filtro">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os parâmetros de loja/CD.</returns>
        IEnumerable<LojaCdParametroPorDepartamento> PesquisarPorFiltros(LojaCdParametroFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém o LojaCdParametro com o id informado.
        /// </summary>
        /// <param name="idLojaCdParametro">O id.</param>
        /// <param name="tpReabastecimento">O tipo de reabastecimento.</param>
        /// <returns>O LojaCdParametro.</returns>
        LojaCdParametro ObterEstruturadoPorId(int idLojaCdParametro, TipoReabastecimento tpReabastecimento);
    }
}
