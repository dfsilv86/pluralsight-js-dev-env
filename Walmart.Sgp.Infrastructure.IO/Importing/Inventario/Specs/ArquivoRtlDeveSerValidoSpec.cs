using System;
using System.IO;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Helpers;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario.Specs
{
    /// <summary>
    /// Especificação referente a se arquivo Rtl é válido.
    /// </summary>
    public class ArquivoRtlDeveSerValidoSpec : SpecBase<ArquivoFtp>
    {
        #region Fields
        private string m_caminhoExtracao;
        private string m_extensaoArquivo;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ArquivoRtlDeveSerValidoSpec"/>.
        /// </summary>
        /// <param name="caminhoExtracao">O diretório local no servidor onde os arquivos são extraídos.</param>
        /// <param name="extensaoArquivo">A extensão do arquivo compactado.</param>
        public ArquivoRtlDeveSerValidoSpec(string caminhoExtracao, string extensaoArquivo)
        {
            m_caminhoExtracao = caminhoExtracao;
            m_extensaoArquivo = extensaoArquivo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o arquivo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">As informações sobre o arquivo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo arquivo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ArquivoFtp target)
            {
            if (target.IsDiretorio || File.Exists(Path.Combine(m_caminhoExtracao, target.NomeArquivo.Replace(m_extensaoArquivo, string.Empty))))
            {
                return NotSatisfied(Texts.InvalidFile);
            }

            return Satisfied();
        }
        #endregion
    }
}
