using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Define a interface para um serviço de domínio para processos.
    /// </summary>
    public interface IProcessoService : IDomainService<Processo>
    {
        /// <summary>
        /// Pesquisa as cargas de processos de lojas que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>As cargas de processos de lojas.</returns>
        IEnumerable<LojaProcessosCarga> PesquisarCargas(ProcessoCargaFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém a carga de processo para uma loja.
        /// </summary>
        /// <param name="filtro">O filtro para localizar a carga de processo da loja.</param>
        /// <returns>A cargas de processo da loja.</returns>
        LojaProcessosCarga ObterCargaPorLoja(ProcessoCargaFiltro filtro);

        /// <summary>
        /// Pesquisa processos de execução que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>Os processos de execução.</returns>
        IEnumerable<ProcessoExecucao> PesquisarProcessosExecucao(ProcessoExecucaoFiltro filtro, Paging paging);
    }
}
