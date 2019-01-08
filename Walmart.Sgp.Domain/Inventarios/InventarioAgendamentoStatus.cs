using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa os possíveis valores para os status de agentamento de um inventário.
    /// </summary>
    public class InventarioAgendamentoStatus : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Os status de agentamento de um inventário de agendado (0).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioAgendamentoStatus Agendado = new InventarioAgendamentoStatus(0);

        /// <summary>
        /// Os status de agentamento de um inventário de executado (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioAgendamentoStatus Executado = new InventarioAgendamentoStatus(1);

        /// <summary>
        /// Os status de agentamento de um inventário de cancelado (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly InventarioAgendamentoStatus Cancelado = new InventarioAgendamentoStatus(2);

        /// <summary>
        /// Todos os os status de agentamento de um inventário.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly InventarioAgendamentoStatus[] Todos = new InventarioAgendamentoStatus[] { InventarioAgendamentoStatus.Agendado, InventarioAgendamentoStatus.Executado, InventarioAgendamentoStatus.Cancelado, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioAgendamentoStatus"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private InventarioAgendamentoStatus(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="InventarioAgendamentoStatus"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator InventarioAgendamentoStatus(int value)
        {
            try
            {
                return InventarioAgendamentoStatus.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(InventarioAgendamentoStatus).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}