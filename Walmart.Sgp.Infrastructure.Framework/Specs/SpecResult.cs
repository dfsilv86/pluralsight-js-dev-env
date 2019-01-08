using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// O resultado da execução de uma especificação.
    /// </summary>
    [DebuggerDisplay("{Satisfied} - {Reason}")]
    public class SpecResult
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SpecResult"/>.
        /// </summary>
        /// <param name="satisfied">Se a especificação foi satisfeita.</param>
        /// <param name="reason">A razão do resultado.</param>
        public SpecResult(bool satisfied, string reason = null)
        {
            Satisfied = satisfied;
            Reason = reason;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém um valor que indica se <see cref="SpecResult"/> é satisfeita.
        /// </summary>
        public bool Satisfied { get; private set; }

        /// <summary>
        /// Obtém the razão.
        /// </summary>
        public string Reason { get; private set; }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="SpecResult"/> to <see cref="bool"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator bool(SpecResult result)
        {
            return result.Satisfied;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SpecResult"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(SpecResult result)
        {
            return result.Reason;
        }
        #endregion
    }
}
