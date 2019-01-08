using System;
using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RevisaoCustoController : ApiControllerBase<IRevisaoCustoService>
    {
        public RevisaoCustoController(IRevisaoCustoService service)
            : base(service)
        {
        }

        [HttpGet]
        public RevisaoCusto ObterEstruturadoPorId(int id)
        {
            return this.MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        [Route("RevisaoCusto/PesquisarPorFiltros")]
        public IEnumerable<RevisaoCusto> PesquisarPorFiltros([FromUri]RevisaoCustoFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarPorFiltros(filtro, paging);
        }

        [HttpPost]
        public RevisaoCusto Salvar(RevisaoCusto revisaoCusto)
        {
            if (revisaoCusto.dtRevisado <= DateTime.MinValue)
            {
                revisaoCusto.dtRevisado = (DateTime?)null;
            }

            if (revisaoCusto.dtCustoRevisado <= DateTime.MinValue)
            {
                revisaoCusto.dtCustoRevisado = (DateTime?)null;
            }

            this.MainService.Salvar(revisaoCusto);
            Commit();            
            return revisaoCusto;
        }
    }
}