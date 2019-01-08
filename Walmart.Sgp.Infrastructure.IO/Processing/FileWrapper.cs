using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Infrastructure.IO.Processing
{
    /// <summary>
    /// Implementação de IFile para um arquivo salvo no sistema de arquivos ou em um servidor.
    /// </summary>
    internal class FileWrapper : IFile
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FileWrapper"/>.
        /// </summary>
        /// <param name="filePath">O caminho completo para o arquivo.</param>
        public FileWrapper(string filePath)
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
        }

        /// <summary>
        /// Obtém o caminho completo para o arquivo.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Obtém o nome do arquivo.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Salva o conteúdo do arquivo.
        /// </summary>
        /// <param name="destinationFileName">O nome do arquivo salvo.</param>
        public void SaveAs(string destinationFileName)
        {
            File.Copy(this.FilePath, destinationFileName);
        }
    }
}
