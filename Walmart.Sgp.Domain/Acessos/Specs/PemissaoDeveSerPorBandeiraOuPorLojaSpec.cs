using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação referente a uma permissão poder ser apenas de bandeiras ou apenas de lojas.
    /// </summary>
    public class PemissaoDeveSerPorBandeiraOuPorLojaSpec : SpecBase<Permissao>
    {
        #region Methods
        /// <summary>
        /// Verifica se o usuário informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O usuário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo usuário.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Permissao target)
        {
            var bandeirasCount = target.Bandeiras.Count;
            var lojasCount = target.Lojas.Count;

            if (bandeirasCount == 0 && lojasCount == 0)
            {
                return NotSatisfied(Texts.InformChainOrStore);
            }

            if (bandeirasCount > 0 && lojasCount > 0)
            {
                return NotSatisfied(Texts.PermissionShouldHaveChainOrStore);
            }

            return Satisfied();
        }
        #endregion
    }
}
