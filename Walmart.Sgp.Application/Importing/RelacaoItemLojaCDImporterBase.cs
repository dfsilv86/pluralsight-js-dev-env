using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Net;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Application.Importing
{
    /// <summary>
    /// Classe base de importação massiva.
    /// </summary>
    public class RelacaoItemLojaCDImporterBase
    {
        private static ColumnMetadata s_configuracaoColunaRelatorio;

        private CargaMassivaVendorPrimarioConfiguracao m_configuracao;

        private IExcelWriter m_excelWriter;

        private IFileVaultService m_fileVaultService;
        private IMailService m_mailService;

        /// <summary>
        /// Seta os parametros.
        /// </summary>
        /// <param name="excelWriter">O escritor de excel.</param>
        /// <param name="mailService">O service de email.</param>
        /// <param name="configuracao">A configuração da importação.</param>
        /// <param name="fileVaultService">O servico de filevault.</param>
        /// <param name="colunaMetadata">Informação da coluna de metadados.</param>
        public void SetarParametros(IExcelWriter excelWriter, IMailService mailService, CargaMassivaVendorPrimarioConfiguracao configuracao, IFileVaultService fileVaultService, ColumnMetadata colunaMetadata)
        {
            m_excelWriter = excelWriter;
            m_fileVaultService = fileVaultService;
            m_mailService = mailService;
            m_configuracao = configuracao;
            s_configuracaoColunaRelatorio = colunaMetadata;
        }

        /// <summary>
        /// Valida as colunas do XLSX.
        /// </summary>
        /// <param name="readerResult">O excelReader com o resultado.</param>
        /// <param name="vinculos">Os vinculos.</param>
        protected static void ValidarColunas(ExcelReaderResult readerResult, IEnumerable<RelacaoItemLojaCDVinculo> vinculos)
        {
            var columnNotFoundSpec = new ColumnNotFoundSpec();
            var specResult = columnNotFoundSpec.IsSatisfiedBy(readerResult.ColumnsNotFound);

            if (!specResult.Satisfied)
            {
                vinculos.ToList().ForEach(v => v.NotSatisfiedSpecReasons.Add(specResult.Reason));
            }
        }

        /// <summary>
        /// Armazena um resultado em FileVault.
        /// </summary>
        /// <param name="stream">O stream.</param>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="vinculos">Os vinculos.</param>
        /// <param name="sucesso">Se teve sucesso na validação de RN.</param>
        /// <returns>Ticket filevault.</returns>
        protected FileVaultTicket StoreResult(Stream stream, FileVaultTicket arquivo, IEnumerable<RelacaoItemLojaCDVinculo> vinculos, bool sucesso)
        {
            var streamResult = CriarRelatorioImportacao(stream, vinculos, sucesso);
            var file = new IntermediateStreamFile(arquivo.FileName, streamResult);
            var ticket = m_fileVaultService.Store(file);

            streamResult.Position = 0;

            m_mailService.Send(
                m_configuracao.EmailsRetornoImportacao,
                m_configuracao.Desvinculando ? Texts.ProcessImportingUnlinkVendorPrimarioFinalizedSuject : Texts.ProcessImportingVendorPrimarioFinalizedSuject,
                Texts.ProcessImportingVendorPrimarioFinalizedBody.With(arquivo.FileName),
                new[] { new Attachment(streamResult, arquivo.FileName) });

            return ticket;
        }

        private static string FormatarMensagemStatus(IEnumerable<string> mensagens, bool sucesso)
        {
            if (mensagens.Count() == 0)
            {
                return sucesso ? "OK" : "OK, porém não importado por problema em outra linha";
            }

            return "NOK - " + string.Join("; ", mensagens);
        }

        private Stream CriarRelatorioImportacao(Stream stream, IEnumerable<RelacaoItemLojaCDVinculo> vinculos, bool sucesso)
        {
            var rows = vinculos.Select(m => new Row
            {
                Index = m.RowIndex,
                Columns = new[] 
                { 
                    new Column 
                    { 
                        Value = FormatarMensagemStatus(m.NotSatisfiedSpecReasons, sucesso), 
                        Metadata = s_configuracaoColunaRelatorio 
                    }
                }
            });

            return m_excelWriter.Write(stream, rows);
        }
    }
}
