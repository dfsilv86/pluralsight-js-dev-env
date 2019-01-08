using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo receituário.
    /// </summary>
    public class TipoReceituario : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>k
        /// Tipo receituário de transformado ("T").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoReceituario Transformado = new TipoReceituario("T");

        /// <summary>
        /// Tipo receituário de insumo ("I").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoReceituario Insumo = new TipoReceituario("I");

        /// <summary>
        /// Tipo receituário de naodefinido (null).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoReceituario NaoDefinido = new TipoReceituario(null);

        /// <summary>
        /// Todos os tipo receituário.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoReceituario[] Todos = new TipoReceituario[] { TipoReceituario.Transformado, TipoReceituario.Insumo, TipoReceituario.NaoDefinido, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoReceituario"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoReceituario(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoReceituario"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoReceituario(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoReceituario.NaoDefinido;
                }

                return TipoReceituario.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoReceituario).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
