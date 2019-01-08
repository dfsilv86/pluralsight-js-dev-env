using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os possíveis valores para tipo de intervalo.
    /// </summary>
    public class TipoIntervalo : FixedValuesBase<short?>
    {
        #region Fields
        /// <summary>
        /// Tipo de intervalo não definido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoIntervalo NaoDefinido = new TipoIntervalo(null);

        /// <summary>
        /// Tipo de intervalo de semanal (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoIntervalo Semanal = new TipoIntervalo(1);

        /// <summary>
        /// Tipo de intervalo de quinzenal (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoIntervalo Quinzenal = new TipoIntervalo(2);

        /// <summary>
        /// Todos os tipo de intervalo.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoIntervalo[] Todos = new TipoIntervalo[] { TipoIntervalo.NaoDefinido, TipoIntervalo.Semanal, TipoIntervalo.Quinzenal, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoIntervalo"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoIntervalo(short? value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoIntervalo"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoIntervalo(short? value)
        {
            try
            {
                if (!value.HasValue || value.Value == 0)
                {
                    return TipoIntervalo.NaoDefinido;
                }

                return TipoIntervalo.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoIntervalo).Name, Todos.Where(i => i.Value.HasValue).Select(i => i.Value.Value.ToString(CultureInfo.InvariantCulture)).JoinWords()),
                    ex);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="short"/> to <see cref="TipoSemana"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoIntervalo(short value)
        {
            return (TipoIntervalo)(new short?(value));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="TipoSemana"/> to <see cref="short"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static explicit operator short?(TipoIntervalo value)
        {
            return value == null ? new short?() : value.Value;
        }
        #endregion
    }
}
