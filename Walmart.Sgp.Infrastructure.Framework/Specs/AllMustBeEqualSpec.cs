using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a se todos os campos são iguais.
    /// </summary>    
    public class AllMustBeEqualSpec : SpecBase<object>
    {   
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

            if (AnyPropertyIsDifferent(target, allProperties))
            {
                var notSatisfiedPropertiesTexts = allProperties.Select(p => GlobalizationHelper.GetText(p.Name))
                    .Distinct();
                var notSatisfiedPropertiesText = notSatisfiedPropertiesTexts.JoinWords();
                var msg = Texts.TheFieldsMustBeEqual;                
                return NotSatisfied(msg.With(notSatisfiedPropertiesText));
            }

            return Satisfied();
        }

        private static bool AnyPropertyIsDifferent(object target, IEnumerable<PropertyInfo> properties)
        {
            var firstValue = properties.First().GetValue(target);
            return properties.Any(p => !PropertyEqualsValue(target, p, firstValue));
        }

        private static bool PropertyEqualsValue(object target, PropertyInfo property, object otherValue)
        {
            IComparable value = property.GetValue(target) as IComparable;
            if (value == null)
            {
                return false;
            }

            return value.CompareTo(otherValue) == 0;
        }
    }
}