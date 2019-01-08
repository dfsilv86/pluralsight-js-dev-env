using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa os possíveis valores para os status de um inventário.
    /// </summary>
    public class InventarioStatus : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Os status de um inventário de aberto (0).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioStatus Aberto = new InventarioStatus(0);

        /// <summary>
        /// Os status de um inventário de importado (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioStatus Importado = new InventarioStatus(1);

        /// <summary>
        /// Os status de um inventário de aprovado (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioStatus Aprovado = new InventarioStatus(2);

        /// <summary>
        /// Os status de um inventário de finalizado (3).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioStatus Finalizado = new InventarioStatus(3);

        /// <summary>
        /// Os status de um inventário de contabilizado (4).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioStatus Contabilizado = new InventarioStatus(4);

        /// <summary>
        /// Os status de um inventário de cancelado (5).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioStatus Cancelado = new InventarioStatus(5);

        /// <summary>
        /// Todos os os status de um inventário.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly InventarioStatus[] Todos = new InventarioStatus[] { InventarioStatus.Aberto, InventarioStatus.Importado, InventarioStatus.Aprovado, InventarioStatus.Finalizado, InventarioStatus.Contabilizado, InventarioStatus.Cancelado, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioStatus"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private InventarioStatus(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="InventarioStatus"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator InventarioStatus(int value)
        {
            try
            {
                return InventarioStatus.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(InventarioStatus).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }

        /// <summary>
        /// Converte um objeto do tipo <see cref="InventarioStatus" /> para um <see cref="System.Nullable{Int32}"/>
        /// </summary>
        /// <param name="value">O valor original.</param>
        /// <returns>O objeto convertido.</returns>
        public static implicit operator int?(InventarioStatus value)
        {
            return ReferenceEquals(null, value) ? (int?)null : value.Value;
        }
        #endregion
    }
}