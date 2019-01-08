using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação para verificar se a soma de uma coleção é igual a 100%.
    /// </summary>
    public class SumMustMatchSpec : SpecBase<object>
    {
        private decimal m_value = 0;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SumMustMatchSpec" /> para verificar se a soma dos itens bate com um valor informado.
        /// </summary>
        /// <param name="matchingValue">Valor desejado da soma.</param>
        public SumMustMatchSpec(decimal matchingValue)
        {
            this.m_value = matchingValue;
        }

        #region Methods
        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">Um IEnumerable de tipos numéricos.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(object target)
        {
            var prop = target as IEnumerable;
            if (prop == null)
            {
                return NotSatisfied(Texts.PropIsNotIEnumerable);
            }

            decimal soma = 0;
            foreach (var item in prop)
            {
                decimal val = Convert.ToDecimal(item, System.Globalization.CultureInfo.CurrentCulture);
                soma += val;
            }

            if (soma != m_value)
            {
                return NotSatisfied(Texts.SumDoesntMatch.With(m_value));
            }

            return Satisfied();
        }

        #endregion
    }
}
