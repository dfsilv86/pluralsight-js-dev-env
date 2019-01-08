using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Classe base do translator excel RelacaoItemLojaCD.
    /// </summary>
    public class RelacaoItemLojaCDExcelDataTranslatorBase
    {
        /// <summary>
        /// Realiza a tradução das linhas em objetos do tipo RelacaoItemLojaCD.
        /// </summary>
        /// <param name="rows">Linhas do arquivo excel.</param>
        /// <returns>Retornar a lista contendo os objetos traduzidos e regras de negócio inválidas.</returns>
        public IEnumerable<RelacaoItemLojaCDVinculo> Translate(IEnumerable<Row> rows)
        {
            var vinculos = new List<RelacaoItemLojaCDVinculo>();

            foreach (var row in rows)
            {
                var vinculo = new RelacaoItemLojaCDVinculo { RowIndex = row.Index, NotSatisfiedSpecReasons = new List<string>() };

                if (row.Columns.All(c => c.IsEmpty()))
                {
                    continue;
                }

                foreach (var column in row.Columns)
                {
                    var validationResult = column.Validate();
                    validationResult.ToList().ForEach(vr => vinculo.NotSatisfiedSpecReasons.Add(vr));

                    if (validationResult.Count() == 0)
                    {
                        SetEntityColumnValue(vinculo, column);
                    }
                }

                vinculos.Add(vinculo);
            }

            return vinculos;
        }

        /// <summary>
        /// Seta as colunas
        /// </summary>
        /// <param name="vinculo">O vinculo.</param>
        /// <param name="column">A coluna.</param>
        public virtual void SetEntityColumnValue(RelacaoItemLojaCDVinculo vinculo, Column column)
        {
        }
    }
}
