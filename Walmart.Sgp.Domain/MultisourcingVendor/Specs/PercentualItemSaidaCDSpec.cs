using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Percentual não atingido (somatória do percentual para o mesmo item de SAIDA por CD não atinge 100).
    /// </summary>
    public class PercentualItemSaidaCDSpec : MultisourcingSpecBase<object>
    {
        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get 
            {
                return (m) => new { m.CdItemDetalheSaida, m.CD };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.PercentageOutputItemCDNotReached; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            return group.Sum(m => m.vlPercentual) == 100;
        }
    }
}