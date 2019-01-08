using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Movimentacao;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class TipoMovimentacaoController : ApiControllerBase<ITipoMovimentacaoService>
    {
        public TipoMovimentacaoController(ITipoMovimentacaoService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("TipoMovimentacao/categoria/{categoria}")]
        public IEnumerable<TipoMovimentacao> ObterPorCategoria([FromUri] CategoriaTipoMovimentacao categoria)
        {
            return MainService.ObterPorCategoria(categoria);
        }
    }
}