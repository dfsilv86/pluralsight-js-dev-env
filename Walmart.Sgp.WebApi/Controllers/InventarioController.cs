using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class InventarioController : ApiControllerBase<IInventarioService>
    {
        #region Fields
        private const string NomeArquivoRelatorioDataHora = "{0}_{1:yyyyMMdd}_{1:HHmmss}";
        private readonly IParametroSistemaService m_parametroSistemaService;
        private readonly ILojaService m_lojaService;
        private readonly IDepartamentoService m_departamentoService;
        private readonly IInventarioImportacaoService m_inventarioImportacaoService;
        private readonly IFileVaultService m_fileVault;
        private readonly IProcessingService m_backgroundProcessService;
        #endregion

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioController"/>.
        /// </summary>
        /// <param name="inventarioService">O serviço de inventário.</param>
        /// <param name="parametroSistemaService">O serviço de parâmetro sistema.</param>
        /// <param name="lojaService">O serviço de loja.</param>
        /// <param name="departamentoService">O serviço de departamento.</param>
        /// <param name="inventarioImportacaoService">O serviço de importação de inventário.</param>
        /// <param name="fileVault">O serviço de FileVault.</param>
        /// <param name="backgroundProcessService">O serviço de processamento em background.</param>
        public InventarioController(IInventarioService inventarioService, IParametroSistemaService parametroSistemaService, ILojaService lojaService, IDepartamentoService departamentoService, IInventarioImportacaoService inventarioImportacaoService, IFileVaultService fileVault, IProcessingService backgroundProcessService)
            : base(inventarioService)
        {
            m_parametroSistemaService = parametroSistemaService;
            m_lojaService = lojaService;
            m_departamentoService = departamentoService;
            m_inventarioImportacaoService = inventarioImportacaoService;
            m_fileVault = fileVault;
            m_backgroundProcessService = backgroundProcessService;
        }

        [HttpGet]
        [Route("Inventario/Loja/{idLoja}/data")]
        public DateTime? ObterDataInventarioDaLoja(int idLoja)
        {
            return MainService.ObterDataInventarioDaLoja(idLoja);
        }

        [HttpGet]
        [Route("Inventario/loja/naoAgendados/quantidade")]
        public int ObterQuantidadeLojasSemAgendamento()
        {
            return MainService.ObterQuantidadeLojasSemAgendamento(RuntimeContext.Current.User.Id);
        }

        [HttpGet]
        [Route("Inventario/ValidarDataObedeceQuantidadeDiasLimiteExpurgo")]
        public void ValidarDataObedeceQuantidadeDiasLimiteExpurgo([FromUri]DateTime? dtAgendamento)
        {
            RangeValue<DateTime> agendamento = new RangeValue<DateTime>();
            agendamento.StartValue = dtAgendamento;
            agendamento.EndValue = dtAgendamento;

            SpecService.Assert(agendamento, new DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec(m_parametroSistemaService));
        }

        [HttpGet]
        [Route("Inventario/ValidarFiltrosRelatorioItensPorInventario")]
        public void ValidarFiltrosRelatorioItensPorInventario([FromUri]int? cdLoja, [FromUri]DateTime? dtInventario)
        {
            SpecService.Assert(new { cdLoja, dtInventario }, new AllMustBeInformedSpec());
        }

        [HttpPut]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("Inventario/Loja/{id}/ValidarDatasAbertasParaImportacaoInventario")]
        public void ValidarDatasAbertasParaImportacaoInventario(int id)
        {
            SpecService.Assert(new Inventario { IDLoja = id }, new DatasDevemEstarAbertasParaImportacaoInventarioSpec(MainService));
        }

        [Route("Inventario/Loja/{idLoja}/Data/ProximoAgendamento/")]
        [HttpGet]
        public DateTime? ObterDataProximoInventarioAberto(int idLoja, byte? cdSistema, int? idDepartamento)
        {
            // frmImportarInventario.aspx.cs linha 135 ->
            // InventarioData.cs linha 450, FindInventarioDataAbertosParaImportacao
            var inventarios = MainService.ObterInventariosAbertosParaImportacao(idLoja, null, idDepartamento, m_inventarioImportacaoService.CarregarCategoriaAtacado(new ImportarInventarioAutomaticoRequest { CdSistema = cdSistema ?? (byte)1, IdLoja = idLoja, IdDepartamento = idDepartamento }));

            return inventarios.Select(x => (DateTime?)x.dhInventario).OrderBy(x => x).FirstOrDefault();
        }

        [HttpPost]
        [Route("Inventario/Importar/Automatico/Loja")]
        public ImportarInventarioResponse ImportarAutomaticoLoja(ImportarInventarioAutomaticoRequest model)
        {
            var result = m_inventarioImportacaoService.ImportarAutomatico(model, TipoOrigemImportacao.Loja);
            ////var result = m_backgroundProcessService.Dispatch(() => m_inventarioImportacaoService.ImportarAutomatico(model, TipoOrigemImportacao.Loja));

            Commit();

            return result;
        }

        [HttpGet]
        [Route("Inventario/Importar/Manual/HO/PrefixosArquivos")]
        public IEnumerable<string> ObterPrefixosArquivos()
        {
            return m_inventarioImportacaoService.ObterPrefixosArquivos();
        }

        [HttpPost]
        [Route("Inventario/Importar/Manual/HO/AdicionarArquivo")]
        [SecurityWebApiAction("Inventario.ImportarManual")]
        public FileVaultTicket AdicionarArquivoImportacaoManual()
        {
            return m_fileVault.Store(System.Web.HttpContext.Current.GetFirstFile().ToFileVault());
        }

        [HttpPost]
        [Route("Inventario/Importar/Manual/HO")]
        [SecurityWebApiAction("Inventario.ImportarManual")]
        public ImportarInventarioResponse ImportarManualHo(ImportarInventarioManualRequest model)
        {
            var result = m_inventarioImportacaoService.ImportarManual(model, TipoOrigemImportacao.HO);
            ////var result = m_backgroundProcessService.Dispatch(() => m_inventarioImportacaoService.ImportarManual(model, TipoOrigemImportacao.HO));

            Commit();

            return result;
        }

        [HttpGet]
        [Route("Inventario/loja/agendamento")]
        public IEnumerable<InventarioAgendamento> ObterAgendamentos([FromUri] InventarioAgendamentoFiltro filtro)
        {
            return MainService.ObterAgendamentos(filtro);
        }

        [HttpGet]
        [Route("Inventario/loja/naoAgendados")]
        public IEnumerable<InventarioNaoAgendado> ObterNaoAgendados([FromUri] InventarioAgendamentoFiltro filtro)
        {
            return MainService.ObterNaoAgendados(filtro);
        }

        [HttpGet]
        [Route("Inventario/loja/agendamento/{id}/estruturado")]
        public InventarioAgendamento ObterAgendamentoEstruturadoPorId(int id)
        {
            var result = MainService.ObterAgendamentoEstruturadoPorId(id);

            return result;
        }

        [HttpGet]
        [Route("Inventario/sumario")]
        public IEnumerable<InventarioSumario> ObterInventarioSumarizadoPorFiltro([FromUri]InventarioFiltro filtro, [FromUri]Paging paging)
        {
            return MainService.ObterSumarizadoPorFiltro(filtro, paging);
        }

        [HttpGet]
        [Route("Inventario/custo/total")]
        public decimal ObterCustoTotalPorFiltro([FromUri]InventarioFiltro filtro)
        {
            var result = MainService.ObterCustoTotalPorFiltro(filtro);
            return result;
        }

        [HttpPost]
        [Route("Inventario/loja/agendamento/remover")]
        public void RemoverAgendamentos(IdsViewModel request)
        {
            MainService.RemoverAgendamentos(request.Ids);
            Commit();
        }

        [HttpPost]
        [SecurityWebApiAction("Inventario.SalvarAgendamento")]
        [Route("Inventario/agendamentos")]
        public AgendamentoResponse InserirAgendamentos(InserirAgendamentosRequest request)
        {
            var selector = new LojaDepartamentoSelector(m_lojaService, m_departamentoService);
            selector.Selecionar(request.CdSistema, request.IDBandeira, request.CdLoja, request.CdDepartamento);

            var quantidadeAgendamentosInseridos = MainService.InserirAgendamentos(request.DtAgendamento, selector.Lojas, selector.Departamentos);
            Commit();

            return quantidadeAgendamentosInseridos;
        }

        [HttpPut]
        [SecurityWebApiAction("Inventario.SalvarAgendamento")]
        [Route("Inventario/agendamentos")]
        public AgendamentoResponse AtualizarAgendamentos(AtualizarAgendamentosRequest request)
        {
            var quantidadeAgendamentosAtualizados = MainService.AtualizarAgendamentos(request.DtAgendamento, request.InventarioAgendamentoIDs);
            Commit();

            return quantidadeAgendamentosAtualizados;
        }

        [HttpGet]
        [Route("Inventario/{id}/estruturado")]
        public InventarioSumario ObterEstruturadoPorId(int id)
        {
            return MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        [Route("Inventario/PermissoesAlteracao")]
        public InventarioOperacoesPermitidas ObterOperacoesPermitidas([FromUri]Inventario inventario)
        {
            return MainService.ObterOperacoesPermitidas(inventario);
        }

        [HttpGet]
        [Route("Inventario/{id}/Itens")]
        public IEnumerable<InventarioItemSumario> ObterItensEstruturadoPorFiltro(
            int id,
            [FromUri]InventarioItemFiltro filtro,
            [FromUri]Paging paging)
        {
            filtro.IDInventario = id;
            return MainService.ObterItensEstruturadoPorFiltro(filtro, paging);
        }

        [HttpGet]
        [Route("Inventario/Item/{id}")]
        public InventarioItem ObterItemEstruturadoPorId(int id)
        {
            return MainService.ObterItemEstruturadoPorId(id);
        }

        [HttpPut]
        [SecurityWebApiAction("Inventario.SalvarInventarioItem")]
        [Route("Inventario/{id}/Item/{idItem}")]
        public void AtualizarInventarioItem(int id, int idItem, InventarioItem item)
        {
            var inventario = MainService.ObterPorId(id);
            item.IDInventarioItem = idItem;
            MainService.SalvarItem(item, inventario);
            Commit();
        }

        [HttpPost]
        [SecurityWebApiAction("Inventario.SalvarInventarioItem")]
        [Route("Inventario/{id}/Item")]
        public void InserirInventarioItem(int id, [FromBody]InventarioItem item)
        {
            item.IDInventarioItem = 0;
            var inventario = MainService.ObterPorId(id);
            MainService.SalvarItem(item, inventario);
            Commit();
        }

        [HttpDelete]
        [Route("Inventario/Item/{id}")]
        public void RemoverInventarioItem(int id)
        {
            MainService.RemoverItem(id);
            Commit();
        }

        [HttpGet]
        [Route("Inventario/{id}/Finalizacao/Irregularidades")]
        public IEnumerable<string> ObterIrregularidadesFinalizacao(int id)
        {
            return MainService.ObterIrregularidadesFinalizacao(id);
        }

        [HttpPut]
        [Route("Inventario/{id}/Finalizar")]
        public void Finalizar(int id)
        {
            MainService.Finalizar(id);
            Commit();
        }

        [HttpPut]
        [Route("Inventario/{id}/Cancelar")]
        public void Cancelar(int id)
        {
            MainService.Cancelar(id);
            Commit();
        }

        [HttpPut]
        [Route("Inventario/{id}/VoltarStatus")]
        public void VoltarStatus(int id)
        {
            MainService.VoltarStatus(id);
            Commit();
        }

        [HttpPut]
        [Route("Inventario/{id}/Aprovar")]
        public void Aprovar(int id)
        {
            MainService.Aprovar(id);
            Commit();
        }

        [HttpGet]
        [Route("Inventario/{id}/Aprovacao/Irregularidades")]
        public IEnumerable<string> ObterIrregularidadesAprovacao(int id)
        {
            return MainService.ObterIrregularidadesAprovacao(id);
        }

        [HttpGet]
        [Route("Inventario/Criticas")]
        public IEnumerable<InventarioCritica> PesquisarCriticas([FromUri]InventarioCriticaFiltro filtro, [FromUri]Paging paging)
        {
            return MainService.PesquisarCriticas(filtro, paging);
        }

        #region Relatórios

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("Inventario/ExportarRelatorioInventarioAgendamento")]
        public HttpResponseMessage ExportarRelatorioInventarioAgendamento(InventarioAgendamentoModel model)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Bandeira", string.IsNullOrEmpty(model.DsBandeira) ? string.Empty : model.DsBandeira);
            parameters.Add("Loja", string.IsNullOrEmpty(model.NmLoja) ? string.Empty : model.NmLoja);
            parameters.Add("Departamento", string.IsNullOrEmpty(model.DsDepartamento) ? string.Empty : model.DsDepartamento);
            parameters.Add("DtAgendamento", model.DtAgendamento == null ? null : ((DateTime)model.DtAgendamento).ToString("yyyy/MM/dd"));
            parameters.Add("IDBandeira", model.IDBandeira);
            parameters.Add("IDLoja", model.IDLoja);
            parameters.Add("IDDepartamento", model.IDDepartamento);
            parameters.Add("Tipo", model.NaoAgendados);

            return Download(ReportFile.InventarioAgendamento, "Manter Calendário de Inventário", parameters);
        }

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("Inventario/ExportarRelatorioItensModificados")]
        public HttpResponseMessage ExportarRelatorioItensModificados(InventarioItensModificadosModel model)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("Loja", model.Loja);
            parameters.Add("DeptoCateg", model.DeptoCateg);
            parameters.Add("DataInventario", string.Format("{0:dd/MM/yyyy}", model.DtInventario));
            parameters.Add("IDDepartamento", model.IDDepartamento);
            parameters.Add("IDCategoria", model.IDCategoria);
            parameters.Add("DTInventario", model.DtInventario.ToString("yyyy/MM/dd"));
            parameters.Add("IDUsuario", RuntimeContext.Current.User.Id);
            parameters.Add("IDLoja", model.IDLoja);

            var reportName = string.Format(NomeArquivoRelatorioDataHora, "RelItensModificados", DateTime.Now);

            return Download(ReportFile.InventarioAjustes, reportName, parameters);
        }

        [HttpPost]
        [Route("Inventario/ExportarRelatorioComparacaoEstoque")]
        public HttpResponseMessage ExportarRelatorioComparacaoEstoque(InventarioComparacaoEstoqueModel model)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            InventarioStatus inventarioStatus = (InventarioStatus)model.StatusInventario;

            parameters.Add("Loja", model.Loja);
            parameters.Add("DataInventario", string.Format("{0:dd/MM/yyyy}", model.DataInventario));
            parameters.Add("StatusInventarioDescricao", inventarioStatus.Description);
            parameters.Add("StatusInventario", model.StatusInventario);
            parameters.Add("Departamento", model.Departamento);
            parameters.Add("IDInventario", model.IDInventario);

            var reportName = string.Format(NomeArquivoRelatorioDataHora, "RelComparativoInventario", DateTime.Now);

            return Download(ReportFile.InventarioComparativoAnalitico, reportName, parameters);
        }

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("Inventario/ExportarRelatorioPreparacaoInventario")]
        public HttpResponseMessage ExportarRelatorioPreparacaoInventario(PreparacaoInventarioModel model)
        {
            SpecService.Assert(
                new Inventario
                {
                    IDLoja = model.IDLoja,
                    IDDepartamento = model.IDDepartamento
                },
                new InventarioPodeSerPreparadoSpec(MainService));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("labelLoja", model.NmLoja);
            parameters.Add("lockLoja", string.Format("{0} - {1}", model.CdLoja, model.NmLoja));
            parameters.Add("IDLoja", model.IDLoja);
            parameters.Add("IDDepartamento", model.IDDepartamento);
            parameters.Add("IDCategoria", null);
            parameters.Add("IDUsuario", RuntimeContext.Current.User.Id);

            string reportFile = string.Empty;
            string reportName = string.Empty;

            switch (model.IDTipoRelatorio)
            {
                case 0:
                    reportFile = ReportFile.InventarioItensSMART;
                    reportName = Texts.ItensInventSMART;

                    break;
                case 1:
                    reportFile = ReportFile.InventarioItensSIP;
                    reportName = Texts.ItensInventSIP;

                    break;
            }

            reportName = string.Format(NomeArquivoRelatorioDataHora, reportName, DateTime.Now);

            return Download(reportFile, reportName, parameters);
        }

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("Inventario/ExportarRelatorioItensPorInventario")]
        public HttpResponseMessage ExportarRelatorioItensPorInventario(ItensPorInventarioModel model)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("Loja", string.Format("{0} - {1}", model.CdLoja, model.Loja));
            parameters.Add("Data", model.DtInventario.ToShortDateString());
            parameters.Add("stInventario", model.StInventario);
            parameters.Add("idLoja", model.IdLoja);
            parameters.Add("dtInventario", model.DtInventario.ToString("yyyy/MM/dd"));

            var reportName = string.Format(NomeArquivoRelatorioDataHora, "rptItensPorInventario", DateTime.Now);

            return Download(ReportFile.InventarioItens, reportName, parameters);
        }

        private HttpResponseMessage Download(string reportName, string fileName, Dictionary<string, object> parameters)
        {
            return DownloadReportHelper.DownloadExcel(Request, reportName, fileName, parameters, m_fileVault);
        }

        #endregion
    }
}
