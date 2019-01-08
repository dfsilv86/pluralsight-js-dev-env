using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class AlcadaDetalheController : ApiControllerBase<IAlcadaDetalheService>
    {
        public AlcadaDetalheController(IAlcadaDetalheService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<AlcadaDetalhe> ObterPorIdAlcada(int idAlcada, [FromUri]Paging paging)
        {
            return this.MainService.ObterPorIdAlcada(idAlcada, paging);
        }
    }
}