using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Movimentacao;

namespace Walmart.Sgp.WebApi.Controllers
{
    /// <summary>
    /// A controller de status item da nota fiscal.
    /// </summary>
    public class NotaFiscalItemStatusController : ApiControllerBase<INotaFiscalItemStatusService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotaFiscalItemStatusController" />.
        /// </summary>
        /// <param name="service">O serviço de status item da nota fiscal service.</param>
        public NotaFiscalItemStatusController(INotaFiscalItemStatusService service)
            : base(service)
        {
        }

        /// <summary>
        /// Obtém todos os status disponíveis.
        /// </summary>
        /// <returns>Os status.</returns>
        [HttpGet]
        public IEnumerable<NotaFiscalItemStatus> ObterTodos()
        {
            return this.MainService.ObterTodos();
        }
    }
}