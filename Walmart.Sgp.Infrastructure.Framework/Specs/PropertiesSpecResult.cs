using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// O resultado de uma especificação que utiliza propriedades para analisar o alvo.
    /// </summary>
    public class PropertiesSpecResult : SpecResult
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PropertiesSpecResult"/>.
        /// </summary>
        /// <param name="allProperties">Todas as propriedades do alvo.</param>
        /// <param name="satisfiedProperties">As propriedades do alvo que satisfizeram a especificação.</param>
        /// <param name="satisfied">Se for <c>true</c> está satisfeita.</param>
        /// <param name="reason">A razão.</param>
        public PropertiesSpecResult(IEnumerable<PropertyInfo> allProperties, IEnumerable<PropertyInfo> satisfiedProperties, bool satisfied, string reason = null) 
            : base(satisfied, reason)
        {
            AllProperties = allProperties;
            SatisfiedProperties = satisfiedProperties;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Obtém todas as propriedades do alvo.
        /// </summary>
        public IEnumerable<PropertyInfo> AllProperties { get; private set; }

        /// <summary>
        /// Obtém as propriedades do alvo que satisfizeram a especificação.
        /// </summary>
        public IEnumerable<PropertyInfo> SatisfiedProperties { get; private set; }
        #endregion
    }
}
