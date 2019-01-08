using System;
using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RoteiroController : ApiControllerBase<IRoteiroService>
    {
        public RoteiroController(IRoteiroService service)
            : base(service)
        {
        }

        [HttpGet]
        public Roteiro ObterPorId(int idRoteiro)
        {
            return MainService.ObterPorId(idRoteiro);
        }

        [HttpGet]
        [Route("Roteiro/{id}/estruturado")]
        public Roteiro ObterEstruturadoPorId(int id)
        {
            return MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        public IEnumerable<Roteiro> ObterRoteirosPorFornecedor(long? cdV9D, int? cdDepartamento, int? cdLoja, string roteiro, [FromUri]Paging paging)
        {
            return MainService.ObterRoteirosPorFornecedor(cdV9D, cdDepartamento, cdLoja, roteiro, paging);
        }

        [HttpPost]
        [SecurityWebApiAction("Roteiro.Salvar")]
        public Roteiro Salvar(Roteiro roteiro)
        {
            MainService.Salvar(roteiro);
            Commit();
            return roteiro;
        }

        [HttpDelete]
        [Route("Roteiro/{id}/Delete")]
        [SecurityWebApiAction("Roteiro.Delete")]
        public void Delete(int id)
        {
            MainService.Remover(id);
            Commit();
        }

        [HttpGet]
        public IEnumerable<SugestaoPedido> ObterSugestaoPedidoLoja(int idRoteiro, string dtPedido, int idItemDetalhe, [FromUri]Paging paging)
        {
            return this.MainService.ObterSugestaoPedidoLoja(idRoteiro, dtPedido.ToDate(), idItemDetalhe, paging);
        }

        [HttpPost]
        [Route("Roteiro/{id}")]
        [SecurityWebApiAction("Roteiro.SalvarSugestaoPedidoConvertidoCaixa")]
        public void SalvarSugestaoPedidoConvertidoCaixa(IEnumerable<SugestaoPedido> sugestoesConvertidas, [FromUri] int id)
        {
            this.MainService.SalvarSugestaoPedidoConvertidoCaixa(sugestoesConvertidas, id);

            Commit();
        }
    }
}