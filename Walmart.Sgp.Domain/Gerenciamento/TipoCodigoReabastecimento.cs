using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa os possíveis valores para código de reabastecimento.
    /// </summary>
    public class TipoCodigoReabastecimento : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Tipo código WOG (W).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCodigoReabastecimento Wog = new TipoCodigoReabastecimento("W");

        /// <summary>
        /// Tipo código ALL (L).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCodigoReabastecimento All = new TipoCodigoReabastecimento("L");

        /// <summary>
        /// Tipo código DAO (D).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCodigoReabastecimento Dao = new TipoCodigoReabastecimento("D");

        /// <summary>
        /// Tipo código não definido.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoCodigoReabastecimento NaoDefinido = new TipoCodigoReabastecimento(null);

        /// <summary>
        /// Todos os tipos de código.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoCodigoReabastecimento[] Todos = new[] { TipoCodigoReabastecimento.Wog, TipoCodigoReabastecimento.All, TipoCodigoReabastecimento.Dao, TipoCodigoReabastecimento.NaoDefinido };

        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoCodigoReabastecimento"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoCodigoReabastecimento(string value)
            : base(value)
        {
        }

        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="TipoCodigoReabastecimento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoCodigoReabastecimento(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return TipoCodigoReabastecimento.NaoDefinido;
                }

                return TipoCodigoReabastecimento.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoCodigoReabastecimento).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}
