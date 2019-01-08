using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a se todas as propriedades informadas no objeto anônimo tiveram seu valor informado.
    /// </summary>
    public class AllMustBeInformedSpec : SpecBase<object>
    {
        #region Fields
        private bool m_allowZeroes = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AllMustBeInformedSpec"/>.
        /// </summary>
        public AllMustBeInformedSpec()
            : this(false)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AllMustBeInformedSpec"/>.
        /// </summary>
        /// <param name="allowZeroes">Indica se o valor zero é permitido.</param>
        public AllMustBeInformedSpec(bool allowZeroes)
        {
            m_allowZeroes = allowZeroes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O object.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(object target)
        {
            var atLeastOneSpec = new AtLeastOneMustBeInformedSpec(m_allowZeroes);
            var result = atLeastOneSpec.IsSatisfiedBy(target) as PropertiesSpecResult;
            var allProperties = result.AllProperties.ToList();
            var satisfiedProperties = result.SatisfiedProperties;

            if (allProperties.Count == 0 || allProperties.Count != satisfiedProperties.Count())
            {
                var notInformedProperties = allProperties.Except(satisfiedProperties)
                    .Select(p => GlobalizationHelper.GetText(p.Name))
                    .Distinct();

                var notInformtedPropertiesText = notInformedProperties.JoinWords();

                var msg = notInformedProperties.Count() > 1 ? Texts.AllMustBeInformed : Texts.AllMustBeInformedSingular;

                return NotSatisfied(msg.With(notInformtedPropertiesText));
            }

            return Satisfied();
        }
        #endregion
    }
}
