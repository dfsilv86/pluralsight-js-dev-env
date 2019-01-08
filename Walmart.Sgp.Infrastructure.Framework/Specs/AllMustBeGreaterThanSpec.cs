using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a o valor ser maior do que o objeto especificado.
    /// </summary>    
    public class AllMustBeGraterThanSpec : SpecBase<object>
    {
        private static readonly object[] ZeroValues = 
        {
            0,
            0.0,
            0.0f,
            0.0d
        };

        private readonly IComparable m_greaterThanValue;    
        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AllMustBeGraterThanSpec"/>.
        /// </summary>
        /// <param name="greaterThanValue">O valor do qual o alvo deve ser maior.</param>        
        public AllMustBeGraterThanSpec(IComparable greaterThanValue)
        {            
            m_greaterThanValue = greaterThanValue;            
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
            var notSatifiedProperties = GetNotSatisfiedProperties(target, allProperties).ToArray();

            if (notSatifiedProperties.Length > 0)
            {
                var notSatisfiedPropertiesTexts = notSatifiedProperties.Select(p => GlobalizationHelper.GetText(p.Name))
                    .Distinct();
                var notSatisfiedPropertiesText = notSatisfiedPropertiesTexts.JoinWords();
                var msg = notSatisfiedPropertiesTexts.Count() > 1 ? Texts.AllMustBeGreaterThan : Texts.AllMustBeGreaterThanSingular;
                var valueIsZero = IsValudZero();
                return NotSatisfied(msg.With(notSatisfiedPropertiesText, valueIsZero ? Texts.Zero : m_greaterThanValue));
            }

            return Satisfied();
        }

        private bool IsValudZero()
        {
            var zeroValue = ZeroValues.FirstOrDefault(t => t.GetType() == m_greaterThanValue.GetType());            
            return null != zeroValue && m_greaterThanValue.CompareTo(zeroValue) == 0;
        }

        private IEnumerable<PropertyInfo> GetNotSatisfiedProperties(object target, IEnumerable<PropertyInfo> properties)
        {
            return properties.Where(p => !IsPropertyGreaterThanValueRange(target, p));
        }

        private bool IsPropertyGreaterThanValueRange(object target, PropertyInfo property)
        {
            IComparable value = property.GetValue(target) as IComparable;
            if (value == null)
            {
                return true;
            }

            return value.CompareTo(m_greaterThanValue) > 0;
        }
    }
}