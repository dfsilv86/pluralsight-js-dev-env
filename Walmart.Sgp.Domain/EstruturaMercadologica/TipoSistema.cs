using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa os possíveis valores para tipo de sistema.
    /// </summary>
    public class TipoSistema : FixedValuesBase<int>
    {
        #region Fields

        /// <summary>
        /// Tipo de sistema de supercenter (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSistema Supercenter = new TipoSistema(1);

        /// <summary>
        /// Tipo de sistema de samsclub (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoSistema SamsClub = new TipoSistema(2);

        /// <summary>
        /// Todos os tipo de sistema.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoSistema[] Todos = new TipoSistema[] { TipoSistema.Supercenter, TipoSistema.SamsClub, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoSistema"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoSistema(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoSistema"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoSistema(int value)
        {
            try
            {
                return TipoSistema.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoSistema).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}