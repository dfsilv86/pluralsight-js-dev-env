using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Classe NotaFiscalItemCustosConsolidado
    /// </summary>
    public class NotaFiscalItemCustosConsolidado : EntityBase
    {
        /// <summary>
        /// Obtém ou define vlUltimoCustoAtual.
        /// </summary>
        public decimal ultimoCustoAtual { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoInventario.
        /// </summary>
        public decimal custoInventario { get; set; }

        /// <summary>
        /// Obtém ou define posEstoqueAtual.
        /// </summary>
        public decimal posEstoqueAtual { get; set; }

        /// <summary>
        /// Obtém ou define vlUltimoCustoMesAnterior.
        /// </summary>
        public decimal ultCustoMesAnterior { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoContab.
        /// </summary>
        public decimal custoContab { get; set; }
    }
}
