using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class PapelController : ApiControllerBase<IPapelService>
    {        
        public PapelController(IPapelService service) : base(service)
        {            
        }

        [HttpGet]
        public IEnumerable<Papel> ObterTodos([FromUri]Paging paging)
        {
            return MainService.ObterTodos(paging);
        }
    }
}