using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa os possíveis valores para tipo de origem do cálculo de sugestão de pedido.
    /// </summary>
    public class TipoOrigemCalculo : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Origem INFOREM.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoOrigemCalculo Inforem = new TipoOrigemCalculo("I");

        /// <summary>
        /// Origem SGP.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoOrigemCalculo Sgp = new TipoOrigemCalculo("S");

        /// <summary>
        /// Origem GRS.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoOrigemCalculo Grs = new TipoOrigemCalculo("G");

        /// <summary>
        /// Origem manual.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoOrigemCalculo Manual = new TipoOrigemCalculo("M");

        /// <summary>
        /// Todos os tipos de origem do cálculo de sugestão de pedido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoOrigemCalculo[] Todos = new TipoOrigemCalculo[] { TipoOrigemCalculo.Inforem, TipoOrigemCalculo.Sgp, TipoOrigemCalculo.Grs, TipoOrigemCalculo.Manual };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoOrigemCalculo"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoOrigemCalculo(string value)
            : base(value)
        {
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
        public static implicit operator TipoOrigemCalculo(string value)
        {
            try
            {
                return TipoOrigemCalculo.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoOrigemCalculo).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
