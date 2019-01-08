using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class GradeSugestaoController : ApiControllerBase<IGradeSugestaoService>
    {
        public GradeSugestaoController(IGradeSugestaoService service)
            : base(service)
        {
        }

        [HttpGet]        
        public IEnumerable<GradeSugestao> PesquisarPorFiltro(int cdSistema, int? idBandeira, int? cdDepartamento, int? cdLoja, [FromUri]Paging paging)
        {
            return MainService.PesquisarEstruturadoPorFiltro(cdSistema, idBandeira, cdDepartamento, cdLoja, paging);
        }

        [HttpGet]
        public GradeSugestao ObterEstruturado(int id)
        {
            return MainService.ObterEstruturadoPorId(id);
        }

        [HttpPut]
        public GradeSugestao Atualizar(GradeSugestao entidade)
        {
            MainService.Atualizar(entidade);
            Commit();
            return entidade;
        }

        [HttpPost]
        public void Criar(GradeSugestao[] entidades)
        {
            MainService.SalvarNovas(entidades);
            Commit();
        }

        [HttpDelete]
        public void Remover(int id)
        {
            MainService.Remover(id);
            Commit();
        }

        [HttpGet]
        [Route("GradeSugestao/Count")]
        public long ContarExistentes(int cdSistema, int idBandeira, int idLoja, int idDepartamento)
        {
            return MainService.ContarExistentes(cdSistema, idBandeira, idLoja, idDepartamento);
        }
    }
}