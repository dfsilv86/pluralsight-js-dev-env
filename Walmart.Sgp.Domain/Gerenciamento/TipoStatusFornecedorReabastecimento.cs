using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os possíveis valores para status vendor reabastecimento.
    /// </summary>
    public class TipoStatusFornecedorReabastecimento : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo status Vendor Reabastecimento Inativo (I).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusFornecedorReabastecimento Inativo = new TipoStatusFornecedorReabastecimento("I");

        /// <summary>
        /// Tipo status Vendor Reabastecimento Ativo (A).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusFornecedorReabastecimento Ativo = new TipoStatusFornecedorReabastecimento("A");

        /// <summary>
        /// Tipo status Vendor Reabastecimento Inativo (N).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusFornecedorReabastecimento InativoN = new TipoStatusFornecedorReabastecimento("N");

        /// <summary>
        /// Tipo status Vendor Reabastecimento Inativo (D).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusFornecedorReabastecimento InativoD = new TipoStatusFornecedorReabastecimento("D");

        /// <summary>
        /// Tipo status Vendor Reabastecimento Inativo (I).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoStatusFornecedorReabastecimento NaoDefinido = new TipoStatusFornecedorReabastecimento(null);

        /// <summary>
        /// Todos os status vendor reabastecimento.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoStatusFornecedorReabastecimento[] Todos = new[] 
        { 
            TipoStatusFornecedorReabastecimento.Inativo,
            TipoStatusFornecedorReabastecimento.Ativo,
            TipoStatusFornecedorReabastecimento.InativoN,
            TipoStatusFornecedorReabastecimento.InativoD
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoStatusFornecedorReabastecimento"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoStatusFornecedorReabastecimento(string value)
            : base(value)
        {
        }

        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoStatusFornecedorReabastecimento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoStatusFornecedorReabastecimento(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoStatusFornecedorReabastecimento.NaoDefinido;
                }

                return TipoStatusFornecedorReabastecimento.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoStatusFornecedorReabastecimento).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }

        #endregion
    }
}
