using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo vinculado.
    /// </summary>
    public class TipoVinculado : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Tipo vinculado de saída ("S").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoVinculado Saida = new TipoVinculado("S");

        /// <summary>
        /// Tipo vinculado de entrada ("E").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoVinculado Entrada = new TipoVinculado("E");

        /// <summary>
        /// Tipo vinculado de não definido (null).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoVinculado NaoDefinido = new TipoVinculado(null);

        /// <summary>
        /// Todos os tipo vinculado.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoVinculado[] Todos = new TipoVinculado[] { TipoVinculado.Saida, TipoVinculado.Entrada, TipoVinculado.NaoDefinido, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoVinculado"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoVinculado(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoVinculado"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoVinculado(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoVinculado.NaoDefinido;
                }

                return TipoVinculado.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoVinculado).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
