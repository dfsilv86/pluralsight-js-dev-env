using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Helpers;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario.Specs
{
    /// <summary>
    /// Especificação referente a se arquivo pipe é válido.
    /// </summary>
    public class ArquivoPipeDeveSerValidoSpec : SpecBase<ArquivoFtp>
    {
        #region Fields
        private string m_prefixoArquivo;
        private string m_tipoArquivo;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ArquivoPipeDeveSerValidoSpec"/>.
        /// </summary>
        /// <param name="prefixoArquivo">O prefixo do arquivo.</param>
        /// <param name="tipoArquivo">O tipo de arquivo.</param>
        public ArquivoPipeDeveSerValidoSpec(string prefixoArquivo, string tipoArquivo)
        {
            m_prefixoArquivo = prefixoArquivo;
            m_tipoArquivo = tipoArquivo;
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
            if (target.IsDiretorio || !target.NomeArquivo.StartsWith(m_prefixoArquivo, StringComparison.OrdinalIgnoreCase) || !target.NomeArquivo.Contains(m_tipoArquivo))
            {
                return NotSatisfied(Texts.InvalidFile);
            }

            return Satisfied();
        }
        #endregion
    }
}
