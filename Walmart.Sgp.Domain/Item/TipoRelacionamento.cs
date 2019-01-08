using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo de relacionamento entre itens..
    /// </summary>
    public class TipoRelacionamento : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Tipo de relacionamento entre itens. de vinculado (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoRelacionamento Vinculado = new TipoRelacionamento(1);

        /// <summary>
        /// Tipo de relacionamento entre itens. de receituário (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoRelacionamento Receituario = new TipoRelacionamento(2);

        /// <summary>
        /// Tipo de relacionamento entre itens. de manipulado (3).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoRelacionamento Manipulado = new TipoRelacionamento(3);

        /// <summary>
        /// Todos os tipo de relacionamento entre itens..
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoRelacionamento[] Todos = new TipoRelacionamento[] { TipoRelacionamento.Vinculado, TipoRelacionamento.Receituario, TipoRelacionamento.Manipulado, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoRelacionamento"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoRelacionamento(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoRelacionamento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoRelacionamento(int value)
        {
            try
            {
                return TipoRelacionamento.Todos.Single(tr => tr.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(Texts.InvalidItemRelationshipType, ex);
            }
        }
        #endregion
    }
}
