using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class LojaCdParametroController : ApiControllerBase<ILojaCdParametroService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaCdParametroController"/>.
        /// </summary>
        /// <param name="mainService">O serviço de principal.</param>
        public LojaCdParametroController(ILojaCdParametroService mainService)
            : base(mainService)
        {
        }
       
        [HttpGet]
        public IEnumerable<LojaCdParametroPorDepartamento> PesquisarPorFiltros([FromUri]LojaCdParametroFiltro filtro, [FromUri]Paging paging)
        {
            var result = MainService.PesquisarPorFiltros(filtro, paging);

            return result;
        }

        [HttpGet]
        [Route("LojaCdParametro/{id}/estruturado")]
        public LojaCdParametro ObterEstruturadoPorId(int id, [FromUri] TipoReabastecimento tpReabastecimento)
        {
            var result = MainService.ObterEstruturadoPorId(id, tpReabastecimento);

            return result;
        }

        [HttpPost]
        public LojaCdParametro Salvar(LojaCdParametro entidade)
        {
            MainService.Salvar(entidade);
            Commit();

            return entidade;
        }
    }
}
