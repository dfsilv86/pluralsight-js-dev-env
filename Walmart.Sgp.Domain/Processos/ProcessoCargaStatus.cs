using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa os possíveis valores para os status de uma carga de processo..
    /// </summary>
    public class ProcessoCargaStatus : FixedValuesBase<string>
    {
        #region Fields
        /// <summary>
        /// Os status de uma carga de processo. de naoiniciado ("NaoIniciado").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ProcessoCargaStatus NaoIniciado = new ProcessoCargaStatus("NaoIniciado");

        /// <summary>
        /// Os status de uma carga de processo. de emandamento ("EmAndamento").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ProcessoCargaStatus EmAndamento = new ProcessoCargaStatus("EmAndamento");

        /// <summary>
        /// Os status de uma carga de processo. de concluido ("Concluido").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ProcessoCargaStatus Concluido = new ProcessoCargaStatus("Concluido");

        /// <summary>
        /// Os status de uma carga de processo. de erro ("Erro").
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ProcessoCargaStatus Erro = new ProcessoCargaStatus("Erro");

        /// <summary>
        /// Todos os os status de uma carga de processo..
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly ProcessoCargaStatus[] Todos = new ProcessoCargaStatus[] { ProcessoCargaStatus.NaoIniciado, ProcessoCargaStatus.EmAndamento, ProcessoCargaStatus.Concluido, ProcessoCargaStatus.Erro, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessoCargaStatus"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private ProcessoCargaStatus(string value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="ProcessoCargaStatus"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator ProcessoCargaStatus(string value)
        {
            try
            {
                return ProcessoCargaStatus.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(ProcessoCargaStatus).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}