using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Roteirizacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RoteiroPedidoController : ApiControllerBase<IRoteiroPedidoService>
    {
        private readonly IFileVaultService m_fileVault;

        public RoteiroPedidoController(IRoteiroPedidoService service, IFileVaultService fileVault)
            : base(service)
        {
            m_fileVault = fileVault;
        }

        [HttpGet]
        public int CalcularTotalPedido(int idRoteiro, string dtPedido, bool usarQtdRoteiroRA)
        {
            return this.MainService.CalcularTotalRoteiro(idRoteiro, dtPedido.ToDate(), usarQtdRoteiroRA);
        }

        [HttpGet]
        public RoteiroPedido ObterPorId(int idRoteiroPedido)
        {
            return this.MainService.ObterPorId(idRoteiroPedido);
        }

        [HttpGet]
        public IEnumerable<RoteiroPedido> ObterRoteirosPedidosPorRoteiroEdtPedido(int idRoteiro, string dtPedido, [FromUri]Paging paging)
        {
            return this.MainService.ObterRoteirosPedidosPorRoteiroEdtPedido(idRoteiro, dtPedido.ToDate(), paging);
        }

        [HttpGet]
        public IEnumerable<PedidoRoteirizadoConsolidado> ObterPedidosRoteirizados(DateTime dtPedido, int idDepartamento, long? cdV9D, bool? stPedido, string roteiro, [FromUri]Paging paging)
        {
            return this.MainService.ObterPedidosRoteirizados(dtPedido, idDepartamento, cdV9D, stPedido, roteiro, paging);
        }

        [HttpPost]
        [SecurityWebApiAction("RoteiroPedido.AutorizarRoteiroPedido")]
        public void AutorizarPedidos([FromBody]PedidoRoteirizadoConsolidado roteiro)
        {
            this.MainService.AutorizarPedidos(roteiro.idRoteiro, roteiro.DtPedido.Value);

            Commit();
        }

        [HttpPut]
        [SecurityWebApiAction("RoteiroPedido.AutorizarPedidos")]
        public void AutorizarPedidos([FromBody] IEnumerable<PedidoRoteirizadoConsolidado> roteiros)
        {
            var dtPedido = roteiros.First().DtPedido.Value;

            foreach (var r in roteiros)
            {
                this.MainService.AutorizarPedidos(r.idRoteiro, dtPedido);
            }

            Commit();
        }

        [HttpGet]
        [Route("RoteiroPedido/NaoAutorizados")]
        public int QtdPedidosNaoAutorizadosParaDataCorrente(int idRoteiro)
        {
            return MainService.QtdPedidosNaoAutorizadosParaDataCorrente(idRoteiro);
        }

        [HttpGet]
        [Route("RoteiroPedido/DadosAutorizacao")]
        public RoteiroPedido ObterDadosAutorizacaoRoteiro(int idRoteiro, string dtPedido)
        {
            return this.MainService.ObterDadosAutorizacaoRoteiro(idRoteiro, dtPedido.ToDate());
        }

        [HttpPost]
        [Route("RoteiroPedido/ExportarRelatorio")]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        public HttpResponseMessage ExportarRelatorio(PedidoRoteirizadoModel model) 
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("idRoteiro", model.IDRoteiro);
            parameters.Add("dtPedido", model.DataPedido);
            parameters.Add("dsRoteiro", model.DsRoteiro);
            parameters.Add("nmVendor", model.NmVendor);
            parameters.Add("cdVendor", model.CdVendor);
            parameters.Add("IDItemDetalhe", model.IDItemDetalhe);

            string reportFile = ReportFile.DetalhePedidoRoteirizado;
            string reportName = Texts.detailedScriptedOrderReport;

            reportName += string.Format(" - {0:yyyyMMdd}", DateTime.Now);

            return DownloadReportHelper.DownloadExcel(Request, reportFile, reportName, parameters, m_fileVault);
        }
    }
}