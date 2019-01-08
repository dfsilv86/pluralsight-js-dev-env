using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RegiaoCompraController : ApiControllerBase<IRegiaoCompraService>
    {
        public RegiaoCompraController(IRegiaoCompraService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<RegiaoCompra> ObterTodos()
        {
            return this.MainService.ObterTodos();
        }
    }
}
