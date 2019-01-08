using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.FileVault
{
    /// <summary>
    /// Configuração do FileVaultService.
    /// </summary>
    public class FileVaultConfiguration
    {
        /// <summary>
        /// Obtém ou define o caminho para a estrutura de diretórios do FileVaultService, onde os arquivos temporários serão armazenados.
        /// </summary>
        public string Staging { get; set; }

        /// <summary>
        /// Obtém ou define a raiz para o armazenamento de arquivos (pode ser local ou server)
        /// </summary>
        public string StorageRoot { get; set; }
    }
}
