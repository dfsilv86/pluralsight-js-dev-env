using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface para o serviço IO que transfere arquivos de inventário do servidor FTP.
    /// </summary>
    public interface ITransferidorArquivosInventario
    {
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
        IEnumerable<string> ObterArquivosViaFtp(int codigoSistema, int? codigoDepartamento, DateTime dataInventario, TipoOrigemImportacao tipoOrigem, TipoProcessoImportacao tipoProcesso, Loja loja);

        /// <summary>
        /// Remove arquivos antigos de backup.
        /// </summary>
        /// <param name="codigoLoja">O código da loja.</param>
        /// <param name="data">A data de referência.</param>
        void RemoverBackupsAntigos(int codigoLoja, DateTime data);

        /// <summary>
        /// Obtém arquivos do serviço FileVault e armazena em um diretório local.
        /// </summary>
        /// <param name="tickets">Os tickets.</param>
        /// <param name="loja">A loja.</param>
        /// <returns>A lista de arquivos (locais) que foram armazenados.</returns>
        IEnumerable<string> CopiarArquivosParaImportar(IEnumerable<FileVaultTicket> tickets, Loja loja);

        /// <summary>
        /// Exclui os arquivos que não foram usados.
        /// </summary>
        /// <param name="codigoLoja">O código da loja.</param>
        /// <param name="arquivosNaoProcessados">Arquivos que não foram usados.</param>
        void ExcluirArquivosNaoProcessados(int codigoLoja, IEnumerable<ArquivoInventario> arquivosNaoProcessados);
    }
}
