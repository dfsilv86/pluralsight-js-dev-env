using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Item de Entrada não pertence ao fornecedor (Item de entrada não pertence ao fornecedor informado).
    /// </summary>
    public class ItemEntradaPertenceFornecedorSpec : MultisourcingSpecBase<object>
    {
        /// <summary>
        /// O delegate que verifica se o multisourcing pertence ao fornecedor.
        /// </summary>
        private Func<Multisourcing, bool> m_pertenceDelegate;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemEntradaPertenceFornecedorSpec"/>.
        /// </summary>
        /// <param name="pertenceDelegate">O delegate que verifica se um multisourcing pertence ao fornecedor.</param>
        public ItemEntradaPertenceFornecedorSpec(Func<Multisourcing, bool> pertenceDelegate)
        {
            m_pertenceDelegate = pertenceDelegate;
        }

        /// <summary>
        /// A chave de agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get { return m => new { m.CdItemDetalheEntrada, m.Vendor9Digitos }; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InputItemDoesNotBelongToTheVendor; }
        }

        /// <summary>
        /// Verifica se o grupo está ok.
        /// </summary>
        /// <param name="group">O grupo de multisourcings.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            return m_pertenceDelegate(group.First());
        }
    }
}
