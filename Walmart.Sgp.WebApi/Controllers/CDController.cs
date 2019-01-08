using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class CDController : ApiControllerBase<ICDService>
    {
        public CDController(ICDService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("CD/ConvertidosAtivos")]
        public IEnumerable<CD> ObterTodosConvertidosAtivos()
        {
            return this.MainService.ObterTodosConvertidosAtivos();
        }
    }
}
