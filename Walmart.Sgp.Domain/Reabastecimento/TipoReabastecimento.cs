using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa os possíveis valores para tipo de reabastecimento.
    /// </summary>
    public class TipoReabastecimento : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo de reabastecimento de cross ("C").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoReabastecimento Cross = new TipoReabastecimento("C");

        /// <summary>
        /// Tipo de reabastecimento de staple ("S").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoReabastecimento Staple = new TipoReabastecimento("S");

        /// <summary>
        /// Todos os tipo de reabastecimento.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoReabastecimento[] Todos = new TipoReabastecimento[] { TipoReabastecimento.Cross, TipoReabastecimento.Staple, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoReabastecimento"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoReabastecimento(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoReabastecimento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoReabastecimento(string value)
        {
            try
            {
                return TipoReabastecimento.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoReabastecimento).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
