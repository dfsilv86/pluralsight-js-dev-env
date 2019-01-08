using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Serviço de domínio para processos.
    /// </summary>
    public class ProcessoService : EntityDomainServiceBase<Processo, IProcessoGateway>, IProcessoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessoService"/>
        /// </summary>
        /// <param name="processoGateway">O table data gateway para processo.</param>
        public ProcessoService(IProcessoGateway processoGateway)
            : base(processoGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Pesquisa as cargas de processos de lojas que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>As cargas de processos de lojas.</returns>
        public IEnumerable<LojaProcessosCarga> PesquisarCargas(ProcessoCargaFiltro filtro, Paging paging)
        {
            return MainGateway.PesquisarCargas(filtro, paging);
        }

        /// <summary>
        /// Obtém a carga de processo para uma loja.
        /// </summary>
        /// <param name="filtro">O filtro para localizar a carga de processo da loja.</param>
        /// <returns>A cargas de processo da loja.</returns>
        public LojaProcessosCarga ObterCargaPorLoja(ProcessoCargaFiltro filtro)
        {
            Assert(filtro, new AllMustBeInformedSpec());
            return MainGateway.PesquisarCargas(filtro, new Paging(0, 1)).FirstOrDefault();
        }

        /// <summary>
        /// Pesquisa processos de execução que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>Os processos de execução.</returns>
        public IEnumerable<ProcessoExecucao> PesquisarProcessosExecucao(ProcessoExecucaoFiltro filtro, Paging paging)
        {
            return MainGateway.PesquisarProcessosExecucao(filtro, paging);
        }
        #endregion
    }
}
