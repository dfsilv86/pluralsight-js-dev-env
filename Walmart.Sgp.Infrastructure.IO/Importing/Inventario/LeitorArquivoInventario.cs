using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Leitor de arquivos de inventário.
    /// </summary>
    public class LeitorArquivoInventario : ILeitorArquivoInventario
    {
        #region Fields
        private ILeitorLogger m_logger;
        private IParametroService m_parametroService;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventario"/>.
        /// </summary>
        /// <param name="logger">O serviço de log de processo de importação de inventário.</param>
        /// <param name="parametroService">O serviço de parâmetros.</param>
        public LeitorArquivoInventario(ILeitorLogger logger, IParametroService parametroService)
        {
            m_logger = logger;
            m_parametroService = parametroService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lê os arquivos Rtl Supercenter.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        public IEnumerable<ArquivoInventario> LerArquivosRtlSupercenter(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario)
        {
            var qtdDias = m_parametroService.Obter().qtdDiasArquivoInventarioVarejo ?? 0;

            var leitor = new LeitorArquivoInventarioRtlSupercenter(tipoProcesso, tipoOrigem, idLojaImportacao, arquivos, dataInventario, qtdDias, m_logger);

            return leitor.LerArquivos();
        }

        /// <summary>
        /// Lê os arquivos Rtl Supercenter.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        public IEnumerable<ArquivoInventario> LerArquivosRtlSupercenter(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, string arquivo, DateTime dataInventario)
        {
            var qtdDias = m_parametroService.Obter().qtdDiasArquivoInventarioVarejo ?? 0;

            var leitor = new LeitorArquivoInventarioRtlSupercenter(tipoProcesso, tipoOrigem, idLojaImportacao, arquivo, dataInventario, qtdDias, m_logger);

            return leitor.LerArquivos();
        }

        /// <summary>
        /// Lê os arquivos Rtl Sam's.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="tipoArquivo">O tipo de arquivo.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        public IEnumerable<ArquivoInventario> LerArquivosRtlSams(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, TipoArquivoInventario tipoArquivo, DateTime dataInventario)
        {
            var leitor = new LeitorArquivoInventarioRtlSams(tipoProcesso, tipoOrigem, idLojaImportacao, arquivos, tipoArquivo, dataInventario, m_logger);

            return leitor.LerArquivos();
        }

        /// <summary>
        /// Lê os arquivos Rtl Sam's.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="tipoArquivo">O tipo de arquivo.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        public IEnumerable<ArquivoInventario> LerArquivosRtlSams(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, string arquivo, TipoArquivoInventario tipoArquivo, DateTime dataInventario)
        {
            var leitor = new LeitorArquivoInventarioRtlSams(tipoProcesso, tipoOrigem, idLojaImportacao, arquivo, tipoArquivo, dataInventario, m_logger);

            return leitor.LerArquivos();
        }

        /// <summary>
        /// Lê os arquivos Pipe.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="codigoSistema">O código da estrutura mercadológica.</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        public IEnumerable<ArquivoInventario> LerArquivosPipe(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int codigoSistema, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario)
        {
            var parametro = m_parametroService.Obter();

            var qtdDias = codigoSistema == 1 ? parametro.qtdDiasArquivoInventarioVarejo : parametro.qtdDiasArquivoInventarioAtacado;

            var leitor = new LeitorArquivoInventarioPipe(tipoProcesso, tipoOrigem, codigoSistema, idLojaImportacao, arquivos, dataInventario, qtdDias, m_logger);

            return leitor.LerArquivos();
        }
        #endregion
    }
}
