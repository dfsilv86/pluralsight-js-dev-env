using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Infrastructure.IO.FileVault
{
    /// <summary>
    /// Define a interface do serviço FileVault, relacionado ao gerenciamento de arquivos em caráter temporário.
    /// </summary>
    public interface IFileVaultService
    {
        /// <summary>
        /// Armazena um arquivo.
        /// </summary>
        /// <param name="file">O arquivo a ser armazenado.</param>
        /// <remarks>
        /// Atualmente existem duas implementações de IFile:
        /// * HttpPostedFileAdapter: pode ser utilizado quando estamos enviando um arquivo do client para o server.
        /// * IntermediateStreamFile: pode ser utilizado quando estamos criando um arquivo no server que deve ser enviado posteriormente ao client.
        /// </remarks>
        /// <returns>O ticket para o arquivo.</returns>
        FileVaultTicket Store(IFile file);

        /// <summary>
        /// Obtém um arquivo previamente armazenado, retornando um Stream, e remove o arquivo assim que o Stream for descartado.
        /// </summary>
        /// <param name="ticket">O ticket para o arquivo.</param>
        /// <returns>O Stream para leitura do arquivo.</returns>
        /// <remarks>O arquivo é removido após o descarte do Stream.</remarks>
        Stream Retrieve(FileVaultTicket ticket);

        /// <summary>
        /// Remove arquivos antigos.
        /// </summary>
        /// <remarks>O arquivo é considerado antigo se foi armazenado há mais de 24 horas.</remarks>
        void Cleanup();

        /// <summary>
        /// Move o arquivo para fora da área temporária.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <param name="path">O caminho para o diretório de destino.</param>
        /// <returns>O caminho completo para o arquivo salvo.</returns>
        string SavePermanently(FileVaultTicket ticket, string path);

        /// <summary>
        /// Remove um arquivo, se existe.
        /// </summary>
        /// <param name="theTicket">O ticket.</param>
        void Discard(FileVaultTicket theTicket);
    }
}
