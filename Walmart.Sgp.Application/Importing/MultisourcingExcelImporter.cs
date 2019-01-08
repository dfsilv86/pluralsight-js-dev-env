using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Application.Importing
{
    /// <summary>
    /// Responsável pela importação de multisourcing.
    /// </summary>
    public class MultisourcingExcelImporter
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasImportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = "ITEM CONTROLE ESTOQUE", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 3, Name = "ITEM DE ENTRADA", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 5, Name = "VENDOR 9 DIGITOS", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 8, Name = "CD", ColumnType = typeof(int), Length = 9 },
            new ColumnMetadata { Index = 11, Name = "PERCENTUAL", ColumnType = typeof(int), Length = 3, IgnoreEmpty = true }
        };

        private static ColumnMetadata s_configuracaoColunaRelatorio = new ColumnMetadata { Index = 12, Name = "STATUS" };

        private IMultisourcingService m_mainService;
        private IItemDetalheService m_itemDetalheService;
        private ICDService m_cdService;
        private IFornecedorService m_fornecedorService;
        private IExcelReader m_excelReader;
        private IExcelWriter m_excelWriter;
        private IMultisourcingExcelDataTranslator m_multisourcingExcelDataTranslator;
        private IFileVaultService m_fileVaultService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingExcelImporter"/>.
        /// </summary>
        /// <param name="mainService">O service de multisourcing.</param>
        /// <param name="itemDetalheService">O service item detalhe.</param>
        /// <param name="cdService">O service de CD.</param>
        /// <param name="fornecedorService">O service de Fornecedor.</param>
        /// <param name="excelReader">O leitor de excel.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        /// <param name="multisourcingExcelDataTranslator">O tradutor de excel para multisourcing.</param>
        /// <param name="fileVaultService">O service de FileVault.</param>
        public MultisourcingExcelImporter(IMultisourcingService mainService, IItemDetalheService itemDetalheService, ICDService cdService, IFornecedorService fornecedorService, IExcelReader excelReader, IExcelWriter excelWriter, IMultisourcingExcelDataTranslator multisourcingExcelDataTranslator, IFileVaultService fileVaultService)
        {
            m_mainService = mainService;
            m_itemDetalheService = itemDetalheService;
            m_cdService = cdService;
            m_fornecedorService = fornecedorService;
            m_excelReader = excelReader;
            m_excelWriter = excelWriter;
            m_multisourcingExcelDataTranslator = multisourcingExcelDataTranslator;
            m_fileVaultService = fileVaultService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a importação massiva de multisourcing através de um arquivo excel.
        /// </summary>
        /// <param name="model">A requisição da importação.</param>
        /// <returns>FileVaultTicket com o resultado da importação.</returns>
        public FileVaultTicket ImportarMultisourcing(ImportarMultisourcingRequest model)
        {
            var streamArquivo = m_fileVaultService.Retrieve(model.Arquivos.FirstOrDefault());
            var readerResult = m_excelReader.Read(streamArquivo, s_configuracaoColunasImportacao);

            SpecService.Assert(readerResult.ColumnsNotFound, new ColumnNotFoundSpec());

            var multisourcings = m_multisourcingExcelDataTranslator.Translate(readerResult.Rows);

            if (multisourcings.Any(m => !m.IsValid))
            {
                return StoreResult(streamArquivo, model.Arquivos.FirstOrDefault(), multisourcings);
            }

            var result = ValidarRNs(model, multisourcings, streamArquivo);

            if (result == null)
            {
                m_mainService.SalvarMultisourcings(multisourcings, model.IdUsuario, model.CdSistema);
            }

            return result;
        }

        private static string FormatarMensagemStatus(IEnumerable<string> mensagens)
        {
            if (mensagens.Count() == 0)
            {
                return "OK, porém não importado por problema em outra linha";
            }

            return "NOK - " + string.Join("; ", mensagens);
        }

        private static void ValidarPercentuais(IEnumerable<Multisourcing> multisourcings)
        {
            new MultisourcingPercentualDevePossuirValorSpec()
                .IsSatisfiedBy(multisourcings);

            new PercentualItemSaidaCDSpec()
                .IsSatisfiedBy(multisourcings);

            new MultisourcingDevePossuirPercentualInferiorACemSpec()
                .IsSatisfiedBy(multisourcings);

            new MultisourcingDevePossuirPercentualMultiploCincoSpec()
                .IsSatisfiedBy(multisourcings);
        }

        private FileVaultTicket ValidarRNs(ImportarMultisourcingRequest model, IEnumerable<Multisourcing> multisourcings, Stream streamArquivo)
        {
            ValidarPercentuais(multisourcings);

            new MultisourcingDevePossuirApenasItensDiferentesSpec()
                .IsSatisfiedBy(multisourcings);

            ValidarItens(model, multisourcings);

            new MultisourcingDevePossuirFornecedorValidoSpec(m => m_fornecedorService.ObterAtivoPorCodigoESistemaComProjecao(m.Vendor9Digitos, model.CdSistema))
                .IsSatisfiedBy(multisourcings);

            new MultisourcingPossuiCanalValidoSpec()
                .IsSatisfiedBy(multisourcings);

            new MultisourcingDevePossuirSARFornecedorValidoSpec()
                .IsSatisfiedBy(multisourcings);

            new ItemEntradaPertenceFornecedorSpec(m => m_itemDetalheService.ItemDetalhePertenceFornecedor(m.CdItemDetalheEntrada, m.Vendor9Digitos, model.CdSistema))
                .IsSatisfiedBy(multisourcings);

            new ItemEntradaPertenceItemSaidaSpec(m => m_itemDetalheService.ItemDetalheEntradaPertenceItemDetalheSaida(m.CdItemDetalheEntrada, m.CdItemDetalheSaida, model.CdSistema))
                .IsSatisfiedBy(multisourcings);

            AplicarValidacoesCD(model, multisourcings);

            new ItemEntradaModalidadeXDockSpec(
                m => m_itemDetalheService.ObterValorTipoReabastecimento(m.ItemDetalheEntrada.IDItemDetalhe, m.IDCD))
                .IsSatisfiedBy(multisourcings);

            new MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpec(m => m_itemDetalheService.EstaVinculadoCompraCasada((int)m, model.CdSistema))
                .IsSatisfiedBy(multisourcings);

            AplicarValidacoesVendor(model, multisourcings);

            if (multisourcings.Any(m => !m.IsValid))
            {
                return StoreResult(streamArquivo, model.Arquivos.FirstOrDefault(), multisourcings);
            }

            return null;
        }

        private void ValidarItens(ImportarMultisourcingRequest model, IEnumerable<Multisourcing> multisourcings)
        {
            new MultisourcingDevePossuirItemSaidaValidoSpec(m => m_itemDetalheService.ObterPorCodigoESistemaComProjecao(m.CdItemDetalheSaida, model.CdSistema))
                .IsSatisfiedBy(multisourcings);

            new MultisourcingDevePossuirItemEntradaValidoSpec(m => m_itemDetalheService.ObterPorCodigoESistemaComProjecao(m.CdItemDetalheEntrada, model.CdSistema))
                .IsSatisfiedBy(multisourcings);
        }

        private void AplicarValidacoesCD(ImportarMultisourcingRequest model, IEnumerable<Multisourcing> multisourcings)
        {
            new MultisourcingDevePossuirCDExistenteSpec(cdCD => m_cdService.ObterIdCDPorCodigo(cdCD, model.CdSistema))
                .IsSatisfiedBy(multisourcings);

            new ItemSaidaPertenceCDSpec(m => m_itemDetalheService.ItemDetalheSaidaPertenceCD(m.CdItemDetalheSaida, m.CD, model.CdSistema))
                .IsSatisfiedBy(multisourcings);
        }
       
        private void AplicarValidacoesVendor(ImportarMultisourcingRequest model, IEnumerable<Multisourcing> multisourcings)
        {
            new MultisourcingNaoDevePossuirVendorRepetidoSpec(m_itemDetalheService.ObterPorItemESistema, model.CdSistema)
                .IsSatisfiedBy(multisourcings);

            new MultisourcingNaoDevePossuirVendorWalmartSpec(m_fornecedorService.VerificaVendorWalmart)
                .IsSatisfiedBy(multisourcings);
        }

        private FileVaultTicket StoreResult(Stream stream, FileVaultTicket arquivo, IEnumerable<Multisourcing> multisourcings)
        {
            var streamResult = CriarRelatorioImportacao(stream, multisourcings);
            var file = new IntermediateStreamFile(arquivo.FileName, streamResult);
            var ticket = m_fileVaultService.Store(file);

            streamResult.Position = 0;

            return ticket;
        }

        private Stream CriarRelatorioImportacao(Stream stream, IEnumerable<Multisourcing> multisourcings)
        {
            var rows = multisourcings.Select(m => new Row
            {
                Index = m.RowIndex,
                Columns = new[] 
                { 
                    new Column 
                    { 
                        Value = FormatarMensagemStatus(m.NotSatisfiedSpecReasons), 
                        Metadata = s_configuracaoColunaRelatorio 
                    }
                }
            });

            return m_excelWriter.Write(stream, rows);
        }
        #endregion
    }
}
