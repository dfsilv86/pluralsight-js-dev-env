using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo de item de entrada.
    /// </summary>
    public class TipoItemEntrada : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Tipo de item de entrada de insumo (0).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoItemEntrada Insumo = new TipoItemEntrada(0);

        /// <summary>
        /// Tipo de item de entrada de embalagem (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoItemEntrada Embalagem = new TipoItemEntrada(1);

        /// <summary>
        /// Todos os tipo de item de entrada.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoItemEntrada[] Todos = new TipoItemEntrada[] { TipoItemEntrada.Insumo, TipoItemEntrada.Embalagem, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoItemEntrada"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoItemEntrada(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoItemEntrada"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoItemEntrada(int value)
        {
            try
            {
                return TipoItemEntrada.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoItemEntrada).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
