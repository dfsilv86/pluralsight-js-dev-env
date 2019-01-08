using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class MotivoRevisaoCustoController : ApiControllerBase<IMotivoRevisaoCustoService>
    {
        public MotivoRevisaoCustoController(IMotivoRevisaoCustoService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<MotivoRevisaoCusto> ObterTodos()
        {
            return this.MainService.ObterTodos();
        }
    }
}