using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa os possíveis valores para tipo de cd.
    /// </summary>
    public class TipoCD : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo de CD não definido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCD NaoDefinido = new TipoCD(null);

        /// <summary>
        /// Tipo de CD de seco (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCD Seco = new TipoCD("1");

        /// <summary>
        /// Tipo de CD de frio (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCD Frio = new TipoCD("2");

        /// <summary>
        /// Todos os tipo de cd.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoCD[] Todos = new TipoCD[] { TipoCD.Seco, TipoCD.Frio, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoCD"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoCD(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoCD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoCD(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoCD.NaoDefinido;
                }

                return TipoCD.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoCD).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}