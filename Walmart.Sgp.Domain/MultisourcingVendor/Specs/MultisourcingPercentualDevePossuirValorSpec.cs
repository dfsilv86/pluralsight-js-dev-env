using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação que valida se o percentual foi informado.
    /// </summary>
    public class MultisourcingPercentualDevePossuirValorSpec : MultisourcingSpecBase<object>
    {
        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get 
            {
                return GroupAll;
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.CannotImportEmptyPercent; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            return group.First().vlPercentual > 0;
        }
    }
}