using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class BandeiraController : ApiControllerBase<IBandeiraService>
    {
        public BandeiraController(IBandeiraService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("Bandeira/PorSistema")]
        public IEnumerable<BandeiraResumo> ObterPorUsuarioESistema(int idUsuario, int? cdSistema, int? idFormato)
        {
            return this.MainService.ObterPorUsuarioESistema(idUsuario, cdSistema, idFormato);
        }

        [HttpGet]
        [Route("Bandeira/PorRegiaoAdministrativa")]
        public IEnumerable<BandeiraResumo> ObterPorUsuarioERegiaoAdministrativa(int idUsuario, int cdSistema, int? idRegiaoAdministrativa)
        {
            return this.MainService.ObterPorUsuarioERegiaoAdministrativa(idUsuario, cdSistema, idRegiaoAdministrativa);
        }

        [HttpGet]
        [Route("Bandeira/PorFiltro")]
        public IEnumerable<Bandeira> PesquisarPorFiltros([FromUri]BandeiraFiltro filtro, [FromUri]Paging paging)
        {
            return MainService.PesquisarPorFiltros(filtro, paging);
        }

        [HttpGet]
        [Route("Bandeira/{id}/estruturado")]
        public Bandeira ObterEstruturadoPorId(int id)
        {
            return MainService.ObterEstruturadoPorId(id);
        }

        [HttpPost]
        public Bandeira Salvar(Bandeira entidade)
        {
            MainService.Salvar(entidade);
            Commit();

            return entidade;
        }
    }
}