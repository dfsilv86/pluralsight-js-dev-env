using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Gerenciamento.Specs
{
    /// <summary>
    /// Especificação referente a que uma região deve ter distritos.
    /// </summary>
    public class RegiaoDeveTerDistritosSpec : SpecBase<Regiao>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RegiaoDeveTerDistritosSpec"/>.
        /// </summary>
        public RegiaoDeveTerDistritosSpec()            
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o usuário informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O usuário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo usuário.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Regiao target)
        {
            if (target.Distritos == null || target.Distritos.Count() == 0)
            {
                return NotSatisfied(Texts.RegionMustHaveDistricts);
            }

            return Satisfied();
        }
        #endregion
    }
}
