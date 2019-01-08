using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Implementa a interface responsável pela tradução das informações do Excel de importação para a entidade de domínio RelacaoItemLojaCD.
    /// </summary>
    public class RelacaoItemLojaCDVinculoExcelDataTranslator : RelacaoItemLojaCDExcelDataTranslatorBase, IRelacaoItemLojaCDVinculoExcelDataTranslator
    {
        /// <summary>
        /// Seta as colunas
        /// </summary>
        /// <param name="vinculo">O vinculo.</param>
        /// <param name="column">A coluna.</param>
        public override void SetEntityColumnValue(RelacaoItemLojaCDVinculo vinculo, Column column)
        {
            if (column.Metadata.Name == "CD" && column.Metadata.Index == 1)
            {
                vinculo.CdCD = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "LOJA" && column.Metadata.Index == 2)
            {
                vinculo.CdLoja = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "ITEM CONTROLE DE ESTOQUE" && column.Metadata.Index == 5)
            {
                vinculo.CdItemDetalheSaida = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "ITEM DE ENTRADA" && column.Metadata.Index == 7)
            {
                vinculo.CdItemDetalheEntrada = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
        }
    }
}
