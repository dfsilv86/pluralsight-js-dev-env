using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.Controllers
{
    /// <summary>
    /// A controller de status item host.
    /// </summary>
    public class StatusItemHostController : ApiControllerBase<IStatusItemHostService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="StatusItemHostController" />.
        /// </summary>
        /// <param name="service">O serviço de status item host service.</param>
        public StatusItemHostController(IStatusItemHostService service)
                            : base(service)
        {
        }

        /// <summary>
        /// Obtém todos os status disponíveis no idioma informado.
        /// </summary>
        /// <returns>Os status.</returns>
        /// <remarks>Utiliza o idioma reportado pelo RuntimeContext.</remarks>
        //// TODO: cadastrar valores em en-US e deixar o globalization traduzir?
        [HttpGet]
        public IEnumerable<StatusItemHost> ObterTodos()
        {
            return this.MainService.ObterPorCultura(RuntimeContext.Current.Culture.Name);
        }
    }
}