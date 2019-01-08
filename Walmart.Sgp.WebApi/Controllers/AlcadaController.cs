using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class AlcadaController : ApiControllerBase<IAlcadaService>
    {
        // TODO: revisar todos os endpoints de alçada
        public AlcadaController(IAlcadaService service)
            : base(service)
        {
        }

        [HttpGet]
        public Alcada ObterPorPerfil(int id)
        {
            return MainService.ObterEstruturadoPorPerfil(id);
        }

        [HttpPost]
        public Alcada Salvar(Alcada alcada)
        {
            MainService.Salvar(alcada);
            Commit();
            return alcada;
        }

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("Alcada/ValidarDuplicidadeDetalhe")]
        public SpecResult ValidarDuplicidadeDetalhe(Alcada entidade)
        {
            return MainService.ValidarDuplicidadeDetalhe(entidade);
        }
    }
}