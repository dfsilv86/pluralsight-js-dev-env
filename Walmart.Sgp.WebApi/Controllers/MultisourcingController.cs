using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Application.Exporting;
using Walmart.Sgp.Application.Importing;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class MultisourcingController : ApiControllerBase<IMultisourcingService>
    {
        #region Fields
        private MultisourcingExcelImporter m_multisourcingExcelImporter;
        private MultisourcingExcelExporter m_multisourcingExcelExporter;
        private IFileVaultService m_fileVaultService;
        #endregion

        #region Constructor
        public MultisourcingController(IMultisourcingService mainService, MultisourcingExcelImporter multisourcingExcelImporter, MultisourcingExcelExporter multisourcingExcelExporter, IFileVaultService fileVaultService)
            : base(mainService)
        {
            m_multisourcingExcelImporter = multisourcingExcelImporter;
            m_multisourcingExcelExporter = multisourcingExcelExporter;
            m_fileVaultService = fileVaultService;
        }
        #endregion

        #region Actions

        [HttpGet]
        public Multisourcing Obter(long id)
        {
            return this.MainService.Obter(id);
        }

        [HttpDelete]
        [SecurityWebApiAction("Multisourcing.DeletarMultisourcing")]
        public void DeletarMultisourcing(long cdItemSaida, long cdCD)
        {
            this.MainService.Excluir(cdItemSaida, cdCD);
            Commit();
        }

        [HttpPut, Route("Multisourcing/inserir/{idUsuario}")]
        [SecurityWebApiAction("Multisourcing.InserirMultisourcing")]
        public void InserirMultisourcing([FromBody] IEnumerable<ItemDetalheCD> itens, int idUsuario)
        {
            this.MainService.SalvarMultisourcing(itens, idUsuario, 1);
            Commit();
        }

        [HttpPost]
        [SecurityWebApiAction("Multisourcing.ImportarMultisourcing")]
        [Route("Multisourcing/AdicionarArquivoImportacao")]
        public FileVaultTicket AdicionarArquivoImportacao()
        {
            return m_fileVaultService.Store(System.Web.HttpContext.Current.GetFirstFile().ToFileVault());
        }

        [HttpPost]
        [SecurityWebApiAction("Multisourcing.ImportarMultisourcing")]
        public HttpResponseMessage ImportarMultisourcing(ImportarMultisourcingRequest model)
        {
            SpecService.Assert(new { selectExcelFile = model.Arquivos.FirstOrDefault().FileName, cdUsuario = model.IdUsuario, MarketingStructure = model.CdSistema }, new AllMustBeInformedSpec());
            SpecService.Assert(model.Arquivos.FirstOrDefault(), new ExcelExtensionSpec());

            var result = m_multisourcingExcelImporter.ImportarMultisourcing(model);
            if (result == null)
            {
                Commit();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (result != null)
            {
                response.Content = new StringContent("NOK");
                response.Headers.Add("x-file-vault-ticket-id", HttpUtility.UrlEncode(result.Id));
                response.Headers.Add("x-file-vault-ticket-created-date", result.CreatedDate.ToUniversalTime().ToString("o"));
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage Exportar(long? idItemDetalhe, int? idDepartamento, int? cdSistema, int? idCD, int filtroMS, int filtroCadastro)
        {
            var stream = m_multisourcingExcelExporter.Exportar(idItemDetalhe, idDepartamento, cdSistema, idCD, filtroMS, filtroCadastro);

            return stream.WithFileVault("Exportacao.xlsx", m_fileVaultService);
        }
        #endregion
    }
}
