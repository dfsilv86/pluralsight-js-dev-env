using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a departamento deve ser perecível.
    /// </summary>
    /// <remarks>Usado na importação automática para emitir mensagem de erro.</remarks>
    public class DepartamentoDeveSerPerecivelSpec : SpecBase<Departamento>
    {
        #region Fields
        private string m_nomeArquivo;
        #endregion

        #region Constructor        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DepartamentoDeveSerPerecivelSpec"/>.
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo.</param>
        public DepartamentoDeveSerPerecivelSpec(string nomeArquivo)
        {
            m_nomeArquivo = nomeArquivo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o departamento informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O departamento.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo departamento.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Departamento target)
        {
            if (target.blPerecivel != "S")
            {
                return NotSatisfied(Texts.FileDepartmentIsNotPerishable.With(target.cdDepartamento, m_nomeArquivo));
            }

            return Satisfied();
        }
        #endregion
    }
}
