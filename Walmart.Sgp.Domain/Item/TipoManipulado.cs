using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo de manipulado.
    /// </summary>
    public class TipoManipulado : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo de manipulado de pai ("P").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoManipulado Pai = new TipoManipulado("P");

        /// <summary>
        /// Tipo de manipulado de derivado ("D").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoManipulado Derivado = new TipoManipulado("D");

        /// <summary>
        /// Tipo de manipulado de naodefinido (null).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoManipulado NaoDefinido = new TipoManipulado(null);

        /// <summary>
        /// Todos os tipo de manipulado.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoManipulado[] Todos = new TipoManipulado[] { TipoManipulado.Pai, TipoManipulado.Derivado, TipoManipulado.NaoDefinido, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoManipulado"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoManipulado(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoManipulado"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoManipulado(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoManipulado.NaoDefinido;
                }

                return TipoManipulado.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoManipulado).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
