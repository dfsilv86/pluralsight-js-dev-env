using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores para status de reprocessamento de custo.
    /// </summary>
    public class StatusReprocessamentoCusto : FixedValuesBase<string>
    {
        #region Fields

        /// <summary>
        /// Status de reprocessamento de custo de agendado ("A").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly StatusReprocessamentoCusto Agendado = new StatusReprocessamentoCusto("A");

        /// <summary>
        /// Status de reprocessamento de custo de processando ("P").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly StatusReprocessamentoCusto Processando = new StatusReprocessamentoCusto("P");

        /// <summary>
        /// Status de reprocessamento de custo de reprocessado ("R").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly StatusReprocessamentoCusto Reprocessado = new StatusReprocessamentoCusto("R");

        /// <summary>
        /// Status de reprocessamento de custo de erro ("E").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly StatusReprocessamentoCusto Erro = new StatusReprocessamentoCusto("E");

        /// <summary>
        /// Status de reprocessamento de custo de concluído ("C").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly StatusReprocessamentoCusto Concluido = new StatusReprocessamentoCusto("C");

        /// <summary>
        /// Status de reprocessamento de custo de concluído ("").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly StatusReprocessamentoCusto Nenhum = new StatusReprocessamentoCusto(null);

        /// <summary>
        /// Todos os status de reprocessamento de custo.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly StatusReprocessamentoCusto[] Todos = new StatusReprocessamentoCusto[] { StatusReprocessamentoCusto.Agendado, StatusReprocessamentoCusto.Processando, StatusReprocessamentoCusto.Reprocessado, StatusReprocessamentoCusto.Erro, StatusReprocessamentoCusto.Concluido, StatusReprocessamentoCusto.Nenhum };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="StatusReprocessamentoCusto"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private StatusReprocessamentoCusto(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="StatusReprocessamentoCusto"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator StatusReprocessamentoCusto(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return StatusReprocessamentoCusto.Nenhum;
                }

                return StatusReprocessamentoCusto.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(StatusReprocessamentoCusto).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}