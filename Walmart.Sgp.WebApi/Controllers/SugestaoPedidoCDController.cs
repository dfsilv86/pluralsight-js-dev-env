using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Application.Exporting;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class SugestaoPedidoCDController : ApiControllerBase<ISugestaoPedidoCDService>
    {
        private SugestaoPedidoCDExcelExporter m_sugestaoPedidoCDExcelExporter;
        private IFileVaultService m_fileVaultService;

        public SugestaoPedidoCDController(ISugestaoPedidoCDService service, SugestaoPedidoCDExcelExporter sugestaoPedidoCDExcelExporter, IFileVaultService fileVaultService)
            : base(service)
        {
            m_sugestaoPedidoCDExcelExporter = sugestaoPedidoCDExcelExporter;
            m_fileVaultService = fileVaultService;
        }

        [HttpGet]
        public SugestaoPedidoCD ObterPorId(long idSugestaoPedidoCD)
        {
            return MainService.ObterPorIdEstruturado(idSugestaoPedidoCD);
        }

        [HttpGet]
        public IEnumerable<SugestaoPedidoCD> Pesquisar([FromUri] SugestaoPedidoCDFiltro filtro, [FromUri] Paging paging)
        {
            return MainService.Pesquisar(filtro, paging);
        }

        [HttpPut]
        [Route("SugestaoPedidoCD/ValidarDataCancelamento")]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        public SpecResult ValidarDataCancelamento([FromBody] SugestaoPedidoCD sugestaoPedidoCD)
        {
            return MainService.ValidarDataCancelamento(sugestaoPedidoCD);
        }

        [HttpPut]
        [Route("SugestaoPedidoCD/ValidarDataEnvio")]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        public SpecResult ValidarDataEnvio([FromBody] SugestaoPedidoCD sugestaoPedidoCD)
        {
            return MainService.ValidarDataEnvio(sugestaoPedidoCD);
        }

        [HttpPost]
        [Route("SugestaoPedidoCD/FinalizarPedidos")]
        [SecurityWebApiAction("SugestaoPedidoCD.FinalizarPedidos")]
        public void FinalizarPedidos(IEnumerable<SugestaoPedidoCD> sugestoesPedidoCD)
        {
            MainService.FinalizarPedidos(sugestoesPedidoCD);
            Commit();
        }

        [HttpPost]
        [Route("SugestaoPedidoCD/SalvarPedidos")]
        [SecurityWebApiAction("SugestaoPedidoCD.SalvarPedidos")]
        public void SalvarPedidos(IEnumerable<SugestaoPedidoCD> sugestoesPedidoCD)
        {
            MainService.SalvarVarios(sugestoesPedidoCD);
            Commit();
        }

        [HttpGet]
        [Route("SugestaoPedidoCD/Exportar")]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        public HttpResponseMessage Exportar(DateTime? dtSolicitacao, int? idDepartamento, int? idCD, int? idItem, int? idFornecedorParametro, int? statusPedido)
        {
            var stream  = m_sugestaoPedidoCDExcelExporter.Exportar(dtSolicitacao, idDepartamento, idCD, idItem, idFornecedorParametro, statusPedido);

            return stream.WithFileVault("Exportacao.xlsx", m_fileVaultService);
        }
    }
}