using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class LojaController : ApiControllerBase<ILojaService>
    {
        public LojaController(ILojaService service)
            : base(service)
        {
        }

        [HttpGet]
        public Loja ObterPorLojaUsuarioEBandeira(int cdLoja, int idUsuario, short? idBandeira)
        {
            return this.MainService.ObterPorLojaUsuarioEBandeira(idUsuario, idBandeira, cdLoja);
        }

        [HttpGet]
        public IEnumerable<Loja> Pesquisar(int idUsuario, byte cdSistema, short? idBandeira, int? cdLoja, string nmLoja, [FromUri]Paging paging)
        {
            if (string.IsNullOrWhiteSpace(nmLoja))
            {
                nmLoja = null;
            }

            return this.MainService.Pesquisar(idUsuario, cdSistema, idBandeira, cdLoja, nmLoja, paging);
        }

        [HttpGet]
        [Route("Loja/PesquisarPorItemDestinoOrigem")]
        public IEnumerable<Loja> PesquisarLojasPorItemDestinoItemOrigem([FromUri]LojaFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarLojasPorItemDestinoItemOrigem(filtro, paging);
        }

        [HttpGet]
        [Route("Loja/{id}/Estruturado")]
        public Loja ObterEstruturadoPorId(int id)
        {
            return this.MainService.ObterEstruturadoPorId(id);
        }

        [HttpPost]        
        public Loja AlterarLoja(Loja loja)
        {
            var result = this.MainService.AlterarLoja(loja);

            Commit();

            return result;
        }
    }
}