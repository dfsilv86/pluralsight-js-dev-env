using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo status do item.
    /// </summary>
    public class TipoStatusItem : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Tipo status do item de ativo ("A").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusItem Ativo = new TipoStatusItem("A");

        /// <summary>
        /// Tipo status do item de deletado ("D").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusItem Deletado = new TipoStatusItem("D");

        /// <summary>
        /// Tipo status do item de inativo ("I").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusItem Inativo = new TipoStatusItem("I");

        /// <summary>
        /// Tipo status do item não definido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusItem NaoDefinido = new TipoStatusItem(null);

        /// <summary>
        /// Todos os tipo status do item.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoStatusItem[] Todos = new TipoStatusItem[] { TipoStatusItem.Ativo, TipoStatusItem.Deletado, TipoStatusItem.Inativo, TipoStatusItem.NaoDefinido };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoStatusItem"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoStatusItem(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoStatusItem"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoStatusItem(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || value.Equals("\0", StringComparison.OrdinalIgnoreCase))
                {
                    value = null;
                }

                return TipoStatusItem.Todos.Single(t => t.Value == value || t.Description == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoStatusItem).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
