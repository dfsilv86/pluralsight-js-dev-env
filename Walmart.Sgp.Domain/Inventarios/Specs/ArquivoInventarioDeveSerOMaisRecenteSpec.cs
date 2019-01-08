using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se arquivo inventario deve ser o mais recente.
    /// </summary>
    public class ArquivoInventarioDeveSerOMaisRecenteSpec : SpecBase<IDetalhesArquivo>
    {
        #region Fields
        private Inventario m_inventario;
        private string m_descritivo;
        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ArquivoInventarioDeveSerOMaisRecenteSpec"/>.
        /// </summary>
        /// <param name="inventario">O inventario.</param>
        /// <param name="descritivo">O descritivo ("Categoria: X" ou "Departamento: X").</param>
        public ArquivoInventarioDeveSerOMaisRecenteSpec(Inventario inventario, string descritivo)
        {
            m_inventario = inventario;
            m_descritivo = descritivo;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Verifica se o arquivo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O arquivo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo arquivo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IDetalhesArquivo target)
        {
            if (m_inventario.dhInventarioArquivo.HasValue && m_inventario.stInventario == InventarioStatus.Importado && m_inventario.dhInventarioArquivo.Value > target.DataArquivo)
            {
                // Um arquivo de inventário mais recente já foi importado. Data arquivo atual: {0:dd/MM/yyyy HH:mm:ss}, Data da última importação: {1:dd/MM/yyyy HH:mm:ss}, {2}.
                return NotSatisfied(Texts.InventoryFileIsOutOfDate.With(target.DataArquivo, m_inventario.dhInventarioArquivo.Value, m_descritivo));
            }

            return Satisfied();
        }
        #endregion

    }
}
