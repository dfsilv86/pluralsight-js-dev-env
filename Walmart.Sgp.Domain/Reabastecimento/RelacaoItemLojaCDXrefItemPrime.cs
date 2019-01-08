using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Classe que representa os dados da relação de item prime Xref
    /// </summary>
    public class RelacaoItemLojaCDXrefItemPrime
    {
        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public long IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define CdCrossRef.
        /// </summary>
        public long? CdCrossRef { get; set; }

        /// <summary>
        /// Obtém ou define CdItem.
        /// </summary>
        public long CdItem { get; set; }

        /// <summary>
        /// Obtém ou define DsItem.
        /// </summary>
        public string DsItem { get; set; }

        /// <summary>
        /// Obtém ou define TpStatus.
        /// </summary>
        public string TpStatus { get; set; }

        /// <summary>
        /// Obtém ou define IdFornecedorParametro.
        /// </summary>
        public long IdFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define CdStatusVendor.
        /// </summary>
        public string CdStatusVendor { get; set; }

        /// <summary>
        /// Obtém ou define Sequencial.
        /// </summary>
        public int Sequencial { get; set; }

        /// <summary>
        /// Obtém ou define CdItemPrime.
        /// </summary>
        public long? CdItemPrime { get; set; }
    }
}
