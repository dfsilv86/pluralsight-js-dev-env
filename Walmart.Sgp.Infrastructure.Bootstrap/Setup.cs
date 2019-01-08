using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using LightInject;
using Walmart.Sgp.Application.Exporting;
using Walmart.Sgp.Application.Importing;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.CompraCasada;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Net;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.IO.Importing;
using Walmart.Sgp.Infrastructure.IO.Importing.Inventario;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;
using Walmart.Sgp.Infrastructure.IO.Logging;
using Walmart.Sgp.Infrastructure.IO.Mail;
using Walmart.Sgp.Infrastructure.IO.Processing;
using Walmart.Sgp.Infrastructure.Web.FileVault;

namespace Walmart.Sgp.Infrastructure.Bootstrap
{
    /// <summary>
    /// Responsável por realizar a leitura de arquivos de configuração, a correta configuração de serviços 
    /// e afins, além, da definição de injeção de dependência.
    /// </summary>
    public class Setup
    {
        #region Fields
        private readonly SetupConfig m_config;
        private LogSection m_logSection;
        private Func<ILifetime> m_createLifetime;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Setup"/>.
        /// </summary>
        /// <param name="config">A configuração utilizada durante a execução do setup.</param>
        public Setup(SetupConfig config)
        {
            ExceptionHelper.ThrowIfAnyNull(new { config });

            m_config = config;
            m_createLifetime = config.LifetimeFactory;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define a callback chamada quando o registro de injeção de dependência é inicializado.
        /// </summary>
        public Action<ServiceContainer, LogSection> RegisterDIStartedCallback { get; set; }

        /// <summary>
        /// Obtém ou define a callback chamada quando o registro de injeção de dependência está para finalizar.
        /// </summary>
        public Action<ServiceContainer, LogSection> RegisterEndingCallback { get; set; }

        /// <summary>
        /// Obtém ou define a callback chamada quando a execução do setup está para finalizar.
        /// </summary>
        public Action<LogSection> RunEndingCallback { get; set; }

        /// <summary>
        /// Obtém o container de injeção de dependência.
        /// </summary>
        public ServiceContainer DIContainer { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a execução do setup.
        /// </summary>
        public void Run()
        {
            LogService.Initialize(new Log4netLogStrategy("default"));

            using (m_logSection = new LogSection(m_config.AppName))
            {
                m_logSection.Log("Version: {0}", m_config.AppVersion);
                ListAppConfig();
                RegisterDI();

                if (RunEndingCallback != null)
                {
                    RunEndingCallback(m_logSection);
                }
            }
        }

        private static void ListAppSettings(LogSection configSection)
        {
            using (var section = new LogSection("AppSettings", configSection))
            {
                var appSettings = ConfigurationManager.AppSettings;
                var secureKeyRegex = new Regex("(secret|user|password|pwd)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                foreach (var key in appSettings.AllKeys)
                {
                    if (secureKeyRegex.IsMatch(key))
                    {
                        continue;
                    }

                    section.Log("{0}: {1}", key, appSettings[key]);
                }
            }
        }

        private static void ListConnectionStrings(LogSection configSection)
        {
            using (var section = new LogSection("ConnectionString", configSection))
            {
                foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                {
                    if (cs.Name.Equals("LocalSqlServer", System.StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var connectionStringBuilder = new SqlConnectionStringBuilder(cs.ConnectionString);

                    section.Log("{0}: {1}\\{2} (Connect Timeout: {3})", cs.Name, connectionStringBuilder.DataSource, connectionStringBuilder.InitialCatalog, connectionStringBuilder.ConnectTimeout);
                }
            }
        }

        private void ListAppConfig()
        {
            using (var section = new LogSection("Config", m_logSection))
            {
                ListAppSettings(section);
                ListConnectionStrings(section);
            }
        }

        private void RegisterDI()
        {
            using (var section = new LogSection("Dependency injection", m_logSection))
            {
                section.Log("Initializing LightInject");
                DIContainer = new ServiceContainer();

                if (RegisterDIStartedCallback != null)
                {
                    RegisterDIStartedCallback(DIContainer, section);
                }

                DIContainer.Register<ApplicationDatabases>(m_createLifetime());

                section.Log("Registering services");
                RegisterServices(DIContainer);

                section.Log("Registering table data gateways");
                RegisterDapperDataGateways(DIContainer);

                section.Log("Registering inventory files");
                RegisterInventoryFiles(DIContainer);

                section.Log("Registering sheet dependencies");
                RegisterSheetDependencies(DIContainer);

                if (RegisterEndingCallback != null)
                {
                    RegisterEndingCallback(DIContainer, section);
                }
            }
        }

        private void RegisterServices(ServiceContainer container)
        {
            // MANTER A LISTA EM ORDEM ALFABÉTICA
            container.Register<IAlcadaDetalheService, AlcadaDetalheService>(m_createLifetime());
            container.Register<IAuditService, AuditService>(m_createLifetime());
            container.Register<IAutorizaPedidoService, AutorizaPedidoService>(m_createLifetime());
            container.Register<IBandeiraService, BandeiraService>(m_createLifetime());
            container.Register<ICDService, CDService>(m_createLifetime());
            container.Register<ICategoriaService, CategoriaService>(m_createLifetime());
            container.Register<ICompraCasadaService, CompraCasadaService>(m_createLifetime());
            container.Register<IDepartamentoService, DepartamentoService>(m_createLifetime());
            container.Register<IDistritoService, DistritoService>(m_createLifetime());
            container.Register<IDivisaoService, DivisaoService>(m_createLifetime());
            container.Register<IEstoqueService, EstoqueService>(m_createLifetime());
            container.Register<IFineLineService, FineLineService>(m_createLifetime());
            container.Register<IFormatoService, FormatoService>(m_createLifetime());
            container.Register<IFornecedorParametroService, FornecedorParametroService>(m_createLifetime());
            container.Register<IFornecedorService, FornecedorService>(m_createLifetime());
            container.Register<IGradeSugestaoService, GradeSugestaoService>(m_createLifetime());
            container.Register<IInventarioImportacaoService, InventarioImportacaoService>(m_createLifetime());
            container.Register<IInventarioService, InventarioService>(m_createLifetime());
            container.Register<IItemDetalheService, ItemDetalheService>(m_createLifetime());
            container.Register<IItemRelacionamentoService, ItemRelacionamentoService>(m_createLifetime());
            container.Register<ILogMensagemReturnSheetVigenteService, LogMensagemReturnSheetVigenteService>(m_createLifetime());
            container.Register<ILogMultiSourcingService, LogMultiSourcingService>(m_createLifetime());
            container.Register<ILogRelacionamentoTransferenciaService, LogRelacionamentoTransferenciaService>(m_createLifetime());
            container.Register<ILojaCdParametroService, LojaCdParametroService>(m_createLifetime());
            container.Register<ILojaService, LojaService>(m_createLifetime());
            container.Register<IMotivoMovimentacaoService, MotivoMovimentacaoService>(m_createLifetime());
            container.Register<IMotivoRevisaoCustoService, MotivoRevisaoCustoService>(m_createLifetime());
            container.Register<IMovimentacaoService, MovimentacaoService>(m_createLifetime());
            container.Register<IMultisourcingService, MultisourcingService>(m_createLifetime());
            container.Register<INotaFiscalItemService, NotaFiscalItemService>(m_createLifetime());
            container.Register<INotaFiscalItemStatusService, NotaFiscalItemStatusService>(m_createLifetime());
            container.Register<INotaFiscalService, NotaFiscalService>(m_createLifetime());
            container.Register<IOrigemCalculoService, OrigemCalculoService>(m_createLifetime());
            container.Register<IPapelService, PapelService>(m_createLifetime());
            container.Register<IParametroService, ParametroService>(m_createLifetime());
            container.Register<IParametroSistemaService, ParametroSistemaService>(m_createLifetime());
            container.Register<IPermissaoService, PermissaoService>(m_createLifetime());
            container.Register<IProcessingService, ProcessingService>(m_createLifetime());
            container.Register<IProcessoService, ProcessoService>(m_createLifetime());
            container.Register<IReabastecimentoFornecedorCDService, ReabastecimentoFornecedorCDService>(m_createLifetime());
            container.Register<IReabastecimentoItemFornecedorCDService, ReabastecimentoItemFornecedorCDService>(m_createLifetime());
            container.Register<IRegiaoAdministrativaService, RegiaoAdministrativaService>(m_createLifetime());
            container.Register<IRegiaoCompraService, RegiaoCompraService>(m_createLifetime());
            container.Register<IRegiaoService, RegiaoService>(m_createLifetime());
            container.Register<IRelacaoItemCDService, RelacaoItemCDService>(m_createLifetime());
            container.Register<IRelacaoItemLojaCDService, RelacaoItemLojaCDService>(m_createLifetime());
            container.Register<IRelacionamentoTransferenciaService, RelacionamentoTransferenciaService>(m_createLifetime());
            container.Register<IReturnSheetItemLojaService, ReturnSheetItemLojaService>(m_createLifetime());
            container.Register<IReturnSheetItemPrincipalService, ReturnSheetItemPrincipalService>(m_createLifetime());
            container.Register<IReturnSheetService, ReturnSheetService>(m_createLifetime());
            container.Register<IRevisaoCustoService, RevisaoCustoService>(m_createLifetime());
            container.Register<IRevisaoCustoService, RevisaoCustoService>(m_createLifetime());
            container.Register<IRoteiroLojaService, RoteiroLojaService>(m_createLifetime());
            container.Register<IRoteiroPedidoService, RoteiroPedidoService>(m_createLifetime());
            container.Register<IRoteiroService, RoteiroService>(m_createLifetime());
            container.Register<ISistemaService, SistemaService>(m_createLifetime());
            container.Register<IStatusItemHostService, StatusItemHostService>(m_createLifetime());
            container.Register<IStatusRevisaoCustoService, StatusRevisaoCustoService>(m_createLifetime());
            container.Register<ISubcategoriaService, SubcategoriaService>(m_createLifetime());
            container.Register<ISugestaoPedidoCDService, SugestaoPedidoCDService>(m_createLifetime());
            container.Register<ISugestaoPedidoService, SugestaoPedidoService>(m_createLifetime());
            container.Register<ISugestaoReturnSheetService, SugestaoReturnSheetService>(m_createLifetime());
            container.Register<ITipoMovimentacaoService, TipoMovimentacaoService>(m_createLifetime());
            container.Register<ITraitService, TraitService>(m_createLifetime());
            container.Register<IUsuarioService, UsuarioService>(m_createLifetime());
           

            container.Register<ISsoService>((factory) =>
            {
#if NO_WEBGUARDIAN
                return new FakeSsoService(container.GetInstance<IUsuarioService>(), container.GetInstance<IPapelService>());
#else
                return new WebGuardianSsoService(m_config.EmailDomain);
#endif
            });

            container.Register<FileVaultConfiguration>(
                (factory) =>
                {
                    // FileVault.
                    string storageRoot = ConfigurationManager.AppSettings["SGP:FileVault:StorageRoot"];

                    if (string.IsNullOrWhiteSpace(storageRoot))
                    {
                        storageRoot = m_config.AppAbsolutePath;
                    }

                    return new FileVaultConfiguration
                    {
                        StorageRoot = storageRoot,
                        Staging = Path.Combine(storageRoot, ConfigurationManager.AppSettings["SGP:FileVault:Staging"] ?? @"Resources\TempUploads")
                    };
                },
                m_createLifetime());

            container.Register<IFileVaultService, FileVaultService>(m_createLifetime());

            // Processing.
            container.Register<ProcessingConfiguration>(
                (s) =>
                {
                    string storageRoot = MakeStorageRoot("SGP:Processing:StorageRoot");
                    string strMaxAge = ConfigurationManager.AppSettings["SGP:Processing:MaxAge"] ?? "30"; // 30 dias

                    TimeSpan maxAge;
                    if (!TimeSpan.TryParse(strMaxAge, System.Globalization.CultureInfo.InvariantCulture, out maxAge))
                    {
                        maxAge = TimeSpan.FromDays(30);
                    }

                    return new ProcessingConfiguration
                    {
                        StorageRoot = storageRoot,
                        ResultsPath = Path.Combine(storageRoot, ConfigurationManager.AppSettings["SGP:Processing:Results"] ?? @"Resources\Processing\Results"),
                        InputFilesPath = Path.Combine(storageRoot, ConfigurationManager.AppSettings["SGP:Processing:InputFiles"] ?? @"Resources\Processing\InputFiles"),
                        EnableQueueing = (ConfigurationManager.AppSettings["SGP:Processing:EnableQueueing"] ?? "false").ToLowerInvariant() == "true",
                        MaxAge = maxAge
                    };
                },
            m_createLifetime());

            // Here be dragons
            // Resolve um serviço pelo seu tipo, usado pelo BackgroundProcessService
            container.Register<Func<Type, object>>(
                (s) =>
                {
                    return (serviceType) => s.TryGetInstance(serviceType);
                },
            m_createLifetime());

            container.Register<SmtpConfiguration>(
                (factory) =>
                {
                    return new SmtpConfiguration
                    {
                        Server = ConfigurationManager.AppSettings["SGP:SMTP:Server"],
                        Port = ConfigurationManager.AppSettings["SGP:SMTP:Port"].ToInt32(),
                        User = ConfigurationManager.AppSettings["SGP:SMTP:User"],
                        Password = ConfigurationManager.AppSettings["SGP:SMTP:Password"],
                        Domain = ConfigurationManager.AppSettings["SGP:SMTP:Domain"],
                        FromAddress = ConfigurationManager.AppSettings["SGP:SMTP:FromAddress"],
                        FromDisplayName = ConfigurationManager.AppSettings["SGP:SMTP:FromDisplayName"]
                    };
                },
                m_createLifetime());

            container.Register<CargaMassivaVendorPrimarioConfiguracao>(
                (factory) =>
                {
                    return new CargaMassivaVendorPrimarioConfiguracao
                    {
                        EmailsRetornoImportacao = ConfigurationManager.AppSettings["SGP:CargaMassivaVendorPrimario:EmailsRetornoImportacao"]
                            .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(email => new MailAddress(email, null))
                    };
                },
                m_createLifetime());

            container.Register<IMailClient, MailClient>(m_createLifetime());
            container.Register<IMailService, SmtpService>(m_createLifetime());
        }

        private string MakeStorageRoot(string key)
        {
            var storageRoot = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(storageRoot))
            {
                storageRoot = ConfigurationManager.AppSettings["SGP:FileVault:StorageRoot"];
            }

            if (string.IsNullOrWhiteSpace(storageRoot))
            {
                storageRoot = m_config.AppAbsolutePath;
            }

            return storageRoot;
        }

        private void RegisterDapperDataGateways(ServiceContainer container)
        {
            // MANTER A LISTA EM ORDEM ALFABÉTICA
            container.Register<IAlcadaDetalheGateway, DapperAlcadaDetalheGateway>(m_createLifetime());
            container.Register<IAlcadaDetalheGateway, DapperAlcadaDetalheGateway>(m_createLifetime());
            container.Register<IAuditGateway, DapperAuditGateway>(m_createLifetime());
            container.Register<IAutorizaPedidoGateway, DapperAutorizaPedidoGateway>(m_createLifetime());
            container.Register<IBandeiraGateway, DapperBandeiraGateway>(m_createLifetime());
            container.Register<ICDGateway, DapperCDGateway>(m_createLifetime());
            container.Register<ICategoriaGateway, DapperCategoriaGateway>(m_createLifetime());
            container.Register<ICompraCasadaGateway, DapperCompraCasadaGateway>(m_createLifetime());
            container.Register<IDepartamentoGateway, DapperDepartamentoGateway>(m_createLifetime());
            container.Register<IDistritoGateway, DapperDistritoGateway>(m_createLifetime());
            container.Register<IDivisaoGateway, DapperDivisaoGateway>(m_createLifetime());
            container.Register<IEstoqueGateway, DapperEstoqueGateway>(m_createLifetime());
            container.Register<IFechamentoFiscalGateway, DapperFechamentoFiscalGateway>(m_createLifetime());
            container.Register<IFineLineGateway, DapperFineLineGateway>(m_createLifetime());
            container.Register<IFormatoGateway, DapperFormatoGateway>(m_createLifetime());
            container.Register<IFornecedorGateway, DapperFornecedorGateway>(m_createLifetime());
            container.Register<IFornecedorParametroGateway, DapperFornecedorParametroGateway>(m_createLifetime());
            container.Register<IGradeSugestaoGateway, DapperGradeSugestaoGateway>(m_createLifetime());
            container.Register<IInventarioAgendamentoGateway, DapperInventarioAgendamentoGateway>(m_createLifetime());
            container.Register<IInventarioCriticaGateway, DapperInventarioCriticaGateway>(m_createLifetime());
            container.Register<IInventarioGateway, DapperInventarioGateway>(m_createLifetime());
            container.Register<IInventarioItemGateway, DapperInventarioItemGateway>(m_createLifetime());
            container.Register<IItemDetalheGateway, DapperItemDetalheGateway>(m_createLifetime());
            container.Register<IItemDetalheHistGateway, DapperItemDetalheHistGateway>(m_createLifetime());
            container.Register<IItemRelacionamentoGateway, DapperItemRelacionamentoGateway>(m_createLifetime());
            container.Register<ILeitorLogger, DapperImportacaoLogGateway>(m_createLifetime());
            container.Register<ILogMensagemReturnSheetVigenteGateway, DapperLogMensagemReturnSheetVigenteGateway>(m_createLifetime());
            container.Register<ILogMultiSourcingGateway, DapperLogMultiSourcingGateway>(m_createLifetime());
            container.Register<ILogRelacionamentoTransferenciaGateway, DapperLogRelacionamentoTransferenciaGateway>(m_createLifetime());
            container.Register<ILojaCdParametroGateway, DapperLojaCdParametroGateway>(m_createLifetime());
            container.Register<ILojaGateway, DapperLojaGateway>(m_createLifetime());
            container.Register<IMotivoMovimentacaoGateway, DapperMotivoMovimentacaoGateway>(m_createLifetime());
            container.Register<IMotivoRevisaoCustoGateway, DapperMotivoRevisaoCustoGateway>(m_createLifetime());
            container.Register<IMovimentacaoGateway, DapperMovimentacaoGateway>(m_createLifetime());
            container.Register<IMultisourcingGateway, DapperMultisourcingGateway>(m_createLifetime());
            container.Register<INotaFiscalGateway, DapperNotaFiscalGateway>(m_createLifetime());
            container.Register<INotaFiscalItemGateway, DapperNotaFiscalItemGateway>(m_createLifetime());
            container.Register<INotaFiscalItemStatusGateway, DapperNotaFiscalItemStatusGateway>(m_createLifetime());
            container.Register<IOrigemCalculoGateway, DapperSugestaoPedidoGateway>(m_createLifetime());
            container.Register<IPapelGateway, DapperPapelGateway>(m_createLifetime());
            container.Register<IParametroGateway, DapperParametroGateway>(m_createLifetime());
            container.Register<IParametroSistemaGateway, DapperParametroSistemaGateway>(m_createLifetime());
            container.Register<IPermissaoBandeiraGateway, DapperPermissaoBandeiraGateway>(m_createLifetime());
            container.Register<IPermissaoGateway, DapperPermissaoGateway>(m_createLifetime());
            container.Register<IPermissaoLojaGateway, DapperPermissaoLojaGateway>(m_createLifetime());
            container.Register<IProcessOrderArgumentGateway, DapperProcessOrderArgumentGateway>(m_createLifetime());
            container.Register<IProcessOrderGateway, DapperProcessOrderGateway>(m_createLifetime());
            container.Register<IProcessOrderServiceGateway, DapperProcessOrderServiceGateway>(m_createLifetime());
            container.Register<IProcessoGateway, DapperProcessoGateway>(m_createLifetime());
            container.Register<IReabastecimentoFornecedorCDGateway, DapperReabastecimentoFornecedorCDGateway>(m_createLifetime());
            container.Register<IReabastecimentoItemFornecedorCDGateway, DapperReabastecimentoItemFornecedorCDGateway>(m_createLifetime());
            container.Register<IRegiaoAdministrativaGateway, DapperRegiaoAdministrativaGateway>(m_createLifetime());
            container.Register<IRegiaoCompraGateway, DapperRegiaoCompraGateway>(m_createLifetime());
            container.Register<IRegiaoGateway, DapperRegiaoGateway>(m_createLifetime());
            container.Register<IRelacaoItemCDGateway, DapperRelacaoItemCDGateway>(m_createLifetime());
            container.Register<IRelacaoItemLojaCDGateway, DapperRelacaoItemLojaCDGateway>(m_createLifetime());
            container.Register<IRelacionamentoItemPrincipalHistGateway, DapperRelacionamentoItemPrincipalHistGateway>(m_createLifetime());
            container.Register<IRelacionamentoItemSecundarioGateway, DapperRelacionamentoItemSecundarioGateway>(m_createLifetime());
            container.Register<IRelacionamentoItemSecundarioHistGateway, DapperRelacionamentoItemSecundarioHistGateway>(m_createLifetime());
            container.Register<IRelacionamentoTransferenciaGateway, DapperRelacionamentoTransferenciaGateway>(m_createLifetime());
            container.Register<IReturnSheetGateway, DapperReturnSheetGateway>(m_createLifetime());
            container.Register<IReturnSheetItemLojaGateway, DapperReturnSheetItemLojaGateway>(m_createLifetime());
            container.Register<IReturnSheetItemPrincipalGateway, DapperReturnSheetItemPrincipalGateway>(m_createLifetime());
            container.Register<IRevisaoCustoGateway, DapperRevisaoCustoGateway>(m_createLifetime());
            container.Register<IRoteiroGateway, DapperRoteiroGateway>(m_createLifetime());
            container.Register<IRoteiroLojaGateway, DapperRoteiroLojaGateway>(m_createLifetime());
            container.Register<IRoteiroPedidoGateway, DapperRoteiroPedidoGateway>(m_createLifetime());
            container.Register<ISistemaGateway, DapperSistemaGateway>(m_createLifetime());
            container.Register<IStatusItemHostGateway, DapperStatusItemHostGateway>(m_createLifetime());
            container.Register<IStatusRevisaoCustoGateway, DapperStatusRevisaoCustoGateway>(m_createLifetime());
            container.Register<ISubcategoriaGateway, DapperSubcategoriaGateway>(m_createLifetime());
            container.Register<ISugestaoPedidoCDGateway, DapperSugestaoPedidoCDGateway>(m_createLifetime());
            container.Register<ISugestaoPedidoGateway, DapperSugestaoPedidoGateway>(m_createLifetime());
            container.Register<ISugestaoReturnSheetGateway, DapperSugestaoReturnSheetGateway>(m_createLifetime());
            container.Register<ITipoMovimentacaoGateway, DapperTipoMovimentacaoGateway>(m_createLifetime());
            container.Register<ITraitGateway, DapperTraitGateway>(m_createLifetime());
            container.Register<IUsuarioGateway, DapperUsuarioGateway>(m_createLifetime());
            container.Register<IAlcadaGateway, DapperAlcadaGateway>(m_createLifetime());
        }

        private void RegisterInventoryFiles(ServiceContainer container)
        {
            container.Register<ConfiguracaoArquivosInventario>(
                (s) =>
                {
                    int dias = 0;
                    int? diasPermanencia = null;

                    if (int.TryParse(ConfigurationManager.AppSettings["SGP:Inventario:DiasPermanenciaArquivos"], out dias))
                    {
                        diasPermanencia = dias;
                    }

                    string storageRoot = MakeStorageRoot("SGP:Inventario:StorageRoot");

                    string diretorioDownload = Path.Combine(storageRoot, ConfigurationManager.AppSettings["SGP:Inventario:CaminhoDownload"] ?? @"Resources\TempDownloadFtp");

                    return new ConfiguracaoArquivosInventario
                    {
                        StorageRoot = storageRoot,
                        CaminhoDownload = diretorioDownload,
                        ExtensaoArquivo = ConfigurationManager.AppSettings["SGP:Inventario:ExtensaoArquivo"],
                        PrefixoArquivoPipe = ConfigurationManager.AppSettings["SGP:Inventario:PrefixoArquivoPipe"],
                        PrefixoArquivoRtl = ConfigurationManager.AppSettings["SGP:Inventario:PrefixoArquivoRtl"],
                        DiasPermanenciaArquivos = diasPermanencia,
                    };
                },
                m_createLifetime());

            container.Register<ILeitorArquivoInventario, LeitorArquivoInventario>(m_createLifetime());
            container.Register<ITransferidorArquivosInventario, TransferidorArquivosInventario>(m_createLifetime());
        }

        private void RegisterSheetDependencies(ServiceContainer container)
        {
            container.Register<IExcelReader, EPPlusExcelReader>(m_createLifetime());
            container.Register<IExcelWriter, EPPlusExcelWriter>(m_createLifetime());
            container.Register<IRelacaoItemLojaCDVinculoExcelDataTranslator, RelacaoItemLojaCDVinculoExcelDataTranslator>(m_createLifetime());
            container.Register<IRelacaoItemLojaCDDesvinculoExcelDataTranslator, RelacaoItemLojaCDDesvinculoExcelDataTranslator>(m_createLifetime());
            container.Register<IMultisourcingExcelDataTranslator, MultisourcingExcelDataTranslator>(m_createLifetime());
            container.Register<MultisourcingExcelExporter, MultisourcingExcelExporter>(m_createLifetime());
            container.Register<MultisourcingExcelImporter, MultisourcingExcelImporter>(m_createLifetime());
            container.Register<RelacaoItemLojaCDVinculoExcelImporter, RelacaoItemLojaCDVinculoExcelImporter>(m_createLifetime());
            container.Register<RelacaoItemLojaCDDesvinculoExcelImporter, RelacaoItemLojaCDDesvinculoExcelImporter>(m_createLifetime());
            container.Register<SugestaoPedidoCDExcelExporter, SugestaoPedidoCDExcelExporter>(m_createLifetime());
            container.Register<SugestaoReturnSheetRAExporter, SugestaoReturnSheetRAExporter>(new PerScopeLifetime());
            container.Register<CompraCasadaExcelExporter, CompraCasadaExcelExporter>(m_createLifetime());
            container.Register<RelacaoItemLojaCDExcelExporter, RelacaoItemLojaCDExcelExporter>(m_createLifetime());
        }
        #endregion
    }
}