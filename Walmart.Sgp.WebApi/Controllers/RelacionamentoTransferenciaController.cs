using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RelacionamentoTransferenciaController : ApiControllerBase<IRelacionamentoTransferenciaService>
    {
        public RelacionamentoTransferenciaController(
            IRelacionamentoTransferenciaService service)
            : base(service)
        {            
        }

        [HttpGet]
        [Route("RelacionamentoTransferencia/{id}/Estruturado")]
        public RelacionamentoTransferencia ObterEstruturadoPorId(int id)
        {
            return this.MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        [Route("RelacionamentoTransferencia/PesquisarPorFiltro")]
        public IEnumerable<RelacionamentoTransferenciaConsolidado> PesquisarPorFiltro([FromUri]RelacionamentoTransferenciaFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarPorFiltro(filtro, paging);
        }

        [HttpGet]
        [Route("RelacionamentoTransferencia/PesquisarItensRelacionados")]
        public IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionados(long idItemDetalheDestino, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarItensRelacionados(idItemDetalheDestino, paging);
        }

        [HttpGet]
        [Route("RelacionamentoTransferencia/PesquisarItensRelacionadosPorCdItemDestino")]
        public IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionadosPorCdItemDestino(long cdItemDestino, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarItensRelacionadosPorCdItemDestino(cdItemDestino, paging);
        }

        [HttpPost]
        [Route("RelacionamentoTransferencia/CriarTransferencia")]
        public void CriarTransferencia(RelacionamentoTransferenciaRequest request)
        {
            this.MainService.CriarTransferencia(request.IDItemDetalheDestino, request.IDItemDetalheOrigem, request.Lojas);
            Commit();
        }

        [HttpPut]
        [Route("RelacionamentoTransferencia/RemoverTransferencia")]
        public void RemoverTransferencia(RelacionamentoTransferencia[] items)
        {
            this.MainService.RemoverTransferencias(items);
            Commit();
        }
    }
}