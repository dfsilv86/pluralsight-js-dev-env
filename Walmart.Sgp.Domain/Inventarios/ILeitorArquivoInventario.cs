using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um leitor arquivo inventario.
    /// </summary>
    public interface ILeitorArquivoInventario
    {
        /// <summary>
        /// Lê os arquivos Rtl Supercenter.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        IEnumerable<ArquivoInventario> LerArquivosRtlSupercenter(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario);

        /// <summary>
        /// Lê os arquivos Rtl Supercenter.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja.</param>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        /// <returns>Os dados lidos dos arquivos de inventário.</returns>
        IEnumerable<ArquivoInventario> LerArquivosRtlSupercenter(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, string arquivo, DateTime dataInventario);

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
        IEnumerable<ArquivoInventario> LerArquivosRtlSams(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, TipoArquivoInventario tipoArquivo, DateTime dataInventario);

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
        IEnumerable<ArquivoInventario> LerArquivosRtlSams(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, string arquivo, TipoArquivoInventario tipoArquivo, DateTime dataInventario);

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
        IEnumerable<ArquivoInventario> LerArquivosPipe(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int codigoSistema, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario);
    }
}
