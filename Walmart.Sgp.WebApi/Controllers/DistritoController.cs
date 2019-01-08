using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class DistritoController : ApiControllerBase<IDistritoService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DistritoController"/>.
        /// </summary>
        /// <param name="service">O serviço de distrito.</param>
        public DistritoController(IDistritoService service)
            : base(service)
        {
        }

        [HttpPost]
        [Route("Distrito/Salvar")]
        public Distrito Salvar(Distrito distrito)
        {
            this.MainService.Salvar(distrito);
            Commit();

            return distrito;
        }

        [HttpGet]
        [Route("Distrito/ObterPorId/{idDistrito}")]
        public Distrito ObterPorId(int idDistrito)
        {
            return this.MainService.ObterEstruturado(idDistrito);
        }

        [HttpGet]
        [Route("Distrito/PorRegiao/{idRegiao}")]
        public IEnumerable<Distrito> ObterPorRegiao(int idRegiao)
        {
            return this.MainService.ObterPorRegiao(idRegiao);
        }

        [HttpGet]
        [Route("Distrito/Pesquisar")]
        public IEnumerable<Distrito> Pesquisar(int? cdSistema, int? idBandeira, int? idRegiao, int? idDistrito, [FromUri] Paging paging)
        {
            return this.MainService.Pesquisar(cdSistema, idBandeira, idRegiao, idDistrito, paging);
        }
    }
}