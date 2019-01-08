using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.IO.Helpers;
using Walmart.Sgp.Infrastructure.IO.Importing.Inventario.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Serviço de IO que localiza e transfere arquivos de inventário do servidor FTP.
    /// </summary>
    public class TransferidorArquivosInventario : ITransferidorArquivosInventario
    {
        #region Fields
        private ILeitorLogger m_logger;
        private IParametroService m_parametroService;
        private ConfiguracaoArquivosInventario m_config;
        private IFileVaultService m_fileVaultService;
        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TransferidorArquivosInventario"/>.
        /// </summary>
        /// <param name="logger">O serviço de log.</param>
        /// <param name="parametroService">O serviço de parâmetros.</param>
        /// <param name="configuracaoArquivosInventario">A configuração de arquivos de inventário.</param>
        /// <param name="fileVaultService">O serviço de FileVault.</param>
        public TransferidorArquivosInventario(ILeitorLogger logger, IParametroService parametroService, ConfiguracaoArquivosInventario configuracaoArquivosInventario, IFileVaultService fileVaultService)
        {
            m_logger = logger;
            m_parametroService = parametroService;
            m_config = configuracaoArquivosInventario;
            m_fileVaultService = fileVaultService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transfere arquivos de inventário do servidor FTP para um diretório local.
        /// </summary>
        /// <param name="codigoSistema">O código da estrutura mercadológica.</param>
        /// <param name="codigoDepartamento">O código do departamento.</param>
        /// <param name="dataInventario">A data do inventário.</param>
        /// <param name="tipoOrigem">O tipo de origem.</param>
        /// <param name="tipoProcesso">O tipo de processo.</param>
        /// <param name="loja">A loja.</param>
        /// <returns>A lista de arquivos (locais) que foram transferidos.</returns>
        public IEnumerable<string> ObterArquivosViaFtp(int codigoSistema, int? codigoDepartamento, DateTime dataInventario, TipoOrigemImportacao tipoOrigem, TipoProcessoImportacao tipoProcesso, Loja loja)
        {
            SpecService.Assert(
                new
                {
                    m_config.ArquivoPipeAtacado,
                    m_config.ArquivoPipeVarejo,
                    m_config.CaminhoDownload,
                    m_config.DiasPermanenciaArquivos,
                    m_config.DiretorioBackup,
                    m_config.DiretorioDescompactados,
                    m_config.ExtensaoArquivo,
                    m_config.PrefixoArquivoPipe,
                    m_config.PrefixoArquivoRtl,
                    cdDepartamento = codigoDepartamento,
                    cdSistema = codigoSistema,
                    dataInventario,
                    tipoOrigem,
                    tipoProcesso
                },
            new AllMustBeInformedSpec());

            IEnumerable<string> arquivosProntosParaProcessar;

            try
            {
                m_logger.InserirLogProcessamento(Texts.ImportLogDownloadFTP, Texts.ImportLogInicioDownloadFtp.With(tipoProcesso.Description, tipoOrigem.Description, loja.cdLoja));

                string servidorSmartDiretorio = loja.dsServidorSmartDiretorio;
                string servidorSmartEndereco = loja.dsServidorSmartEndereco;
                string servidorSmartNomeUsuario = loja.dsServidorSmartNomeUsuario;
                string servidorSmartSenha = loja.dsServidorSmartSenha;

                int? qtdDiasArquivoInventarioAtacado = 0, qtdDiasArquivoInventarioVarejo = 0;

                Parametro parametro = m_parametroService.Obter();

                if (!new AllMustBeInformedSpec().IsSatisfiedBy(new { loja.dsServidorSmartDiretorio, loja.dsServidorSmartEndereco, loja.dsServidorSmartNomeUsuario, loja.dsServidorSmartSenha }))
                {
                    servidorSmartDiretorio = parametro.dsServidorSmartDiretorio;
                    servidorSmartEndereco = parametro.dsServidorSmartEndereco.ToUpper(CultureInfo.InvariantCulture).Replace("XXXX", loja.cdLoja.ToString("D4", CultureInfo.InvariantCulture)); // Walmart.SGP.Config.BL.cs linha 116
                    servidorSmartNomeUsuario = parametro.dsServidorSmartNomeUsuario;
                    servidorSmartSenha = parametro.dsServidorSmartSenha;

                    qtdDiasArquivoInventarioAtacado = parametro.qtdDiasArquivoInventarioAtacado;
                    qtdDiasArquivoInventarioVarejo = parametro.qtdDiasArquivoInventarioVarejo;
                }

                string diretorioDownload;
                string diretorioExtracao;
                string diretorioBackup;
                DiretoriosParaLoja(loja.cdLoja, out diretorioDownload, out diretorioExtracao, out diretorioBackup);

                int? qtdDias = codigoSistema == 1 ? qtdDiasArquivoInventarioVarejo : qtdDiasArquivoInventarioAtacado;

                NetworkCredential networkCredential = new NetworkCredential(servidorSmartNomeUsuario, servidorSmartSenha);
                Uri remoteUri = new Uri("ftp://{0}{1}{2}".With(ObterEnderecoIpServidor(servidorSmartEndereco), servidorSmartDiretorio, servidorSmartDiretorio.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? string.Empty : "/"));

                IEnumerable<string> arquivosLocais;
                if (parametro.TpArquivoInventario == TipoFormatoArquivoInventario.Rtl) 
                {
                    arquivosLocais = RealizarTransferenciaRtl(codigoSistema, codigoDepartamento, dataInventario, remoteUri, networkCredential, diretorioDownload, diretorioExtracao, qtdDias);
                }
                else
                {
                    arquivosLocais = RealizarTransferenciaPipe(codigoSistema, codigoDepartamento, dataInventario, remoteUri, networkCredential, diretorioDownload, diretorioExtracao, qtdDias);
                }

                arquivosProntosParaProcessar = PrepararArquivosTransferidos(arquivosLocais, diretorioExtracao, diretorioBackup);

                m_logger.InserirLogProcessamento(Texts.ImportLogDownloadFTP, Texts.ImportLogFinalDownloadFtp.With(tipoProcesso.Description, tipoOrigem.Description, loja.cdLoja));

                return arquivosProntosParaProcessar;
            }
            catch (Exception ex)
            {
                m_logger.InserirLogErroProcessamento(Texts.ImportLogDownloadFTP, Texts.ImportLogMensagemErroDownloadFtp.With(ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Remove arquivos antigos de backup.
        /// </summary>
        /// <param name="codigoLoja">O código da loja.</param>
        /// <param name="data">A data de referência.</param>
        public void RemoverBackupsAntigos(int codigoLoja, DateTime data)
        {
            string diretorioDownload;
            string diretorioExtracao;
            string diretorioBackup;
            DiretoriosParaLoja(codigoLoja, out diretorioDownload, out diretorioExtracao, out diretorioBackup);

            DirectoryInfo dirE = new DirectoryInfo(diretorioExtracao);

            foreach (FileInfo file in dirE.GetFiles())
            {
                // Walmart.SGP.ArchiveImport.BL.cs linha 391
                if (file.CreationTime < data.AddDays(-5))
                {
                    File.Delete(file.FullName);
                }
            }

            if (!m_config.DiasPermanenciaArquivos.HasValue)
            {
                return;
            }

            diretorioBackup = Path.GetDirectoryName(diretorioBackup);

            if (!Directory.Exists(diretorioBackup))
            {
                Directory.CreateDirectory(diretorioBackup);
            }

            DirectoryInfo diretorios = new DirectoryInfo(diretorioBackup);

            foreach (DirectoryInfo dirB in diretorios.GetDirectories())
            {
                if (dirB.CreationTime < DateTime.Now.AddDays(-m_config.DiasPermanenciaArquivos.Value))
                {
                    Directory.Delete(dirB.FullName, true);
                }
            }
        }

        /// <summary>
        /// Exclui os arquivos que não foram usados.
        /// </summary>
        /// <param name="codigoLoja">O código da loja.</param>
        /// <param name="arquivosNaoProcessados">Arquivos que não foram usados.</param>
        public void ExcluirArquivosNaoProcessados(int codigoLoja, IEnumerable<ArquivoInventario> arquivosNaoProcessados)
        {
            string diretorioDownload;
            string diretorioExtracao;
            string diretorioBackup;
            DiretoriosParaLoja(codigoLoja, out diretorioDownload, out diretorioExtracao, out diretorioBackup);

            foreach (var arquivo in arquivosNaoProcessados)
            {
                string nomeArquivo = Path.Combine(diretorioExtracao, arquivo.NomeArquivo);

                if (File.Exists(nomeArquivo))
                {
                    File.Delete(nomeArquivo);
                }
            }
        }

        /// <summary>
        /// Obtém arquivos do serviço FileVault e armazena em um diretório local.
        /// </summary>
        /// <param name="tickets">Os tickets.</param>
        /// <param name="loja">A loja.</param>
        /// <returns>A lista de arquivos (locais) que foram armazenados.</returns>
        public IEnumerable<string> CopiarArquivosParaImportar(IEnumerable<FileVaultTicket> tickets, Loja loja)
        {
            string diretorioDownload, diretorioExtracao, diretorioBackup;

            DiretoriosParaLoja(loja.cdLoja, out diretorioDownload, out diretorioExtracao, out diretorioBackup);

            return tickets.Select(ticket => m_fileVaultService.SavePermanently(ticket, diretorioExtracao)).ToArray();
        }

        private static string ObterEnderecoIpServidor(string servidorSmartEndereco)
        {
            try
            {
                var addresses = Dns.GetHostEntry(servidorSmartEndereco).AddressList;

                if (!addresses.Any())
                {
                    throw new InvalidOperationException(Texts.CouldNotResolveDnsHost);
                }

                return addresses.First().ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Texts.ErrorResolvingHostName.With(servidorSmartEndereco, ex.Message), ex);
            }
        }

        private static void PrepararArquivoBackup(string caminhoBackup, string arquivoLocal)
        {
            string arquivoBackup = Path.Combine(caminhoBackup, Path.GetFileName(arquivoLocal));

            if (!Directory.Exists(Path.GetDirectoryName(arquivoBackup)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(arquivoBackup));
            }

            if (File.Exists(arquivoBackup))
            {
                File.Delete(arquivoBackup);
            }

            File.Move(arquivoLocal, arquivoBackup);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters")]
        private void DiretoriosParaLoja(int codigoLoja, out string diretorioDownload, out string diretorioExtracao, out string diretorioBackup)
        {
            diretorioDownload = Path.Combine(m_config.CaminhoDownload, codigoLoja.ToString(CultureInfo.InvariantCulture));
            diretorioExtracao = Path.Combine(diretorioDownload, m_config.DiretorioDescompactados);
            diretorioBackup = Path.Combine(diretorioDownload, m_config.DiretorioBackup, "{0:yyyyMMddHHmmss}".With(DateTime.Now));
        }

        private IEnumerable<string> RealizarTransferenciaPipe(int codigoSistema, int? codigoDepartamento, DateTime dataInventario, Uri remoteUri, NetworkCredential networkCredential, string caminhoDownload, string caminhoExtracao, int? qtdDias)
        {
            var dataSpec = new DataArquivoInventarioDeveSerValidaSpec(dataInventario, qtdDias.Value, (codigoSistema == 1 ? "departamento {0}" : "categoria {0}").With(codigoDepartamento));

            string tipoArquivo = codigoSistema == 1 ? m_config.ArquivoPipeVarejo : m_config.ArquivoPipeAtacado;

            // No legado, buscava todos com PrefixoArquivoPipe e depois pegava apenas aqueles com o codigoDepto
            // Aqui já filtra pelo codigo depto
            string prefixoArquivo = m_config.PrefixoArquivoPipe + (codigoDepartamento ?? 0).ToString("D4", CultureInfo.InvariantCulture);  // Walmart.SGP.ArchiveImport.BL.cs linha 264

            var maiorArquivoRemoto = DownloadFtpHelper.EncontrarArquivos(remoteUri, networkCredential, m_config.PrefixoArquivoPipe, false)
                .Where(a => dataSpec.IsSatisfiedBy(a))
                .IsSatisfiedBy(new ArquivoPipeDeveSerValidoSpec(prefixoArquivo, tipoArquivo))
                .OrderByDescending(e => e.TamanhoArquivo)
                .FirstOrDefault();

            if (null == maiorArquivoRemoto || File.Exists(Path.Combine(caminhoExtracao, maiorArquivoRemoto.NomeArquivo.Replace(m_config.ExtensaoArquivo, string.Empty))))
            {
                return new string[0];
            }                

            return DownloadFtpHelper.TransferirArquivos(caminhoDownload, networkCredential, maiorArquivoRemoto.Uri);
        }

        private IEnumerable<string> RealizarTransferenciaRtl(int codigoSistema, int? codigoDepartamento, DateTime dataInventario, Uri remoteUri, NetworkCredential networkCredential, string caminhoDownload, string caminhoExtracao, int? qtdDias)
        {
            var dataSpec = new DataArquivoInventarioDeveSerValidaSpec(dataInventario, qtdDias.Value, (codigoSistema == 1 ? "departamento {0}" : "categoria {0}").With(codigoDepartamento));

            var arquivosRemotos = DownloadFtpHelper.EncontrarArquivos(remoteUri, networkCredential, m_config.PrefixoArquivoRtl, false)
                .Where(a => dataSpec.IsSatisfiedBy(a))
                .IsSatisfiedBy(new ArquivoRtlDeveSerValidoSpec(caminhoExtracao, m_config.ExtensaoArquivo));

            return DownloadFtpHelper.TransferirArquivos(caminhoDownload, networkCredential, arquivosRemotos.Select(r => r.Uri).ToArray());
        }

        private IEnumerable<string> PrepararArquivosTransferidos(IEnumerable<string> arquivosLocais, string caminhoExtracao, string caminhoBackup)
        {
            List<string> arquivosProntosParaProcessar = new List<string>();

            foreach (var arquivoLocal in arquivosLocais)
            {
                if (File.Exists(arquivoLocal))
                {
                    string arquivoDescompactado = PrepararArquivoDescompactado(caminhoExtracao, arquivoLocal);
                    
                    arquivosProntosParaProcessar.Add(arquivoDescompactado);
                    
                    PrepararArquivoBackup(caminhoBackup, arquivoLocal);
                }
            }

            return arquivosProntosParaProcessar;
        }

        private string PrepararArquivoDescompactado(string caminhoExtracao, string arquivoLocal)
        {
            string arquivoDescompactado = Path.Combine(caminhoExtracao, Path.GetFileName(arquivoLocal));

            if (!Directory.Exists(Path.GetDirectoryName(arquivoDescompactado)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(arquivoDescompactado));
            }

            if (Path.GetExtension(arquivoLocal).ToUpper(CultureInfo.InvariantCulture) == m_config.ExtensaoArquivo)
            {
                arquivoDescompactado = Path.Combine(caminhoExtracao, Path.GetFileNameWithoutExtension(arquivoLocal));

                SGPZCompressionHelper.Extrair(arquivoLocal, arquivoDescompactado);
            }
            else
            {
                if (File.Exists(arquivoDescompactado))
                {
                    File.Delete(arquivoDescompactado);
                }

                File.Copy(arquivoLocal, arquivoDescompactado);
            }

            return arquivoDescompactado;
        }

        #endregion
    }
}
