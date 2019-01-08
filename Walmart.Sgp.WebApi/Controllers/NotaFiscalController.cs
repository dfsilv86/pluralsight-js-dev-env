using System;
using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class NotaFiscalController : ApiControllerBase<INotaFiscalService>
    {
        #region Constructor
        public NotaFiscalController(INotaFiscalService mainService)
            : base(mainService)
        {
        }
        #endregion

        #region Actions
        [HttpGet]
        [Route("NotaFiscal/{id}/Estruturado")]
        public NotaFiscal ObterEstruturadoPorId(int id)
        {
            return this.MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        [Route("NotaFiscal/{idNotaFiscal}/itens")]
        public IEnumerable<NotaFiscalItem> ObterItensDaNotaFiscal(int idNotaFiscal, [FromUri]Paging paging)
        {
            return this.MainService.ObterItensDaNotaFiscal(idNotaFiscal, paging);
        }

        [HttpGet]
        [Route("NotaFiscal/PesquisarPorFiltros")]
        public IEnumerable<NotaFiscal> PesquisarPorFiltros([FromUri]NotaFiscalFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarPorFiltros(filtro, paging);
        }

        [HttpGet]
        [Route("NotaFiscal/PesquisarCustosPorFiltros")]
        public IEnumerable<CustoNotaFiscal> PesquisarCustosPorFiltros([FromUri]NotaFiscalFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarCustosPorFiltros(filtro, paging);
        }

        [HttpGet]
        [Route("NotaFiscal/PesquisarUltimasEntradas")]
        public IEnumerable<NotaFiscalConsolidado> PesquisarUltimasEntradasPorFiltros(long idItemDetalhe, int idLoja, DateTime dtSolicitacao, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarUltimasEntradasPorFiltros(idItemDetalhe, idLoja, dtSolicitacao, paging);
        }

        [HttpGet]
        [Route("NotaFiscal/ObterCustosPorItem")]
        public NotaFiscalItemCustosConsolidado ObterCustosPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao)
        {
            return this.MainService.ObterCustosPorItem(cdLoja, cdItem, dtSolicitacao);
        }

        [HttpGet]
        [Route("NotaFiscal/NotasPendentes")]
        public bool ExisteNotasPendentesPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao)
        {
            return this.MainService.ExisteNotasPendentesPorItem(cdLoja, cdItem, dtSolicitacao);
        }

        [HttpPost]
        [Route("NotaFiscal/CorrigirCustos")]
        public void CorrigirCustos(IEnumerable<CustoNotaFiscal> custos)
        {
            this.MainService.CorrigirCustos(custos);
            
            Commit();
        }
        #endregion
    }
}