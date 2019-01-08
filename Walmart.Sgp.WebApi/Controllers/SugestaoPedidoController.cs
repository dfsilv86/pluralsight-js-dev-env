using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class SugestaoPedidoController : ApiControllerBase<ISugestaoPedidoService>
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoController"/>.
        /// </summary>
        /// <param name="mainService">O serviço de principal.</param>
        public SugestaoPedidoController(ISugestaoPedidoService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Pesquisa sugestões de pedidos pelos filtros informados.
        /// </summary>
        /// <param name="request">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>
        /// As sugestões de pedido.
        /// </returns>
        [HttpGet]
        public IEnumerable<SugestaoPedidoModel> PesquisarPorFiltros([FromUri]SugestaoPedidoFiltro request, [FromUri]Paging paging)
        {
            request.cdOrigemCalculo = request.cdOrigemCalculo == "T" ? null : request.cdOrigemCalculo;

            return this.MainService.PesquisarPorFiltros(request, paging);
        }

        [HttpPut]
        public AlterarSugestoesResponse AlterarSugestoes([FromBody] AlterarSugestoesRequest alterarSugestoesRequest)
        {
            var result = this.MainService.AlterarSugestoes(alterarSugestoesRequest.Sugestoes, alterarSugestoesRequest.IDLoja, alterarSugestoesRequest.DtPedido);

            Commit();

            return result;
        }

        [HttpPut]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("SugestaoPedido/ValidarAlteracaoSugestao")]
        public SpecResult ValidarSugestao([FromBody] ValidarSugestaoRequest alterarSugestoesRequest)
        {
            return this.MainService.ValidarAlteracaoSugestao(alterarSugestoesRequest.Sugestao, alterarSugestoesRequest.IDLoja, alterarSugestoesRequest.IDDepartamento, alterarSugestoesRequest.DtPedido);
        }

        [HttpPut]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("SugestaoPedido/ObterStatusAutorizarPedido")]
        public string ObterStatusAutorizarPedido([FromBody]AutorizarPedidoRequest request)
        {
            return this.MainService.ObterStatusAutorizarPedido(request.DtPedido.Date, request.IDLoja, request.IDDepartamento, request.CdSistema);
        }

        [HttpPut]
        [Route("SugestaoPedido/AutorizarPedido")]
        public string AutorizarPedido([FromBody]AutorizarPedidoRequest request)
        {
            var result = this.MainService.AutorizarPedido(request.DtPedido, request.IDLoja, request.IDDepartamento, request.CdSistema);

            Commit();

            LogService.Debug(
                Texts.OrderSugesstionAuthorizationLog,
                request.DtPedido,
                request.CdSistema,
                request.IDLoja,
                request.IDDepartamento,
                RuntimeContext.Current.User.UserName);

            return result;
        }

        [HttpGet]
        [Route("SugestaoPedido/{id}/ComAlcada")]
        public SugestaoPedidoModel ObterEstruturado(int id)
        {
            return this.MainService.ObterEstruturadoComAlcada(id);
        }

        [HttpPut]
        [Route("SugestaoPedido/recalcular")]
        public SugestaoPedidoModel Recalcular(SugestaoPedidoModel sugestaoPedido)
        {
            return SugestaoPedidoModel.Recalcular(sugestaoPedido);
        }

        [HttpGet]
        [Route("SugestaoPedido/{idSugestaoPedido}/logs")]
        public IEnumerable<AuditRecord<SugestaoPedido>> ObterLogs([FromUri] AuditFilter filter, [FromUri] Paging paging)
        {
            return MainService.ObterLogs(filter, paging);
        }

        [HttpGet]
        [Route("SugestaoPedido/Quantidade")]
        public QuantidadeSugestaoPedido ObterQuantidade(DateTime data)
        {
            var quantidade = MainService.ObterQuantidade(data);

            return quantidade;
        }
    }
}
