using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa os possíveis valores para tipo de movimento.
    /// </summary>
    public class TipoMovimento : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Tipo de movimento de nenhum (N).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimento Nenhum = new TipoMovimento("N");

        /// <summary>
        /// Tipo de movimento de entrada (E).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimento Entrada = new TipoMovimento("E");

        /// <summary>
        /// Tipo de movimento de saida (S).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimento Saida = new TipoMovimento("S");

        /// <summary>
        /// Tipo de movimento de ajusteinventario (I).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimento AjusteInventario = new TipoMovimento("I");

        /// <summary>
        /// Todos os tipo de movimento.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoMovimento[] Todos = new TipoMovimento[] { TipoMovimento.Nenhum, TipoMovimento.Entrada, TipoMovimento.Saida, TipoMovimento.AjusteInventario, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoMovimento"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoMovimento(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoMovimento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoMovimento(string value)
        {
            try
            {
                return TipoMovimento.Todos.Single(tr => tr.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(Texts.InvalidMovementType, ex);
            }
        }
        #endregion
    }
}
