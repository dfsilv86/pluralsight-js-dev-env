using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Configuração da infraestrutura de processamentos.
    /// </summary>
    public class ProcessingConfiguration
    {
        /// <summary>
        /// Obtém ou define a raiz para o armazenamento de arquivos (pode ser local ou server)
        /// </summary>
        public string StorageRoot { get; set; }

        /// <summary>
        /// Obtém ou define o caminho para a estrutura de diretórios do ProcessingService, onde os arquivos de resultado dos processamentos serão armazenados.
        /// </summary>
        public string ResultsPath { get; set; }

        /// <summary>
        /// Obtém ou define o caminho para a estrutura de diretórios do ProcessingService, onde os arquivos que servem como parâmetros do processo serão armazenados.
        /// </summary>
        public string InputFilesPath { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o enfileiramento está habilitado de maneira global.
        /// </summary>
        public bool EnableQueueing { get; set; }

        /// <summary>
        /// Obtém ou define o tempo após a última atualização da ordem de processamento até que esta seja removida.
        /// </summary>
        public TimeSpan MaxAge { get; set; }
    }
}
