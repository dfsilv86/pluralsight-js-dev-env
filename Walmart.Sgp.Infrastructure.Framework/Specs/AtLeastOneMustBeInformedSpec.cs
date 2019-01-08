using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a se pelo menos uma das propriedades informadas no objeto anônimo teve seu valor informado.
    /// </summary>
    public class AtLeastOneMustBeInformedSpec : SpecBase<object>
    {
        #region Fields
        private bool m_allowZeroes = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AtLeastOneMustBeInformedSpec"/>.
        /// </summary>
        public AtLeastOneMustBeInformedSpec()
            : this(false)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AtLeastOneMustBeInformedSpec"/>.
        /// </summary>
        /// <param name="allowZeroes">Indica se o valor zero é permitido.</param>
        public AtLeastOneMustBeInformedSpec(bool allowZeroes)
        {
            m_allowZeroes = allowZeroes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O object.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public override SpecResult IsSatisfiedBy(object target)
        {
            var allProperties = target == null ? new PropertyInfo[0] : target.GetType().GetProperties();
            var satisfiedProperties = GetSatisfiedProperties(target, allProperties);

            if (satisfiedProperties.Any())
            {
                return new PropertiesSpecResult(allProperties, satisfiedProperties, true);
            }

            var transaltedProperties = allProperties
                    .Select(p => GlobalizationHelper.GetText(p.Name))
                    .Distinct();

            return new PropertiesSpecResult(allProperties, satisfiedProperties, false, Texts.AtLeastOneMustBeInformed.With(transaltedProperties.JoinWords(Texts.Or.ToLowerInvariant())));
        }

        /// <summary>
        /// Obtém quais propriedades tiveram seu valor informado (diferente de nulo ou valor padrão).
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <param name="allProperties">As propriedades do alvo.</param>
        /// <returns>As propriedades que satisfazem a spec.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private IEnumerable<PropertyInfo> GetSatisfiedProperties(object target, PropertyInfo[] allProperties)
        {            
            return from p in allProperties
                   let value = p.GetValue(target) as object
                   let nullableType = Nullable.GetUnderlyingType(p.PropertyType)
                   let propertyType = nullableType == null ? p.PropertyType : nullableType
                   where PropertyValueIsNullOrEmpty(value, propertyType)
                   select p;
        }

        private bool PropertyValueIsNullOrEmpty(object value, Type propertyType)
        {
            return value != null
                && (propertyType != typeof(byte) || (byte)value != 0 || (this.m_allowZeroes && (byte)value == 0))
                && (propertyType != typeof(int) || (int)value != 0 || (this.m_allowZeroes && (int)value == 0))
                && (propertyType != typeof(long) || (long)value != 0 || (this.m_allowZeroes && (long)value == 0))
                && (propertyType != typeof(float) || (float)value != 0 || (this.m_allowZeroes && (float)value == 0))
                && (propertyType != typeof(decimal) || (decimal)value != 0 || (this.m_allowZeroes && (decimal)value == 0))
                && (propertyType != typeof(string) || !string.IsNullOrWhiteSpace((string)value))
                && (propertyType != typeof(DateTime) || (DateTime)value != DateTime.MinValue);
        }
        #endregion
    }
}
