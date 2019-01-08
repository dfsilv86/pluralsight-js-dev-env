using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RoteiroLojaController : ApiControllerBase<IRoteiroLojaService>
    {
        public RoteiroLojaController(IRoteiroLojaService mainService)
            : base(mainService)
        {
        }

        [HttpGet]
        public IEnumerable<RoteiroLoja> ObterLojasValidas(long cdV9D, string dsEstado, int? idRoteiro, [FromUri]Paging paging)
        {
            return this.MainService.ObterLojasValidas(cdV9D, dsEstado, idRoteiro, paging);
        }
    }
}
