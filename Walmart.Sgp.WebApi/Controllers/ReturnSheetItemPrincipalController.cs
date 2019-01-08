using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ReturnSheetItemPrincipalController : ApiControllerBase<IReturnSheetItemPrincipalService>
    {
        public ReturnSheetItemPrincipalController(IReturnSheetItemPrincipalService mainService)
            : base(mainService)
        {
        }

        [HttpGet]
        public ReturnSheetItemPrincipal Obter(int id)
        {
            return this.MainService.ObterPorId(id);
        }

        [HttpGet]
        public IEnumerable<ReturnSheetItemPrincipal> PesquisarPorIdReturnSheet(int idReturnSheet, [FromUri] Paging paging)
        {
            var r = this.MainService.PesquisarPorIdReturnSheet(idReturnSheet, paging);
            return r;
        }

        [HttpDelete]
        [SecurityWebApiAction("ReturnSheetItemPrincipal.Remover")]
        public void Remover(int id)
        {
            this.MainService.Remover(id);
            Commit();
        }

        [HttpPost]
        [SecurityWebApiAction("ReturnSheetItemPrincipal.SalvarLojas")]
        public bool SalvarLojas(ReturnSheetItemPrincipalRequest request)
        {
            this.MainService.SalvarLojas(request.LojasAlteradas, request.IdReturnSheet, request.PrecoVenda);
            Commit();
            return true;
        }

        [HttpDelete]
        [SecurityWebApiAction("ReturnSheetItemPrincipal.RemoverItem")]
        public void Remover(int idReturnSheet, int cdItem)
        {
            this.MainService.Remover(idReturnSheet, cdItem);
            Commit();
        }
    }
}