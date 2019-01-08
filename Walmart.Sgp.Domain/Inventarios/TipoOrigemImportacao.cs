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
    public class TipoOrigemImportacao : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Origem loja.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoOrigemImportacao Loja = new TipoOrigemImportacao(0);

        /// <summary>
        /// Origem HO.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoOrigemImportacao HO = new TipoOrigemImportacao(1);

        /// <summary>
        /// Todos os os tipos de origem de importação.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoOrigemImportacao[] Todos = new TipoOrigemImportacao[] { TipoOrigemImportacao.Loja, TipoOrigemImportacao.HO, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoOrigemImportacao"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoOrigemImportacao(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoOrigemImportacao"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoOrigemImportacao(int value)
        {
            try
            {
                return TipoOrigemImportacao.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoOrigemImportacao).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}