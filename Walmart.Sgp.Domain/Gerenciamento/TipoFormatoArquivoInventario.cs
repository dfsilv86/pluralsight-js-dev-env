using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os possíveis valores para os tipos de importacao.
    /// </summary>
    public class TipoFormatoArquivoInventario : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Os tipos de importacao de automatico ("A").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFormatoArquivoInventario Rtl = new TipoFormatoArquivoInventario("1");

        /// <summary>
        /// Os tipos de importacao de manual ("M").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFormatoArquivoInventario Pipe = new TipoFormatoArquivoInventario("2");  // aparentemente é 2: WalMart.SGP.ArchiveImport.BL.cs linha 294

        /// <summary>
        /// Todos os os tipos de importacao.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoFormatoArquivoInventario[] Todos = new TipoFormatoArquivoInventario[] { TipoFormatoArquivoInventario.Rtl, TipoFormatoArquivoInventario.Pipe, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoFormatoArquivoInventario"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoFormatoArquivoInventario(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoFormatoArquivoInventario"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoFormatoArquivoInventario(string value)
        {
            try
            {
                return TipoFormatoArquivoInventario.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoFormatoArquivoInventario).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}