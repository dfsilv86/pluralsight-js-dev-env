using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os possíveis valores para tipo de semana.
    /// </summary>
    public class TipoSemana : FixedValuesBase<short?>
    {
        #region Fields
        /// <summary>
        /// Tipo de semana não definida (null)
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSemana NaoDefinida = new TipoSemana(null);

        /// <summary>
        /// Tipo de semana de ímpar (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSemana Impar = new TipoSemana(1);

        /// <summary>
        /// Tipo de semana de par (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSemana Par = new TipoSemana(2);

        /// <summary>
        /// Todos os tipo de semana.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoSemana[] Todos = new TipoSemana[] { TipoSemana.NaoDefinida, TipoSemana.Impar, TipoSemana.Par, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoSemana"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoSemana(short? value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoSemana"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoSemana(short? value)
        {
            try
            {
                if (!value.HasValue || value.Value == 0)
                {
                    return TipoSemana.NaoDefinida;
                }

                return TipoSemana.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                   Texts.InvalidFixedValue.With(value, typeof(TipoSemana).Name, Todos.Where(i => i.Value.HasValue).Select(i => i.Value.Value.ToString(CultureInfo.InvariantCulture)).JoinWords()),
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
        public static implicit operator TipoSemana(short value)
        {
            return (TipoSemana)(new short?(value));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="TipoSemana"/> to <see cref="short"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static explicit operator short?(TipoSemana value)
        {
            return value == null ? new short?() : value.Value;
        }
        #endregion
    }
}
