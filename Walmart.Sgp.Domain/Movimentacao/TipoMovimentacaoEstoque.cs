using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa os possíveis valores para tipo de movimentação de estoque.
    /// </summary>
    public class TipoMovimentacaoEstoque : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo de movimentação de estoque de ajuste de inventário ("I").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimentacaoEstoque AjusteInventario = new TipoMovimentacaoEstoque("I");

        /// <summary>
        /// Tipo de movimentação de estoque de entrada ("E").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimentacaoEstoque Entrada = new TipoMovimentacaoEstoque("E");

        /// <summary>
        /// Tipo de movimentação de estoque de saída ("S").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoMovimentacaoEstoque Saida = new TipoMovimentacaoEstoque("S");

        /// <summary>
        /// Todos os tipo de movimentação de estoque.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoMovimentacaoEstoque[] Todos = new TipoMovimentacaoEstoque[] { TipoMovimentacaoEstoque.AjusteInventario, TipoMovimentacaoEstoque.Entrada, TipoMovimentacaoEstoque.Saida, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoMovimentacaoEstoque"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoMovimentacaoEstoque(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoMovimentacaoEstoque"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoMovimentacaoEstoque(string value)
        {
            try
            {
                return TipoMovimentacaoEstoque.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoMovimentacaoEstoque).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}