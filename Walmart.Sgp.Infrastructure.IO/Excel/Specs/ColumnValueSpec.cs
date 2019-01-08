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
    /// Especificação responsável por restringir os valores permitidos para a coluna.
    /// </summary>
    public class ColumnValueSpec : SpecBase<Column>
    {
        private readonly IEnumerable<object> m_invalidValues;
        private readonly string m_notSatisfiedCustomMessage;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ColumnValueSpec"/>.
        /// </summary>
        /// <param name="invalidValues">A lista de valores que devem ser considerados inválidos.</param>
        /// <param name="notSatisfiedCustomMessage">A mensagem customizada que será retornada em caso de especificação inválida.</param>
        public ColumnValueSpec(IEnumerable<object> invalidValues, string notSatisfiedCustomMessage = null)
        {
            this.m_invalidValues = invalidValues;
            this.m_notSatisfiedCustomMessage = notSatisfiedCustomMessage;
        }

        /// <summary>
        /// Verifica se a coluna informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A coluna.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Column target)
        {
            if (m_invalidValues == null || !m_invalidValues.Any(invalidValue => AreEqual(invalidValue, target.Value)))
            {
                return Satisfied();
            }

            return NotSatisfied(m_notSatisfiedCustomMessage ?? string.Format(RuntimeContext.Current.Culture, Texts.InvalidColumnFormat, target.Metadata.Name));
        }

        private static bool AreEqual(object value1, object value2)
        {
            return Convert.ToString(value1, RuntimeContext.Current.Culture) == Convert.ToString(value2, RuntimeContext.Current.Culture);
        }
    }
}