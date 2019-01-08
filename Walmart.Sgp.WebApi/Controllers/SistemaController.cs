using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class SistemaController : ApiControllerBase<ISistemaService>
    {
        public SistemaController(ISistemaService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<Sistema> ObterPorUsuario(int idUsuario)
        {
            return this.MainService.ObterPorUsuario(idUsuario, RuntimeContext.Current.Culture.Name);
        }
    }
}