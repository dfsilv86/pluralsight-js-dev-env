using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario.Specs
{
    /// <summary>
    /// Especificação referente a se data arquivo inventário é válida.
    /// </summary>
    public class DataArquivoInventarioDeveSerValidaSpec : SpecBase<IDetalhesArquivo>
    {
        #region Fields
        private int m_diasValidade;
        private DateTime m_dataInventario;
        private string m_descritivo;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DataArquivoInventarioDeveSerValidaSpec"/>.
        /// </summary>
        /// <param name="dataInventario">A data de inventário da loja.</param>
        /// <param name="qtdDiasValidade">A quantidade de dias em torno da data de inventário em que o arquivo é considerado válido.</param>
        /// <param name="descritivo">Detalhe sobre o arquivo ("categoria X" ou "departamento X")</param>
        public DataArquivoInventarioDeveSerValidaSpec(DateTime dataInventario, int? qtdDiasValidade, string descritivo)
        {
            m_diasValidade = qtdDiasValidade ?? 0;
            m_dataInventario = dataInventario;
            m_descritivo = descritivo;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Verifica se a data do arquivo é válida em relação a data de inventário em aberto e quantidade de dias que os arquivos são considerados válidos.
        /// </summary>
        /// <param name="target">A data do arquivo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pela data.
        /// </returns>
        /// <remarks>Realiza o teste apenas se a data foi especificada. O valor 31/12/9999 é considerado como "sem data" (vide legado).</remarks>
        public override SpecResult IsSatisfiedBy(IDetalhesArquivo target)
        {
            // TODO: usar apenas DateTime? (nulavel)
            if (!target.DataArquivo.HasValue || new DateTime(9999, 12, 31) == target.DataArquivo.Value)
            {
                return Satisfied();
            }

            var validadeInicio = m_dataInventario.Date.AddDays(-m_diasValidade);
            var validadeFim = m_dataInventario.Date.AddDays(m_diasValidade + 1); // usando < faz valer até as 23:59:59.9999 do dia

            if (validadeInicio <= target.DataArquivo.Value && target.DataArquivo.Value < validadeFim)
            {
                return Satisfied();
            }

            // A data de geração ({0}) do arquivo {1} ({2}) é divergente em mais de {3} dias da data do inventário.
            return NotSatisfied(Texts.DivergentFileDate.With(target.DataArquivo, target.NomeArquivo, m_descritivo, m_diasValidade));
        }

        #endregion
    }
}
