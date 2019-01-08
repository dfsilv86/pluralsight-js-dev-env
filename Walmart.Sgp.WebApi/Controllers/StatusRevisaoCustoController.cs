using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class StatusRevisaoCustoController : ApiControllerBase<IStatusRevisaoCustoService>
    {
        public StatusRevisaoCustoController(IStatusRevisaoCustoService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<StatusRevisaoCusto> ObterTodos()
        {
            return this.MainService.ObterTodos();
        }  
    }
}