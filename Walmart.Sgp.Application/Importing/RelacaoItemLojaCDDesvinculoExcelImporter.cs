using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemSaida;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Net;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Application.Importing
{
    /// <summary>
    /// Classe responsável pela importação de desvinculos de RelacaoItemLojaCD.
    /// </summary>
    public class RelacaoItemLojaCDDesvinculoExcelImporter : RelacaoItemLojaCDImporterBase
    {
        private static ColumnMetadata[] s_configuracaoColunasImportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = "LOJA", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 2, Name = "ITEM CONTROLE DE ESTOQUE", ColumnType = typeof(long), Length = 18 }
        };

        private ColumnMetadata m_configuracaoColunaRelatorioDesvinculo = new ColumnMetadata { Index = 3, Name = "STATUS" };

        private CargaMassivaVendorPrimarioConfiguracao m_configuracao;

        private IExcelWriter m_excelWriter;
        private IExcelReader m_excelReader;
        private IRelacaoItemLojaCDDesvinculoExcelDataTranslator m_relacaoItemLojaCDDesvinculoExcelDataTranslator;
        private IRelacaoItemLojaCDService m_mainService;
        private IFileVaultService m_fileVaultService;
        private IMailService m_mailService;

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacaoItemLojaCDDesvinculoExcelImporter"/>.
        /// </summary>
        /// <param name="configuracao">A configuração da importação.</param>
        /// <param name="mailService">O service de email.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        /// <param name="excelReader">O leitor de excel.</param>
        /// <param name="relacaoItemLojaCDDesvinculoExcelDataTranslator">O tradutor de excel para RelacaoItemLojaCDVinculo.</param>
        /// <param name="mainService">O main service.</param>
        /// <param name="fileVaultService">O service do FileVault.</param>
        public RelacaoItemLojaCDDesvinculoExcelImporter(CargaMassivaVendorPrimarioConfiguracao configuracao, IMailService mailService, IExcelWriter excelWriter, IExcelReader excelReader, IRelacaoItemLojaCDDesvinculoExcelDataTranslator relacaoItemLojaCDDesvinculoExcelDataTranslator, IRelacaoItemLojaCDService mainService, IFileVaultService fileVaultService)
        {
            m_excelWriter = excelWriter;
            m_excelReader = excelReader;
            m_relacaoItemLojaCDDesvinculoExcelDataTranslator = relacaoItemLojaCDDesvinculoExcelDataTranslator;
            m_mainService = mainService;
            m_fileVaultService = fileVaultService;
            m_mailService = mailService;
            m_configuracao = configuracao;
        }
        #endregion

        /// <summary>
        /// Realiza a importação de desvinculos RelacaoItemLojaCD.
        /// </summary>
        /// <param name="model">Parametros para a importação.</param>
        /// <returns>O retorno da importação.</returns>
        [ServiceProcess("ImpDesvincularItemLojaCD", MaxGlobal = 1)]
        public FileVaultTicket ImportarDesvincular(ImportarRelacaoItemLojaCDRequest model)
        {
            m_configuracao.Desvinculando = true;
            SetarParametros(m_excelWriter, m_mailService, m_configuracao, m_fileVaultService, m_configuracaoColunaRelatorioDesvinculo);

            var streamArquivoDesvinculo = m_fileVaultService.Retrieve(model.Arquivos.FirstOrDefault());
            var readerResultDesvinculo = m_excelReader.Read(streamArquivoDesvinculo, s_configuracaoColunasImportacao);
            var desvinculos = m_relacaoItemLojaCDDesvinculoExcelDataTranslator.Translate(readerResultDesvinculo.Rows);

            ValidarColunas(readerResultDesvinculo, desvinculos);

            // Validações de colunas, verifica se os tipos de dados estão OK
            if (desvinculos.Any(m => !m.IsValid))
            {
                return StoreResult(streamArquivoDesvinculo, model.Arquivos.FirstOrDefault(), desvinculos, false);
            }

            // Validações de dominio RNs
            var result = ValidaRNs(model, desvinculos, streamArquivoDesvinculo);

            m_mainService.Desvincular(desvinculos.Where(d => d.IsValid), model.IdUsuario, model.CdSistema);

            return result;
        }

        private FileVaultTicket ValidaRNs(ImportarRelacaoItemLojaCDRequest model, IEnumerable<RelacaoItemLojaCDVinculo> desvinculos, Stream streamArquivo)
        {
            new VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpec(m_mainService.ObterPorVinculo, model.CdSistema)
                .IsSatisfiedBy(desvinculos);

            return StoreResult(streamArquivo, model.Arquivos.FirstOrDefault(), desvinculos, true);
        }
    }
}
