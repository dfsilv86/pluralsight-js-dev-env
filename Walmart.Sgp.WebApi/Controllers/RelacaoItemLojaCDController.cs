using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Walmart.Sgp.Application.Exporting;
using Walmart.Sgp.Application.Importing;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class RelacaoItemLojaCDController : ApiControllerBase<IRelacaoItemLojaCDService>
    {
        #region Fields
        private readonly IFileVaultService m_fileVaultService;
        private readonly IProcessingService m_backgroundProcessService;
        private readonly RelacaoItemLojaCDVinculoExcelImporter m_relacaoItemLojaCDVinculoExcelImporter;
        private readonly RelacaoItemLojaCDDesvinculoExcelImporter m_relacaoItemLojaCDDesvinculoExcelImporter;
        private RelacaoItemLojaCDExcelExporter m_relacaoItemLojaCDExcelExporter;
        #endregion

        public RelacaoItemLojaCDController(IRelacaoItemLojaCDService mainService, IFileVaultService fileVaultService, IProcessingService backgroundProcessService, RelacaoItemLojaCDVinculoExcelImporter relacaoItemLojaCDVinculoExcelImporter, RelacaoItemLojaCDDesvinculoExcelImporter relacaoItemLojaCDDesvinculoExcelImporter, RelacaoItemLojaCDExcelExporter relacaoItemLojaCDExporter)
            : base(mainService)
        {
            m_fileVaultService = fileVaultService;
            m_backgroundProcessService = backgroundProcessService;
            m_relacaoItemLojaCDVinculoExcelImporter = relacaoItemLojaCDVinculoExcelImporter;
            m_relacaoItemLojaCDDesvinculoExcelImporter = relacaoItemLojaCDDesvinculoExcelImporter;
            m_relacaoItemLojaCDExcelExporter = relacaoItemLojaCDExporter;
        }

        [HttpPost]
        [SecurityWebApiAction("RelacaoItemLojaCD.Vincular")]
        [Route("RelacaoItemLojaCD/AdicionarArquivoVinculo")]
        public FileVaultTicket AdicionarArquivoVinculo()
        {
            return m_fileVaultService.Store(System.Web.HttpContext.Current.GetFirstFile().ToFileVault());
        }

        [HttpPost]
        [SecurityWebApiAction("RelacaoItemLojaCD.Desvincular")]
        [Route("RelacaoItemLojaCD/AdicionarArquivoDesvinculo")]
        public FileVaultTicket AdicionarArquivoDesvinculo()
        {
            return m_fileVaultService.Store(System.Web.HttpContext.Current.GetFirstFile().ToFileVault());
        }
                       
        [HttpPost]
        [SecurityWebApiAction("RelacaoItemLojaCD.Vincular")]
        [Route("RelacaoItemLojaCD/ImportarVinculos")]
        public ProcessOrderResult<FileVaultTicket> ImportarVinculos(ImportarRelacaoItemLojaCDRequest model)
        {
            ValidarArquivoImportacao(model);

            ProcessOrderResult<FileVaultTicket> result = m_backgroundProcessService.Dispatch(() => m_relacaoItemLojaCDVinculoExcelImporter.ImportarVincular(model));

            Commit();

            return result;
        }

        [HttpPost]
        [SecurityWebApiAction("RelacaoItemLojaCD.Desvincular")]
        [Route("RelacaoItemLojaCD/ImportarDesvinculos")]
        public ProcessOrderResult<FileVaultTicket> ImportarDesvinculos(ImportarRelacaoItemLojaCDRequest model)
        {
            ValidarArquivoImportacao(model);

            ProcessOrderResult<FileVaultTicket> result = m_backgroundProcessService.Dispatch(() => m_relacaoItemLojaCDDesvinculoExcelImporter.ImportarDesvincular(model));

            Commit();

            return result;
        }

        [HttpGet]
        [Route("RelacaoItemLojaCD/ObterProcessamentosImportacao")]
        public IEnumerable<ProcessOrder> ObterProcessamentosImportacao(int? userId, string processName, ProcessOrderState? state, [FromUri]Paging paging)
        {
            var result = this.MainService.ObterProcessamentosImportacao(userId, processName, state, paging);

            return result;
        }

        /// <summary>
        /// Obtem o modelo de vinculo
        /// </summary>
        /// <returns>Arquivo xlsx</returns>
        [Route("RelacaoItemLojaCD/ObterModeloVinculo")]
        [HttpGet]
        public HttpResponseMessage ObterModeloVinculo()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Walmart.Sgp.WebApi.Resources.ModeloVinculacaoExportacao.xlsx"))
            {
                return stream.WithFileVault("template.xlsx", m_fileVaultService);
            }
        }

        /// <summary>
        /// Obtem o modelo de desvinculo
        /// </summary>
        /// <returns>Arquivo xlsx</returns>
        [Route("RelacaoItemLojaCD/ObterModeloDesvinculo")]
        [HttpGet]
        public HttpResponseMessage ObterModeloDesvinculo()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Walmart.Sgp.WebApi.Resources.ModeloDesvinculacao.xlsx"))
            {
                return stream.WithFileVault("template.xlsx", m_fileVaultService);
            }
        }

        /// <summary>
        /// Metodo para Obter a listagem de RelacaoItemLojaCD
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">Paginação do resultado</param>
        /// <returns>Lista de relacao item loja cd</returns>
        [HttpGet]
        public IEnumerable<RelacaoItemLojaCDConsolidado> ObterPorFiltro([FromUri]RelacaoItemLojaCDFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.ObterPorFiltro(filtro, paging);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        /// <returns>Entidade salva</returns>
        [HttpPost]
        public RelacaoItemLojaCD[] Salvar([FromBody]RelacaoItemLojaCD[] entidade)
        {
            foreach (var item in entidade)
            {
                MainService.Salvar(item);
            }

            Commit();

            return entidade;
        }

        [HttpGet]
        [Route("RelacaoItemLojaCD/Exportar")]
        public HttpResponseMessage Exportar([FromUri]RelacaoItemLojaCDFiltro filtro)
        {
            var stream = m_relacaoItemLojaCDExcelExporter.Exportar(filtro);

            return stream.WithFileVault("Exportacao.xlsx", m_fileVaultService);
        }

        private static void ValidarArquivoImportacao(ImportarRelacaoItemLojaCDRequest model)
        {
            var arquivo = model.Arquivos.FirstOrDefault();

            SpecService.Assert(new { selectExcelFile = arquivo.FileName, cdUsuario = model.IdUsuario, MarketingStructure = model.CdSistema }, new AllMustBeInformedSpec());
            SpecService.Assert(arquivo, new ExcelExtensionSpec());
        }
    }
}