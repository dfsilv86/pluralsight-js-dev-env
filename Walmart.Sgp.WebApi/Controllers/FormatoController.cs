using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.WebApi.Controllers
{
    /// <summary>
    /// A controller de formato.
    /// </summary>
    public class FormatoController : ApiControllerBase<IFormatoService>
    {
        #region Constructor        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FormatoController"/>.
        /// </summary>
        /// <param name="service">O serviço de formato.</param>
        public FormatoController(IFormatoService service)
            : base(service)
        {
        }
        #endregion

        #region Actions
        [HttpGet]
        [Route("Formato/PorSistema/{cdSistema}")]
        public IEnumerable<Formato> ObterPorSistema(int? cdSistema)
        {
            return MainService.ObterPorSistema(cdSistema);
        }

        [HttpGet]
        [Route("Formato")]
        public IEnumerable<Formato> ObterTodos()
        {
            return MainService.ObterTodos();
        }
        #endregion
    }
}