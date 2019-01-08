using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.WebApi.Controllers
{
    /// <summary>
    /// A controller de regiao.
    /// </summary>
    public class RegiaoController : ApiControllerBase<IRegiaoService>
    {
        #region Constructor        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RegiaoController"/>.
        /// </summary>
        /// <param name="service">O serviço de região.</param>
        public RegiaoController(IRegiaoService service)
            : base(service)
        {
        }

        #endregion

        #region Actions
        [HttpGet]
        [Route("Regiao/PorBandeira/{idBandeira}")]
        public IEnumerable<Regiao> ObterPorBandeira(int idBandeira)
        {
            return this.MainService.ObterPorBandeira(idBandeira);
        }
        #endregion
    }
}