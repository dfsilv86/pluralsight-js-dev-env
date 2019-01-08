using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Web.Resources
{
    /// <summary>
    /// Suporte para acesso a um arquivo.
    /// </summary>
    public interface IPostedFile
    {
        /// <summary>
        /// Obtém o nome completo do arquivo.
        /// </summary>
        /// <value>
        /// O nome completo do arquivo, incluindo seu diretório.
        /// </value>
        string FileName { get; }

        /// <summary>
        /// Obtém tipo de conteúdo MIME do arquivo.
        /// </summary>
        /// <value>
        /// O tipo de conteudo MIME do arquivo.
        /// </value>
        string ContentType { get; }

        /// <summary>
        /// Salva o conteúdo do arquivo.
        /// </summary>
        /// <param name="fileName">O nome do arquivo salvo.</param>
        void SaveAs(string fileName);
    }
}
