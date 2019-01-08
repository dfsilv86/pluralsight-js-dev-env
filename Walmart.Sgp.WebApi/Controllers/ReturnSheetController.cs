using System;
using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ReturnSheetController : ApiControllerBase<IReturnSheetService>
    {
        public ReturnSheetController(IReturnSheetService mainService)
            : base(mainService)
        {
        }

        [HttpGet]
        public ReturnSheet Obter(long id)
        {
            var r = this.MainService.Obter(id);
            r.HoraCorte = r.DhFinalReturn;

            return r;
        }

        [HttpGet]
        public IEnumerable<ReturnSheet> Pesquisar(DateTime? inicioReturn, DateTime? finalReturn, string evento, int? idDepartamento, int filtroAtivos, int? idRegiaoCompra, [FromUri] Paging paging)
        {
            return this.MainService.Pesquisar(inicioReturn, finalReturn, evento, idDepartamento, filtroAtivos, idRegiaoCompra, paging);
        }

        [HttpPost]
        [SecurityWebApiAction("ReturnSheet.Inserir")]
        public ReturnSheet Inserir([FromBody]ReturnSheet rs)
        {
            if (rs.IsNew)
            {
                rs.DhCriacao = DateTime.Now;
                rs.BlAtivo = true;
            }
            else
            {
                rs.DhAtualizacao = DateTime.Now;
            }

            rs.DhInicioReturn = rs.DhInicioReturn.Date;
            rs.DhFinalReturn = rs.DhFinalReturn.Date.AddHours(rs.HoraCorte.Hour).AddMinutes(rs.HoraCorte.Minute);

            rs.DhInicioEvento = rs.DhInicioEvento.Date;
            rs.DhFinalEvento = rs.DhFinalEvento.Date;

            this.MainService.Salvar(rs);
            this.Commit();

            rs.HoraCorte = rs.DhFinalReturn;
            return rs;
        }

        [HttpDelete]
        [SecurityWebApiAction("ReturnSheet.Remover")]
        public void Remover(int id)
        {
            this.MainService.Remover(id);
            Commit();
        }

        [HttpGet]
        public bool PodeSerEditada(int idRS)
        {
            return this.MainService.PodeSerEditada(idRS);
        }

        [HttpGet]
        [Route("ReturnSheet/JaComecou/")]
        public bool JaComecou(int idRS)
        {
            var returnSheet = this.MainService.ObterPorId(idRS);
            if (returnSheet != null)
            {
                return returnSheet.DhInicioReturn != null && returnSheet.DhInicioReturn <= DateTime.Now;
            }

            return false;
        }
    }
}