using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Classe base para especificações.
    /// </summary>
    /// <remarks>Specification Pattern: http://en.wikipedia.org/wiki/Specification_pattern.</remarks>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public abstract class SpecBase<TTarget> : ISpec<TTarget>
    {
        #region Fields
        /// <summary>
        ///  Resultado quando uma especificação é satisfeita.
        /// </summary>
        private static readonly SpecResult SatisfiedDefault = new SpecResult(true);
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public abstract SpecResult IsSatisfiedBy(TTarget target);

        /// <summary>
        /// Cria um resultado de especificação satisfeita.
        /// </summary>
        /// <returns>O resultado da especificação.</returns>
        protected static SpecResult Satisfied()
        {
            return SatisfiedDefault;
        }

        /// <summary>
        /// Cria um resultado de especificação não satisfeita.
        /// </summary>
        /// <param name="reason">A razão da especificação não ter sido satisfeita.</param>
        /// <returns>O resultado da especificação.</returns>
        protected static SpecResult NotSatisfied(string reason)
        {
            return new SpecResult(false, reason);
        }        
        #endregion
    }
}
