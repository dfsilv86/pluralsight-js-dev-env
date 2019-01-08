using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Web.Resources
{
    /// <summary>
    /// Provê acesso a arquivos individuais criados manualmente no servidor. 
    /// </summary>
    public class LocalPostedFile : IPostedFile
    {
        private static readonly Dictionary<string, string> ContentTypes = new Dictionary<string, string>
        {
            { ".jpeg.jpg.jpe", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".pdf", "application/pdf" },
            { ".png", "image/png" },
            { ".xls.xlsx", "application/vnd.ms-excel" }
        };

        private readonly byte[] m_contents;

        private readonly string m_contentType;

        private readonly string m_fileName;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LocalPostedFile"/>.
        /// </summary>
        /// <param name="fileContents">O conteúdo do arquivo.</param>
        /// <param name="fileName">O nome do arquivo.</param>
        public LocalPostedFile(byte[] fileContents, string fileName)
        {
            m_contents = fileContents;
            m_fileName = fileName;
            m_contentType = GetContentType(m_fileName);
        }

        /// <summary>
        /// Obtém o nome completo do arquivo.
        /// </summary>
        /// <value>
        /// O nome completo do arquivo, incluindo seu diretório.
        /// </value>
        public string FileName
        {
            get
            {
                return m_fileName;
            }
        }

        /// <summary>
        /// Obtém tipo de conteúdo MIME do arquivo.
        /// </summary>
        /// <value>
        /// O tipo de conteudo MIME do arquivo.
        /// </value>
        public string ContentType
        {
            get
            {
                return m_contentType;
            }
        }

        /// <summary>
        /// Retorna o tipo de conteudo do arquivo especificado.
        /// </summary>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <returns>O tipo de contúdo MIME baseado na extensão do arquivo.</returns>
        /// <exception cref="System.InvalidOperationException">Quando o arquivo não possui uma extensão conhecida.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            extension = extension.ToLowerInvariant();
            var contentTypeKey = ContentTypes.Keys.FirstOrDefault(t => t.Contains(extension));
            if (contentTypeKey != null)
            {
                return ContentTypes[contentTypeKey];
            }

            throw new InvalidOperationException(Texts.UnableToFindFileContentType);
        }

        /// <summary>
        /// Salva o conteúdo do arquivo.
        /// </summary>
        /// <param name="fileName">O nome do arquivo salvo.</param>
        public void SaveAs(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                stream.Write(m_contents, 0, m_contents.Length);
            }
        }
    }
}
