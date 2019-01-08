using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Classe usada no grid da tela de Solicitacao de ajuste de custo
    /// </summary>
    public class NotaFiscalConsolidado : EntityBase
    {
        /// <summary>
        /// Obtém ou define nrNotaFiscal.
        /// </summary>
        public long? nrNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define dtEmissao.
        /// </summary>
        public DateTime? dtEmissao { get; set; }

        /// <summary>
        /// Obtém ou define dtRecebimento.
        /// </summary>
        public DateTime? dtRecebimento { get; set; }

        /// <summary>
        /// Obtém ou define cdItem.
        /// </summary>
        public int CdItem { get; set; }

        /// <summary>
        /// Obtém ou define dsItem.
        /// </summary>
        public string DsItem { get; set; }

        /// <summary>
        /// Obtém ou define vlCusto.
        /// </summary>
        public decimal vlCusto { get; set; }
    }
}
