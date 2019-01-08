using System;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class OrigemCalculoController : ApiControllerBase<IOrigemCalculoService>
    {
        public OrigemCalculoController(IOrigemCalculoService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("OrigemCalculo/Disponibilidade")]
        public DisponibilidadeOrigemCalculo ObterDisponibilidade(DateTime dia)
        {
            var disponibilidade = MainService.ObterDisponibilidade(dia);

            return disponibilidade;
        }
    }
}