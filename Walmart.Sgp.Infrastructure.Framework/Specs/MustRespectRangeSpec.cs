using System;
using System.Reflection;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a o primeira propriedade não ser maior do que a segunda nem a segunda menor do que a primeira.
    /// </summary>    
    public class MustRespectRangeSpec : SpecBase<object>
    {   
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MustRespectRangeSpec"/>
        /// </summary>
        public MustRespectRangeSpec()
        {
            AllowEquals = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define um valor que indica se pode permiste valores iguais.
        /// </summary>
        public bool AllowEquals { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(object target)
        {
            var properties = GetProperties(target);
            var compareResult = CompareProperties(target, properties);

            if ((AllowEquals && compareResult > 0) || (!AllowEquals && compareResult >= 0))
            {
                return NotSatisfied(properties);
            }

            return Satisfied();
        }

        private static PropertyInfo[] GetProperties(object target)
        {
            return target == null ? new PropertyInfo[] { } : target.GetType().GetProperties();
        }

        private static int CompareProperties(object target, PropertyInfo[] properties)
            {
            if (properties.Length > 1)
            {
                var val1 = properties[0].GetValue(target) as IComparable;
                var val2 = properties[1].GetValue(target) as IComparable;

                if (val1 != null && val2 != null)
                {
                    return val1.CompareTo(val2);
                }
                }

            return -1;
            }

        private SpecResult NotSatisfied(PropertyInfo[] properties)
        {
            var text = AllowEquals ? Texts.TheFieldMustNotBeGreaterThanField : Texts.TheFieldMustNotBeGreaterOrEqualThanField;
            var msg = text.With(
                          GlobalizationHelper.GetText(properties[0].Name),
                          GlobalizationHelper.GetText(properties[1].Name));

            return NotSatisfied(msg);
        }
        #endregion
    }
}