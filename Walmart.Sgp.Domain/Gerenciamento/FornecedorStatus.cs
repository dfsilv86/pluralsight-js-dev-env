using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os possíveis valores para os status do fornecedor.
    /// </summary>
    public class FornecedorStatus : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Fornecedor status ativo ("A").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly FornecedorStatus Ativo = new FornecedorStatus("A");

        /// <summary>
        /// Fornecedor status deletado ("D").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly FornecedorStatus Deletado = new FornecedorStatus("D");

        /// <summary>
        /// Fornecedor status inativo ("I").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly FornecedorStatus Inativo = new FornecedorStatus("I");

        /// <summary>
        /// Tipo status do item não definido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly FornecedorStatus NaoDefinido = new FornecedorStatus(null);

        /// <summary>
        /// Todos os tipo status do item.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly FornecedorStatus[] Todos = new FornecedorStatus[] { FornecedorStatus.Ativo, FornecedorStatus.Deletado, FornecedorStatus.Inativo, FornecedorStatus.NaoDefinido };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FornecedorStatus"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private FornecedorStatus(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="FornecedorStatus"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator FornecedorStatus(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || value.Equals("\0", StringComparison.OrdinalIgnoreCase))
                {
                    value = null;
                }

                return FornecedorStatus.Todos.Single(t => t.Value == value || t.Description == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(FornecedorStatus).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
