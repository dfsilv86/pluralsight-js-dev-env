using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa os possíveis valores para os tipos de importacao.
    /// </summary>
    public class TipoImportacao : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Os tipos de importacao de automatico ("A").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoImportacao Automatico = new TipoImportacao("A");

        /// <summary>
        /// Os tipos de importacao de manual ("M").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoImportacao Manual = new TipoImportacao("M");

        /// <summary>
        /// Todos os os tipos de importacao.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoImportacao[] Todos = new TipoImportacao[] { TipoImportacao.Automatico, TipoImportacao.Manual, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoImportacao"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoImportacao(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoImportacao"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoImportacao(string value)
        {
            try
            {
                return TipoImportacao.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoImportacao).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}