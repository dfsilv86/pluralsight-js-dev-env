using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Implementa a interface responsável pela tradução das informações do Excel de importação para a entidade de domínio Multisourcing
    /// </summary>
    public class MultisourcingExcelDataTranslator : IMultisourcingExcelDataTranslator
    {
        /// <summary>
        /// Realiza a tradução das linhas em objetos do tipo Multisourcing
        /// </summary>
        /// <param name="rows">Linhas do arquivo excel.</param>
        /// <returns>Retornar a lista contendo os objetos traduzidos e regras de negócio inválidas.</returns>
        public IEnumerable<Multisourcing> Translate(IEnumerable<Row> rows)
        {
            var multisourcings = new List<Multisourcing>();

            foreach (var row in rows)
            {
                var multisourcing = new Multisourcing { RowIndex = row.Index };

                if (row.Columns.All(c => c.IsEmpty()))
                {
                    continue;
                }

                foreach (var column in row.Columns)
                {
                    var validationResult = column.Validate();
                    validationResult.ToList().ForEach(vr => multisourcing.NotSatisfiedSpecReasons.Add(vr));
                    
                    if (validationResult.Count() == 0)
                    {
                        SetEntityColumnValue(multisourcing, column);
                    }
                }

                multisourcings.Add(multisourcing);
            }

            return multisourcings;
        }

        // TODO (Bueno): Avaliar de-para de colunas e propriedades da classe
        private static void SetEntityColumnValue(Multisourcing multisoucing, Column column)
        {
            if (column.Metadata.Name == "ITEM CONTROLE ESTOQUE" && column.Metadata.Index == 1)
            {
                multisoucing.CdItemDetalheSaida = Convert.ToInt32(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "ITEM DE ENTRADA" && column.Metadata.Index == 3)
            {
                multisoucing.CdItemDetalheEntrada = Convert.ToInt32(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "VENDOR 9 DIGITOS" && column.Metadata.Index == 5)
            {
                multisoucing.Vendor9Digitos = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "CD" && column.Metadata.Index == 8)
            {
                multisoucing.CD = Convert.ToInt32(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "PERCENTUAL" && column.Metadata.Index == 11)
            {
                if (column.Value == null || string.IsNullOrWhiteSpace(column.Value.ToString()))
                {
                    column.Value = 0;
                }

                multisoucing.vlPercentual = Convert.ToDecimal(column.Value, RuntimeContext.Current.Culture);
            }
        }
    }
}
