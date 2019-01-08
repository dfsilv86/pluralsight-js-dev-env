using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ProcessoController : ApiControllerBase<IProcessoService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessoController"/>.
        /// </summary>
        /// <param name="processoService">O serviço de inventário.</param>
        public ProcessoController(IProcessoService processoService)
            : base(processoService)
        {
        }

        [HttpGet]
        [Route("Processo/Carga")]
        public IEnumerable<LojaProcessosCarga> PesquisarCargas([FromUri] ProcessoCargaFiltro filtro, [FromUri]Paging paging)
        {
            return MainService.PesquisarCargas(filtro, paging);
        }

        [HttpGet]
        [Route("Processo/Carga/loja")]
        public LojaProcessosCarga ObterCargaPorLoja([FromUri] ProcessoCargaFiltro filtro)
        {
            return MainService.ObterCargaPorLoja(filtro);
        }

        [HttpGet]
        [Route("Processo/Execucao")]
        public IEnumerable<ProcessoExecucao> PequisarProcessosExecucao([FromUri]ProcessoExecucaoFiltro filtro, [FromUri]Paging paging)
        {
            return MainService.PesquisarProcessosExecucao(filtro, paging);
        }

        [HttpGet]
        public IEnumerable<Processo> ObterTodos()
        {
            return this.MainService.ObterTodos();
        }      
    }
}
