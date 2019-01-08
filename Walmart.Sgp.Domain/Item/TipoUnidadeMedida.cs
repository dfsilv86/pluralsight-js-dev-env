using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para tipo de unidade de medida.
    /// </summary>
    public class TipoUnidadeMedida : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo de unidade de medida de quilo ("Q").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoUnidadeMedida Quilo = new TipoUnidadeMedida("Q");

        /// <summary>
        /// Tipo de unidade de medida de unidade ("U").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoUnidadeMedida Unidade = new TipoUnidadeMedida("U");

        /// <summary>
        /// Tipo de unidade de medida de nenhum ("").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoUnidadeMedida Nenhum = new TipoUnidadeMedida(null);

        /// <summary>
        /// Todos os tipo de unidade de medida.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoUnidadeMedida[] Todos = new TipoUnidadeMedida[] { TipoUnidadeMedida.Quilo, TipoUnidadeMedida.Unidade, TipoUnidadeMedida.Nenhum, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoUnidadeMedida"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoUnidadeMedida(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoUnidadeMedida"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoUnidadeMedida(string value)
        {
            try
            {
                // TODO: trocar essa verificação por uma extension para FixedValuesBase. Algo como TipoUnidadeMedida.Todos.Convert(value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoUnidadeMedida.Nenhum;
                }

                return TipoUnidadeMedida.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoUnidadeMedida).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}