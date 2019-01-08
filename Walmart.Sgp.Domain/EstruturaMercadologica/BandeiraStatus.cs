using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa os possíveis valores para os status de uma bandeira..
    /// </summary>
    public class BandeiraStatus : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Os status de uma bandeira. de ativo ("S").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly BandeiraStatus Ativo = new BandeiraStatus("S");

        /// <summary>
        /// Os status de uma bandeira. de inativo ("N").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly BandeiraStatus Inativo = new BandeiraStatus("N");

        /// <summary>
        /// Todos os os status de uma bandeira..
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly BandeiraStatus[] Todos = new BandeiraStatus[] { BandeiraStatus.Ativo, BandeiraStatus.Inativo, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="BandeiraStatus"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private BandeiraStatus(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="BandeiraStatus"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator BandeiraStatus(string value)
        {
            try
            {
                return BandeiraStatus.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(BandeiraStatus).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}