using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa os possíveis valores para categoria de tipo de movimentação.
    /// </summary>
    public class CategoriaTipoMovimentacao : FixedValuesBase<int>
    {
        #region Fields
        /// <summary>
        /// Categoria de tipo de movimentação não definida (0).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly CategoriaTipoMovimentacao NaoDefinida = new CategoriaTipoMovimentacao(0);

        /// <summary>
        /// Categoria de tipo de movimentação de ajustedeestoque (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly CategoriaTipoMovimentacao AjusteDeEstoque = new CategoriaTipoMovimentacao(1);

        /// <summary>
        /// Categoria de tipo de movimentação de quebra (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly CategoriaTipoMovimentacao Quebra = new CategoriaTipoMovimentacao(2);

        /// <summary>
        /// Todos os categoria de tipo de movimentação.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly CategoriaTipoMovimentacao[] Todos = new CategoriaTipoMovimentacao[] { CategoriaTipoMovimentacao.NaoDefinida, CategoriaTipoMovimentacao.AjusteDeEstoque, CategoriaTipoMovimentacao.Quebra, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CategoriaTipoMovimentacao"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private CategoriaTipoMovimentacao(int value)
            : base(value)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém os ids dos tipos movimentação da categoria.
        /// </summary>
        /// <param name="categoria">A categoria.</param>
        /// <returns>Os ids.</returns>
        public static int[] ObterIdsTipoMovimentacao(CategoriaTipoMovimentacao categoria)
        {
            // Infelizmente, não existe uma maneira melhor de descobrir quais são os registros de TipoMovimentacao que são para
            // ajuste de estoque, ou quebra, etc.
            // TODO: adicionar coluna em TipoMovimentacao para marcar os registros que são utilizados por cada categoria.
            categoria = categoria ?? CategoriaTipoMovimentacao.NaoDefinida;

            switch (categoria.Value)
            {
                case 1: // AjusteDeEstoque
                    return new int[] { 15, 16 };

                case 2: // Quebra
                    return new int[] { 11, 12, 13 };

                default:
                    throw new InvalidOperationException(Texts.NoIdsMapForMovementTypeCategory.With(categoria));
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="CategoriaTipoMovimentacao"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator CategoriaTipoMovimentacao(int value)
        {
            try
            {
                return CategoriaTipoMovimentacao.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(CategoriaTipoMovimentacao).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
