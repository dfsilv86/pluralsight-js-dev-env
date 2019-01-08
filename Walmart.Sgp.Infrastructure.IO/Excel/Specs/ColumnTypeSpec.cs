using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel.Specs
{
    /// <summary>
    /// Especificação responsável por validar o type configurado.
    /// </summary>
    public class ColumnTypeSpec : SpecBase<Column>
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
            try
            {
                Convert.ChangeType(target.Value, target.Metadata.ColumnType, RuntimeContext.Current.Culture);
                return Satisfied();
            }
            catch (InvalidCastException)
            {
                return NotSatisfied(string.Format(RuntimeContext.Current.Culture, Texts.InvalidColumnFormat, target.Metadata.Name));
            }
            catch (FormatException)
            {
                return NotSatisfied(string.Format(RuntimeContext.Current.Culture, Texts.InvalidColumnFormat, target.Metadata.Name));
            }
        }
    }
}
