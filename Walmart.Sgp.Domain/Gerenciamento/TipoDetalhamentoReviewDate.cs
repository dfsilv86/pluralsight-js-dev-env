using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os tipos de detalhamento na tela de detalhe de parametros do fornecedor.
    /// </summary>
    public class TipoDetalhamentoReviewDate : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo detalhamento Loja.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoDetalhamentoReviewDate Loja = new TipoDetalhamentoReviewDate("Loja");

        /// <summary>
        /// Tipo detalhamento CD.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoDetalhamentoReviewDate Cd = new TipoDetalhamentoReviewDate("CD");

        /// <summary>
        /// Tipo detalhamento Todos.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoDetalhamentoReviewDate NaoDefinido = new TipoDetalhamentoReviewDate("Todos");

        /// <summary>
        /// Todos os tipos de detalhamento.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoDetalhamentoReviewDate[] Todos = new[] 
        {
            TipoDetalhamentoReviewDate.NaoDefinido,
            TipoDetalhamentoReviewDate.Loja,
            TipoDetalhamentoReviewDate.Cd
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoDetalhamentoReviewDate"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoDetalhamentoReviewDate(string value)
            : base(value)
        {
        }

        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoDetalhamentoReviewDate"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoDetalhamentoReviewDate(string value)
        {
            try
            {                
                return TipoDetalhamentoReviewDate.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoDetalhamentoReviewDate).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }

        #endregion
    }
}
