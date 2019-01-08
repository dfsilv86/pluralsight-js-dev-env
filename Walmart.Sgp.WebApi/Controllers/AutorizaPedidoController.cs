using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class AutorizaPedidoController : ApiControllerBase<IAutorizaPedidoService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AutorizaPedidoController"/>.
        /// </summary>
        /// <param name="autorizaPedidoService">O serviço de inventário.</param>
        public AutorizaPedidoController(IAutorizaPedidoService autorizaPedidoService)
            : base(autorizaPedidoService)
        {
        }

        [HttpGet]
        public IEnumerable<AutorizaPedido> ObterAutorizacoesPorSugestaoPedido(int idSugestaoPedido, [FromUri] Paging paging)
        {
            return MainService.ObterAutorizacoesPorSugestaoPedido(idSugestaoPedido, paging);
        }
    }
}
