using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Application.Exporting;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class SugestaoReturnSheetController : ApiControllerBase<ISugestaoReturnSheetService>
    {
        private readonly SugestaoReturnSheetRAExporter m_sugestaoReturnSheetRAExporter;
        private IFileVaultService m_fileVaultService;

        public SugestaoReturnSheetController(ISugestaoReturnSheetService mainService, SugestaoReturnSheetRAExporter sugestaoReturnSheetRAExporter, IFileVaultService fileVaultService)
            : base(mainService)
        {
            m_sugestaoReturnSheetRAExporter = sugestaoReturnSheetRAExporter;
            m_fileVaultService = fileVaultService;
        }

        [HttpPut]
        [Route("SugestaoReturnSheet/AutorizarExportarPlanilhas")]
        [SecurityWebApiAction("SugestaoReturnSheet.AutorizarE")]
        public void AutorizarExportarPlanilhas([FromBody]SugestaoReturnSheetRARequest request)
        {
            this.MainService.AutorizarExportarPlanilhas(request.DtInicioReturn, request.DtFinalReturn, request.CdV9D, request.Evento, request.CdItemDetalhe, request.CdDepartamento, request.CdLoja, request.IdRegiaoCompra, request.BlExportado, request.BlAutorizado);
            Commit();
        }

        [HttpGet]
        public SugestaoReturnSheet Obter(int id)
        {
            return this.MainService.ObterPorId(id);
        }

        [HttpGet]
        public IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLoja(int idDepartamento, int idLoja, DateTime dataSolicitacao, string evento, long vendor9D, int idItemDetalhe, [FromUri] Paging paging)
        {
            return this.MainService.ConsultaReturnSheetLoja(idDepartamento, idLoja, dataSolicitacao.Date, evento, vendor9D, idItemDetalhe, paging);
        }

        [HttpGet]
        public IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLojaRA([FromUri]SugestaoReturnSheetRARequest request, [FromUri]Paging paging)
        {
            return this.MainService.ConsultaReturnSheetLojaRA(request.DtInicioReturn, request.DtFinalReturn, request.CdV9D, request.Evento, request.CdItemDetalhe, request.CdDepartamento, request.CdLoja, request.IdRegiaoCompra, request.BlExportado, request.BlAutorizado, paging);
        }

        [HttpPut, Route("SugestaoReturnSheet/SalvarLoja")]
        [SecurityWebApiAction("SugestaoReturnSheet.SalvarLoja")]
        public void SalvarLoja([FromBody]IEnumerable<SugestaoReturnSheet> sugestoes)
        {
            this.MainService.SalvarSugestoesLoja(sugestoes);
            Commit();
        }

        [HttpPut, Route("SugestaoReturnSheet/SalvarRA")]
        [SecurityWebApiAction("SugestaoReturnSheet.SalvarRA")]
        public void SalvarRA([FromBody]IEnumerable<SugestaoReturnSheet> sugestoes)
        {
            this.MainService.SalvarSugestoesRA(sugestoes);
            Commit();
        }

        [HttpGet]
        [Route("SugestaoReturnSheet/ExportarRA")]
        public HttpResponseMessage ExportarRA([FromUri]SugestaoReturnSheetRARequest request, [FromUri]Paging paging)
        {
            var stream = m_sugestaoReturnSheetRAExporter.Exportar(request.DtInicioReturn, request.DtFinalReturn, request.CdV9D, request.Evento, request.CdItemDetalhe, request.CdDepartamento, request.CdLoja, request.IdRegiaoCompra, request.BlExportado, request.BlAutorizado, paging);
            return stream.WithFileVault("Exportacao.xlsx", m_fileVaultService);
        }

        [HttpGet]
        [Route("SugestaoReturnSheet/possuiReturnsVigentesQuantidadesVazias")]
        public bool PossuiReturnsVigentesQuantidadesVazias()
        {
            return MainService.PossuiReturnsVigentesQuantidadesVazias();
        }

        [HttpPost]
        [Route("SugestaoReturnSheet/registrarLogAvisoReturnSheetsVigentes")]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        public void RegistrarLogAvisoReturnSheetsVigentes()
        {
            MainService.RegistrarLogAvisoReturnSheetsVigentes();
            Commit();
        }
    }
}