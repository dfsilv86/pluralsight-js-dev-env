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
    public class RelacaoItemLojaCDDesvinculoExcelDataTranslator : RelacaoItemLojaCDExcelDataTranslatorBase, IRelacaoItemLojaCDDesvinculoExcelDataTranslator
    {
        /// <summary>
        /// Seta as colunas
        /// </summary>
        /// <param name="vinculo">O vinculo.</param>
        /// <param name="column">A coluna.</param>
        public override void SetEntityColumnValue(RelacaoItemLojaCDVinculo vinculo, Column column)
        {
            if (column.Metadata.Name == "LOJA" && column.Metadata.Index == 1)
            {
                vinculo.CdLoja = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
            else if (column.Metadata.Name == "ITEM CONTROLE DE ESTOQUE" && column.Metadata.Index == 2)
            {
                vinculo.CdItemDetalheSaida = Convert.ToInt64(column.Value, RuntimeContext.Current.Culture);
            }
        }
    }
}
