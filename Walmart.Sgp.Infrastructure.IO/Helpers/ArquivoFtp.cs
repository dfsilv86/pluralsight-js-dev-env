using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.IO.Helpers
{
    /// <summary>
    /// Informações sobre um arquivo ou diretório presente em um servidor FTP.
    /// </summary>
    public class ArquivoFtp : IDetalhesArquivo
    {
        /// <summary>
        /// Obtém o nome do arquivo.
        /// </summary>
        public string NomeArquivo { get; internal set; }

        /// <summary>
        /// Obtém a data do arquivo, se presente.
        /// </summary>
        public DateTime? DataArquivo { get; internal set; }

        /// <summary>
        /// Obtém a Uri para o arquivo.
        /// </summary>
        public Uri Uri { get; internal set; }

        /// <summary>
        /// Obtém ou define um valor que indica se este item é um diretório.
        /// </summary>
        public bool IsDiretorio { get; set; }

        /// <summary>
        /// Obtém ou define o tamanho do arquivo.
        /// </summary>
        public int TamanhoArquivo { get; set; }
    }
}
