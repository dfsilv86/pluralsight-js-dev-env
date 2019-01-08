using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Movimentacao;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class MotivoMovimentacaoController : ApiControllerBase<IMotivoMovimentacaoService>
    {
        public MotivoMovimentacaoController(IMotivoMovimentacaoService service)
            : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<MotivoMovimentacao> ObterVisiveis()
        {
            return this.MainService.ObterVisiveis();
        }
    }
}