using System;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma SugestaoPedidoCDFiltro.
    /// </summary>
    public class SugestaoPedidoCDFiltro
    {
        /// <summary>
        /// Obtém ou define o DtSolicitacao.
        /// </summary>
        public DateTime DtSolicitacao { get; set; }

        /// <summary>
        /// Obtém ou define o IdDepartamento.
        /// </summary>
        public int IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o IdCD.
        /// </summary>
        public int IdCD { get; set; }

        /// <summary>
        /// Obtém ou define o IdItem.
        /// </summary>
        public int? IdItem { get; set; }

        /// <summary>
        /// Obtém ou define o IdFornecedorParametro.
        /// </summary>
        public int? IdFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define o StatusPedido.
        /// </summary>
        public int? StatusPedido { get; set; }

        /// <summary>
        /// Obtém ou define o ItemPesoVariavel.
        /// </summary>
        public int? ItemPesoVariavel { get; set; }
    }
}
