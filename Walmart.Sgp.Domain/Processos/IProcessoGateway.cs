using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Define a interface de um table data gateway para processo.
    /// </summary>
    public interface IProcessoGateway : IDataGateway<Processo>
    {
        /// <summary>
        /// Pesquisa as cargas de processos de lojas que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>As cargas de processos de lojas.</returns>
        IEnumerable<LojaProcessosCarga> PesquisarCargas(ProcessoCargaFiltro filtro, Paging paging);

        /// <summary>
        /// Pesquisa processos de execução que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>Os processos de execução.</returns>
        IEnumerable<ProcessoExecucao> PesquisarProcessosExecucao(ProcessoExecucaoFiltro filtro, Paging paging);
    }
}
