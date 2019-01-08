using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.FileVault
{
    /// <summary>
    /// Implementação de IFileVaultInputFile que utiliza um Stream.
    /// </summary>
    public class IntermediateStreamFile : IFile
    {
        #region Fields
        private Stream m_stream;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="IntermediateStreamFile"/>.
        /// </summary>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <param name="stream">O stream do arquivo.</param>
        public IntermediateStreamFile(string fileName, Stream stream)
        {
            FileName = fileName;
            m_stream = stream;
        }
        #endregion

        /// <summary>
        /// Obtém o nome completo do arquivo.
        /// </summary>
        /// <value>
        /// O nome completo do arquivo, incluindo seu diretório.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Salva o conteúdo do arquivo.
        /// </summary>
        /// <param name="destinationFileName">O nome do arquivo salvo.</param>
        public void SaveAs(string destinationFileName)
        {
            using (var file = File.Create(destinationFileName))
            {
                m_stream.CopyTo(file);
            }
        }
    }
}
