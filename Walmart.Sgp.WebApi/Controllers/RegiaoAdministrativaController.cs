using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RegiaoAdministrativaController : ApiControllerBase<IRegiaoAdministrativaService>
    {
        public RegiaoAdministrativaController(IRegiaoAdministrativaService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<RegiaoAdministrativa> ObterTodos()
        {
            return this.MainService.ObterTodos();
        }
    }
}
