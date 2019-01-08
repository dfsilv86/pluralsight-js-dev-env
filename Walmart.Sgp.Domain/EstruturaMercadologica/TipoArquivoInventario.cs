using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa os possíveis valores para tipo de arquivo de inventário.
    /// </summary>
    public class TipoArquivoInventario : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Tipo de arquivo de inventário de nenhum (0).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoArquivoInventario Nenhum = new TipoArquivoInventario(0);

        /// <summary>
        /// Tipo de arquivo de inventário de final (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoArquivoInventario Final = new TipoArquivoInventario(1);

        /// <summary>
        /// Tipo de arquivo de inventário de parcial (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoArquivoInventario Parcial = new TipoArquivoInventario(2);

        /// <summary>
        /// Todos os tipo de arquivo de inventário.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoArquivoInventario[] Todos = new TipoArquivoInventario[] { TipoArquivoInventario.Nenhum, TipoArquivoInventario.Final, TipoArquivoInventario.Parcial, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoArquivoInventario"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoArquivoInventario(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoArquivoInventario"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoArquivoInventario(int value)
        {
            try
            {
                return TipoArquivoInventario.Todos.Single(tr => tr.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(Texts.InvalidInventoryFileType, ex);
            }
        }
        #endregion
    }
}