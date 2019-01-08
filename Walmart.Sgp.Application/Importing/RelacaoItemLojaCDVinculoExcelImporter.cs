using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.CompraCasada;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemSaida;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Net;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Application.Importing
{
    /// <summary>
    /// Classe responsável pela importação de vinculos de RelacaoItemLojaCD.
    /// </summary>
    public class RelacaoItemLojaCDVinculoExcelImporter : RelacaoItemLojaCDImporterBase
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasImportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = "CD", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 2, Name = "LOJA", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 5, Name = "ITEM CONTROLE DE ESTOQUE", ColumnType = typeof(long), Length = 18 },
            new ColumnMetadata { Index = 7, Name = "ITEM DE ENTRADA", ColumnType = typeof(long), Length = 18 }
        };

        private CargaMassivaVendorPrimarioConfiguracao m_configuracao;

        private ColumnMetadata m_configuracaoColunaRelatorio = new ColumnMetadata { Index = 10, Name = "STATUS" };

        private IFileVaultService m_fileVaultService;
        private IExcelReader m_excelReader;
        private IExcelWriter m_excelWriter;
        private IRelacaoItemLojaCDVinculoExcelDataTranslator m_relacaoItemLojaCDVinculoExcelDataTranslator;
        private IRelacaoItemLojaCDService m_mainService;
        private IItemDetalheService m_itemDetalheService;
        private IFornecedorParametroService m_fornecedorParametroService;
        private ITraitService m_traitService;
        private IMailService m_mailService;
        private ICompraCasadaService m_compraCasadaService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacaoItemLojaCDVinculoExcelImporter"/>.
        /// </summary>
        /// <param name="configuracao">A configuração da importação.</param>
        /// <param name="excelReader">O leitor de excel.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        /// <param name="relacaoItemLojaCDVinculoExcelDataTranslator">O tradutor de excel para RelacaoItemLojaCDVinculo.</param>
        /// <param name="fileVaultService">O servico de filevault.</param>
        /// <param name="itemDetalheService">O service de ItemDetalhe.</param>
        /// <param name="fornecedorParametroService">O service de FornecedorParametro.</param>
        /// <param name="traitService">O service de Trait.</param>
        /// <param name="relacaoItemLojaCDService">O service de RelacaoItemLojaCD.</param>
        /// <param name="compraCasadaService">O service de CompraCasada.</param>
        /// <param name="mailService">O service de envio de e-mails.</param>
        public RelacaoItemLojaCDVinculoExcelImporter(CargaMassivaVendorPrimarioConfiguracao configuracao, IExcelReader excelReader, IExcelWriter excelWriter, IRelacaoItemLojaCDVinculoExcelDataTranslator relacaoItemLojaCDVinculoExcelDataTranslator, IFileVaultService fileVaultService, IItemDetalheService itemDetalheService, IFornecedorParametroService fornecedorParametroService, ITraitService traitService, IRelacaoItemLojaCDService relacaoItemLojaCDService, ICompraCasadaService compraCasadaService, IMailService mailService)
        {
            m_excelWriter = excelWriter;
            m_excelReader = excelReader;
            m_relacaoItemLojaCDVinculoExcelDataTranslator = relacaoItemLojaCDVinculoExcelDataTranslator;
            m_fileVaultService = fileVaultService;
            m_itemDetalheService = itemDetalheService;
            m_fornecedorParametroService = fornecedorParametroService;
            m_traitService = traitService;
            m_compraCasadaService = compraCasadaService;
            m_mainService = relacaoItemLojaCDService;
            m_mailService = mailService;
            m_configuracao = configuracao;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a importação de vinculos RelacaoItemLojaCD.
        /// </summary>
        /// <param name="model">Parametros para a importação.</param>
        /// <returns>O retorno da importação.</returns>
        [ServiceProcess("ImpVincularItemLojaCD", MaxGlobal = 1)]
        public FileVaultTicket ImportarVincular(ImportarRelacaoItemLojaCDRequest model)
        {
            SetarParametros(m_excelWriter, m_mailService, m_configuracao, m_fileVaultService, m_configuracaoColunaRelatorio);

            var streamArquivo = m_fileVaultService.Retrieve(model.Arquivos.FirstOrDefault());
            var readerResult = m_excelReader.Read(streamArquivo, s_configuracaoColunasImportacao);
            var vinculos = m_relacaoItemLojaCDVinculoExcelDataTranslator.Translate(readerResult.Rows);

            ValidarColunas(readerResult, vinculos);

            // Validações de colunas, verifica se os tipos de dados estão OK
            if (vinculos.Any(m => !m.IsValid))
            {
                return StoreResult(streamArquivo, model.Arquivos.FirstOrDefault(), vinculos, false);
            }

            // Validações de dominio RNs
            var result = ValidaRNs(model.CdSistema, model.Arquivos, vinculos, streamArquivo);

            m_mainService.SalvarVinculos(vinculos.Where(v => v.IsValid), model.IdUsuario);

            return result;
        }

        private FileVaultTicket ValidaRNs(int cdSistema, IEnumerable<FileVaultTicket> arquivos, IEnumerable<RelacaoItemLojaCDVinculo> vinculos, Stream streamArquivo)
        {
            ValidarLojaCD(cdSistema, vinculos);

            ValidarItensVendor(cdSistema, vinculos);

            ValidarTraitsRelacionamento(cdSistema, vinculos);

            new VinculoLojaNaoDevePossuirCadastroSpec(m_mainService.ItemSaidaPossuiCadastro, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoLojaCDPossuemCadastroItemControleEstoqueSpec(m_mainService.LojaCDPossuiCadastroItemControleEstoque, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoItemEntradaNaoPodeSerFilhoCompraCasadaSpec(m_compraCasadaService.ObterCodItemPaiPorCodItemFilho, cdSistema)
                .IsSatisfiedBy(vinculos);

            new ItemSaidaDeveSerValidoSpec(m_mainService.ObterItemSaidaAtendeRequisitos, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoItemPrimeXREFDevePossuirItemSecundarioStapleSpec(m_mainService.ItemPossuiItensXrefSecundarios, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoItemXrefDeveSerPrimeSpec(m_mainService.ItemXrefPrime, cdSistema)
                .IsSatisfiedBy(vinculos);

            new ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec(m_mainService.ObterTipoReabastecimentoItemVinculadoXrefPrime, cdSistema)
                .IsSatisfiedBy(vinculos);

            return StoreResult(streamArquivo, arquivos.FirstOrDefault(), vinculos, true);
        }

        private void ValidarTraitsRelacionamento(int cdSistema, IEnumerable<RelacaoItemLojaCDVinculo> vinculos)
        {
            new VinculoItemEntradaDevePossuirTraitSpec(m_traitService.PossuiTrait, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoItemSaidaDevePossuirTraitSpec(m_traitService.PossuiTrait, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoDevePossuirRelacionamentoSGPSpec(m_mainService.PossuiRelacionamentoSGP, cdSistema)
                .IsSatisfiedBy(vinculos);
        }

        private void ValidarItensVendor(int cdSistema, IEnumerable<RelacaoItemLojaCDVinculo> vinculos)
        {
            new ItemEntradaDevePossuirTipoReabastecimentoSpec(m_itemDetalheService.ObterPorItemESistema, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoItemEntradaNaoPodeSerInativoOuDeletadoSpec(m_itemDetalheService.ObterPorItemESistema, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoItemSaidaNaoPodeSerInativoOuDeletadoSpec(m_itemDetalheService.ObterPorItemESistema, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoVendorItemEntradaNaoPodeSerInativoOuDeletadoSpec(m_fornecedorParametroService.EstaInativoOuExcluido, cdSistema)
                .IsSatisfiedBy(vinculos);

            new FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec(m_itemDetalheService.ObterPorItemESistema, m_itemDetalheService.ObterEstruturadoPorId, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoVendorItemSaidaNaoPodeSerInativoOuDeletadoSpec(m_fornecedorParametroService.EstaInativoOuExcluido, cdSistema)
                .IsSatisfiedBy(vinculos);

            new VinculoVendorItemEntradaNaoPodeSerNullSpec(m_fornecedorParametroService.PossuiVendorVinculado, cdSistema)
                .IsSatisfiedBy(vinculos);

            // comentado por solicitação do Pedro em 2016/09/14 00:26
            ////new VinculoVendorItemSaidaNaoPodeSerNullSpec(m_fornecedorParametroService.PossuiVendorVinculado, model.CdSistema)
            ////    .IsSatisfiedBy(vinculos);

            new ItemEntradaNaoPodeSerTransferenciaSpec(m_itemDetalheService.ObterPorItemESistema, cdSistema)
                .IsSatisfiedBy(vinculos);

            new ItemSaidaNaoPodeSerTransferenciaSpec(m_itemDetalheService.ObterPorItemESistema, cdSistema)
                .IsSatisfiedBy(vinculos);
        }

        private void ValidarLojaCD(int cdSistema, IEnumerable<RelacaoItemLojaCDVinculo> vinculos)
        {
            new CDAtivoDeveExistirSpec(m_mainService.VerificaCDExistente, cdSistema)
                .IsSatisfiedBy(vinculos);

            new LojaAtivaDeveExistirSpec(m_mainService.VerificaLojaExistente, cdSistema)
                .IsSatisfiedBy(vinculos);

            new LojaDeveSerAtendidaPeloCDSpec(m_mainService.VerificaLojaAtendeCD, cdSistema)
                .IsSatisfiedBy(vinculos);
        }
        #endregion
    }
}
