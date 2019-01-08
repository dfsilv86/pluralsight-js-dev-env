using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Representa os tipos de registro de log de auditoria.
    /// </summary>
    public class AuditKind : FixedValuesBase<int>
    {
        #region Fields

        /// <summary>
        /// Log de Insert.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly AuditKind Insert = new AuditKind(1);

        /// <summary>
        /// Log de Update.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly AuditKind Update = new AuditKind(2);

        /// <summary>
        /// Log de Delete.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly AuditKind Delete = new AuditKind(3);

        /// <summary>
        /// Todos os tipos de log.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly AuditKind[] Todos = new AuditKind[] { AuditKind.Insert, AuditKind.Update, AuditKind.Delete, };

        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AuditKind"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private AuditKind(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="AuditKind"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator AuditKind(int value)
        {
            try
            {
                return AuditKind.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(AuditKind).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
