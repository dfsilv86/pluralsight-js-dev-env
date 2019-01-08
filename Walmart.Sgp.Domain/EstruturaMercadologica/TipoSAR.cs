using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa os possíveis valores para tipo de SAR.
    /// </summary>
    public class TipoSAR : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Tipo de SAR Yes ("Y").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSAR Yes = new TipoSAR("Y");

        /// <summary>
        /// Tipo de SAR Required ("R").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSAR Required = new TipoSAR("R");

        /// <summary>
        /// Tipo de SAR Manual ("M").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSAR Manual = new TipoSAR("M");

        /// <summary>
        /// Tipo de SAR No ("N").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSAR No = new TipoSAR("N");

        /// <summary>
        /// Tipo de SAR NaoDefinido (null).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSAR NaoDefinido = new TipoSAR(null);

        /// <summary>
        /// Todos os tipo de SAR.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoSAR[] Todos = new TipoSAR[] { TipoSAR.Yes, TipoSAR.Required, TipoSAR.Manual, TipoSAR.No, TipoSAR.NaoDefinido };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoSAR"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoSAR(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoSAR"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoSAR(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoSAR.NaoDefinido;
                }

                return TipoSAR.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoSAR).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}