using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a o valor está dentro de uma faixa válida.
    /// </summary>    
    public class AllMustBeInRangeSpec : SpecBase<object>
    {
        private readonly IComparable m_minValue;
        private readonly IComparable m_maxValue;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AllMustBeInRangeSpec"/>.
        /// </summary>
        /// <param name="minValue">O valor mínimo.</param>
        /// <param name="maxValue">O valor máximo.</param>
        public AllMustBeInRangeSpec(IComparable minValue, IComparable maxValue)
        {
            m_minValue = minValue;
            m_maxValue = maxValue;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(object target)
        {
            var allProperties = target == null ? new PropertyInfo[] { } : target.GetType().GetProperties();
            var outOfRangeProperties = GetOutOfRangeProperties(target, allProperties).ToArray();

            if (outOfRangeProperties.Length > 0)
            {
                var outOfRangePropertiesTexts = outOfRangeProperties.Select(p => GlobalizationHelper.GetText(p.Name))
                    .Distinct();
                var outOfRangePropertiesText = outOfRangePropertiesTexts.JoinWords();
                var msg = outOfRangePropertiesTexts.Count() > 1 ? Texts.AllMustBeInRange : Texts.AllMustBeInRangeSingular;
                return NotSatisfied(msg.With(outOfRangePropertiesText, m_minValue, m_maxValue));
            }

            return Satisfied();
        }

        private IEnumerable<PropertyInfo> GetOutOfRangeProperties(object target, IEnumerable<PropertyInfo> properties)
        {
            return properties.Where(p => !IsPropertyInRange(target, p));
        }

        private bool IsPropertyInRange(object target, PropertyInfo property)
        {
            IComparable value = property.GetValue(target) as IComparable;
            if (value == null)
            {
                return true;
            }

            return value.CompareTo(m_minValue) >= 0 && value.CompareTo(m_maxValue) <= 0;
        }
    }
}