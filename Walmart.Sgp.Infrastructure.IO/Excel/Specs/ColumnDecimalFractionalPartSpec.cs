using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel.Specs
{
    /// <summary>
    /// Especificação responsável por validar a quantidade de casas decimais configuradas.
    /// </summary>
    public class ColumnDecimalFractionalPartSpec : SpecBase<Column>
    {
        private static CultureInfo cultureEn = new CultureInfo("en");
        private static Regex integerPartRegex = new Regex("^(.*\\.{1}|\\d+)", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// Verifica se a coluna informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A coluna.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Column target)
        {
            if (target.Value == null || !target.Metadata.Scale.HasValue || target.Metadata.ColumnType != typeof(decimal))
            {
                return Satisfied();
            }

            var value = decimal.Parse(target.Value.ToString(), RuntimeContext.Current.Culture);

            if (integerPartRegex.Replace(value.ToString(cultureEn), string.Empty).Length <= target.Metadata.Scale.Value)
            {
                return Satisfied();
            }

            return NotSatisfied(string.Format(RuntimeContext.Current.Culture, Texts.DecimalFractionalPartGreaterThanAllowed, target.Metadata.Name));
        }
    }
}
