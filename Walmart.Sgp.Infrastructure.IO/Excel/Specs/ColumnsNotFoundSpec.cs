using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel.Specs
{
    /// <summary>
    /// Especificação responsável por validar a ausência de colunas configuradas para leitura.
    /// </summary>
    public class ColumnNotFoundSpec : SpecBase<IEnumerable<ColumnMetadata>>
    {
        /// <summary>
        /// Verifica se a lista de metadados de colunas informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A lista de metadados de colunas.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ColumnMetadata> target)
        {
            if (target == null || target.Count() == 0)
            {
                return Satisfied();
            }

            var reason = target
                .Count()
                .PluralizedMessage("ColumnNotFound")
                .With(target.Select(c => c.Name).JoinWords());

            return NotSatisfied(reason);
        }
    }
}
