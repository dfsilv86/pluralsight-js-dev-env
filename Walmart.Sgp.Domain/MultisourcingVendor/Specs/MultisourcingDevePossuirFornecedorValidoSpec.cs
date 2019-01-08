using System;
using System.Linq;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Fornecedor Inválido (Vendor inativo/delatado).
    /// </summary>
    public class MultisourcingDevePossuirFornecedorValidoSpec : MultisourcingSpecBase<long>
    {
        /// <summary>
        /// O delegate que obtém o fornecedor por código e sistema.
        /// </summary>
        private Func<Multisourcing, Fornecedor> m_obterPorCodigoESistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingDevePossuirFornecedorValidoSpec"/>.
        /// </summary>
        /// <param name="obterPorCodigoESistema">O delegate que obtém o fornecedor por código e sistema.</param>
        public MultisourcingDevePossuirFornecedorValidoSpec(Func<Multisourcing, Fornecedor> obterPorCodigoESistema)
        {
            m_obterPorCodigoESistema = obterPorCodigoESistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, long> Key
        {
            get { return m => m.Vendor9Digitos; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InvalidVendor; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<long, Multisourcing> group)
        {
            var multisourcing = group.First();
            var fornecedor = m_obterPorCodigoESistema(multisourcing);

            foreach (var item in group)
            {
                item.Fornecedor = fornecedor;
            }

            return fornecedor != null;
        }
    }
}
