using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.FileVault
{
    /// <summary>
    /// Define a interface para um arquivo de entrada no file vault.
    /// </summary>
    public interface IFile
    {        
        /// <summary>
        /// Obtém o nome completo do arquivo.
        /// </summary>
        /// <value>
        /// O nome completo do arquivo, incluindo seu diretório.
        /// </value>
        string FileName { get; }

        /// <summary>
        /// Salva o conteúdo do arquivo.
        /// </summary>
        /// <param name="destinationFileName">O nome do arquivo salvo.</param>
        void SaveAs(string destinationFileName);
    }
}
