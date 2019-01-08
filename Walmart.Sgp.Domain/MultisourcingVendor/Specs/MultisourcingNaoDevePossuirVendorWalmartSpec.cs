using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação que valida se o vendor é walmart.
    /// </summary>
    public class MultisourcingNaoDevePossuirVendorWalmartSpec : MultisourcingSpecBase<object>
    {
        private Func<long, bool> m_verificaVendorWalmart;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingNaoDevePossuirVendorWalmartSpec"/>.
        /// </summary>
        /// <param name="verificaVendorWalmart">O delegate que verifica se um vendor é do tipo Walmart.</param>
        public MultisourcingNaoDevePossuirVendorWalmartSpec(Func<long, bool> verificaVendorWalmart)
        {
            m_verificaVendorWalmart = verificaVendorWalmart;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get
            {
                return (m) => m.Vendor9Digitos;
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.MultisourcingInvalidInputItemVendor; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            var vendors = group.Where(g => g.Fornecedor != null).Select(s => s.Fornecedor.cdFornecedor).ToArray();

            foreach (var vendor in vendors)
            {
                if (m_verificaVendorWalmart(vendor))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
