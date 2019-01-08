using System;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Quantidades de sugestões de pedidos em um determinado dia.
    /// </summary>
    public class QuantidadeSugestaoPedido
    {
        /// <summary>
        /// Obtém ou define o dia das quantidades;
        /// </summary>
        public DateTime Dia { get; set; }

        /// <summary>
        /// Obtém ou define o total de sugestões de pedidos.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Obtém ou define o total de sugestões de pedidos com origem cálculo Manual.
        /// </summary>
        public long TotalOrigemCalculoManual { get; set; }

        /// <summary>
        /// Obtém o total de sugestões de pedidos com origem cálculo diferente de Manual.
        /// </summary>
        public long TotalOrigemCalculoNaoManual
        {
            get
            {
                return Total - TotalOrigemCalculoManual;
            }
        }
    }
}
