using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ReturnSheetItemLojaController : ApiControllerBase<IReturnSheetItemLojaService>
    {
        public ReturnSheetItemLojaController(IReturnSheetItemLojaService mainService)
            : base(mainService)
        {
        }

        [HttpGet]
        public ReturnSheetItemLoja Obter(int id)
        {
            return this.MainService.ObterPorId(id);
        }

        [HttpDelete]
        [SecurityWebApiAction("ReturnSheetItemLoja.Remover")]
        public void Remover(int id)
        {
            this.MainService.Remover(id);
            Commit();
        }

        [HttpGet]
        public IEnumerable<ReturnSheetItemLoja> ObterLojasValidasItem(int cdItem, int cdSistema, int idReturnSheet, string dsEstado, [FromUri]Paging paging)
        {
            return this.MainService.ObterLojasValidasItem(cdItem, cdSistema, idReturnSheet, dsEstado, paging);
        }

        [HttpGet]
        public IEnumerable<string> ObterEstadosLojasValidasItem(int cdItem, int cdSistema)
        {
            return this.MainService.ObterEstadosLojasValidasItem(cdItem, cdSistema);
        }
    }
}