using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa os possíveis valores para tipo de pedido mínimo.
    /// </summary>
    public class TipoPedidoMinimo : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Tipo de pedido mínimo não definido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoPedidoMinimo NaoDefinido = new TipoPedidoMinimo(null);

        /// <summary>
        /// Tipo de pedido mínimo de dinheiro ("$").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoPedidoMinimo Dinheiro = new TipoPedidoMinimo("$");

        /// <summary>
        /// Tipo de pedido mínimo de emcaixas ("C").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoPedidoMinimo EmCaixas = new TipoPedidoMinimo("C");

        /// <summary>
        /// Todos os tipo de pedido mínimo.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoPedidoMinimo[] Todos = new TipoPedidoMinimo[] { TipoPedidoMinimo.Dinheiro, TipoPedidoMinimo.EmCaixas, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoPedidoMinimo"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoPedidoMinimo(string value)
            : base(value)
        {
            // Como '$' não pode ser utilizado numa chave de globalização do Texts.resx, é preciso fazer esse tratamento.
            if (!string.IsNullOrEmpty(value) && value.Equals("$", StringComparison.OrdinalIgnoreCase))
            {
                Description = Texts.TipoPedidoMinimoFixedValueDinheiro;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoPedidoMinimo"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoPedidoMinimo(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return TipoPedidoMinimo.NaoDefinido;
            }

            // Segundo a Vivian Modanese, qualquer valor diferente de $ deve ser tratado como "Em caixa".
            return value.Equals("$", StringComparison.OrdinalIgnoreCase) ? TipoPedidoMinimo.Dinheiro : TipoPedidoMinimo.EmCaixas;           
        }
        #endregion
    }
}
