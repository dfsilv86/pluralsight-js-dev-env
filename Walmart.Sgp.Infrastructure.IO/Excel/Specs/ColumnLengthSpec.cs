using System;
using System.Collections.Generic;
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
    /// Especificação responsável por validar o comprimento configurado.
    /// </summary>
    public class ColumnLengthSpec : SpecBase<Column>
    {
        /// <summary>
        /// Verifica se a coluna informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A coluna.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Column target)
        {
            if (target.Value == null)
            {
                return Satisfied();
            }

            if (target.Value.ToString().Length <= target.Metadata.Length)
            {
                return Satisfied();
            }

            return NotSatisfied(string.Format(RuntimeContext.Current.Culture, Texts.LengthGreaterThanTheAllowed, target.Metadata.Name));
        }
    }
}
