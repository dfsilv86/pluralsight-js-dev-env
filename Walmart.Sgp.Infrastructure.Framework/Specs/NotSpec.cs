using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação utilizada para negar o resultado de outra especificação.
    /// </summary>
    /// <typeparam name="TTarget">O tipo do alvo da especificação.</typeparam>
    public class NotSpec<TTarget> : SpecBase<TTarget>
    {
        #region Fields
        private ISpec<TTarget> m_underlyingSpec;
        private string m_reason;
        #endregion

        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotSpec{TTarget}"/>.
        /// </summary>
        /// <param name="underlyingSpec">A especificação que será negada.</param>
        /// <param name="reason">The razão.</param>
        public NotSpec(ISpec<TTarget> underlyingSpec, string reason)
        {
            m_underlyingSpec = underlyingSpec;
            m_reason = reason;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o target informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O target.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo target.
        /// </returns>
        public override SpecResult IsSatisfiedBy(TTarget target)
        {
            var result = m_underlyingSpec.IsSatisfiedBy(target);

            return new SpecResult(!result.Satisfied, m_reason);
        }
        #endregion
    }
}
