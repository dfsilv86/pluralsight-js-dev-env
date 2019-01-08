using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa os possíveis valores para os tipos de importacao (automático, manual...).
    /// </summary>
    public class TipoProcessoImportacao : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Processamento automático.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoProcessoImportacao Automatico = new TipoProcessoImportacao(0);

        /// <summary>
        /// Processamento manual.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoProcessoImportacao Manual = new TipoProcessoImportacao(1);

        /// <summary>
        /// Todos os os tipos de origem de importação.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoProcessoImportacao[] Todos = new TipoProcessoImportacao[] { TipoProcessoImportacao.Automatico, TipoProcessoImportacao.Manual, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoProcessoImportacao"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoProcessoImportacao(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoProcessoImportacao"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoProcessoImportacao(int value)
        {
            try
            {
                return TipoProcessoImportacao.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoProcessoImportacao).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}