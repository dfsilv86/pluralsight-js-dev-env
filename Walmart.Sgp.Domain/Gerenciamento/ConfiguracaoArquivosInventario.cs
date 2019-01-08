using System.Diagnostics.CodeAnalysis;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa configurações relacionadas aos arquivos de inventário que ficam no Web.config
    /// </summary>
    public class ConfiguracaoArquivosInventario
    {
        /// <summary>
        /// Obtém ou define o diretório no servidor da api onde manter os arquivos obtidos do servidor FTP.
        /// </summary>
        public string CaminhoDownload { get; set; }

        /// <summary>
        /// Obtém ou define o prefixo dos nomes de arquivo Rtl.
        /// </summary>
        public string PrefixoArquivoRtl { get; set; }

        /// <summary>
        /// Obtém ou define o prefixo dos nomes de arquivo Pipe.
        /// </summary>
        public string PrefixoArquivoPipe { get; set; }

        /// <summary>
        /// Obtém ou define a extensão dos arquivos.
        /// </summary>
        public string ExtensaoArquivo { get; set; }

        /// <summary>
        /// Obtém ou define o período em que
        /// </summary>
        public int? DiasPermanenciaArquivos { get; set; }

        /// <summary>
        /// Obtém o nome do subdiretório onde os arquivos descompactados são gravados.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string DiretorioDescompactados
        {
            get
            {
                return "Descompactados";
            }
        }

        /// <summary>
        /// Obtém o nome do subdiretório onde os arquivos de backup são gravados.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string DiretorioBackup
        {
            get
            {
                return "BackUp";
            }
        }

        /// <summary>
        /// Obtém o fragmento de nome de arquivo que indica que o arquivo pipe é de varejo.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string ArquivoPipeVarejo
        {
            get
            {
                return "FINALIZED";
            }
        }

        /// <summary>
        /// Obtém o fragmento de nome de arquivo que indica que o arquivo pipe é de atacado.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string ArquivoPipeAtacado
        {
            get
            {
                return "STARTED";
            }
        }

        /// <summary>
        /// A raiz para o armazenamento de arquivos (pode ser local ou server)
        /// </summary>
        public string StorageRoot { get; set; }
    }
}
