using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa os possíveis valores para tipo de TipoCaixaFornecedor.
    /// </summary>
    public class TipoCaixaFornecedor : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Tipo de TipoCaixaFornecedor Caixa ("F").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCaixaFornecedor Caixa = new TipoCaixaFornecedor("F");

        /// <summary>
        /// Tipo de TipoCaixaFornecedor KgOuUnidade ("V").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCaixaFornecedor KgOuUnidade = new TipoCaixaFornecedor("V");

        /// <summary>
        /// Tipo de TipoCaixaFornecedor NaoDefinido (null).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCaixaFornecedor NaoDefinido = new TipoCaixaFornecedor(null);

        /// <summary>
        /// Todos os tipo de SAR.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoCaixaFornecedor[] Todos = new TipoCaixaFornecedor[] { TipoCaixaFornecedor.Caixa, TipoCaixaFornecedor.KgOuUnidade, TipoCaixaFornecedor.NaoDefinido };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoCaixaFornecedor"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoCaixaFornecedor(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoCaixaFornecedor"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoCaixaFornecedor(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoCaixaFornecedor.NaoDefinido;
                }

                return TipoCaixaFornecedor.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoCaixaFornecedor).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}