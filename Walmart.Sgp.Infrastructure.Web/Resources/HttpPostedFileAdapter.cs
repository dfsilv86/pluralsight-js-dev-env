using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Infrastructure.Web.Resources
{
    /// <summary>
    /// Provê acesso a arquivos individuais que foram carregados por um cliente.
    /// </summary>
    public class HttpPostedFileAdapter : IPostedFile, IFile
    {
        private readonly HttpPostedFile m_postedFile;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="HttpPostedFileAdapter"/>.
        /// </summary>
        /// <param name="postedFile">O arquivo.</param>
        public HttpPostedFileAdapter(HttpPostedFile postedFile)
        {
            ExceptionHelper.ThrowIfNull("postedFile", postedFile);
            this.m_postedFile = postedFile;
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
                return m_postedFile.FileName;
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
                return LocalPostedFile.GetContentType(FileName);
            }
        }

        /// <summary>
        /// Obtém o tamanho do arquivo, em bytes.
        /// </summary>
        public int ContentLength
        {
            get
            {
                return m_postedFile.ContentLength;
            }
        }

        /// <summary>
        /// Obtém o stream de entrada do arquivo.
        /// </summary>
        public Stream InputStream
        {
            get
            {
                return m_postedFile.InputStream;
            }
        }

        /// <summary>
        /// Salva o conteúdo do arquivo.
        /// </summary>
        /// <param name="fileName">O nome do arquivo salvo.</param>
        public void SaveAs(string fileName)
        {
            m_postedFile.SaveAs(fileName);
        }
    }
}
