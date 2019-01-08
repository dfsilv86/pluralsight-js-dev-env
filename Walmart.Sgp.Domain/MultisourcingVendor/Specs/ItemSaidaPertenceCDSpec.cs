using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Item de saída deve estar vinculado ao CD.
    /// </summary>
    public class ItemSaidaPertenceCDSpec : MultisourcingSpecBase<object>
    {
        /// <summary>
        /// O delegate que verifica se o item detalhe saída pertence ao CD.
        /// </summary>
        private Func<Multisourcing, bool> m_itemDetalheSaidaPertenceCDDelegate;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemSaidaPertenceCDSpec"/>.
        /// </summary>
        /// <param name="itemDetalheSaidaPertenceCDDelegate">O delegate que verifica se o item detalhe saida pertence ao CD.</param>
        public ItemSaidaPertenceCDSpec(Func<Multisourcing, bool> itemDetalheSaidaPertenceCDDelegate)
        {
            m_itemDetalheSaidaPertenceCDDelegate = itemDetalheSaidaPertenceCDDelegate;
        }

        /// <summary>
        /// A chave de agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get { return m => new { m.CdItemDetalheSaida, m.CD }; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.OutputItemDoesNotBelongToTheCD; }
        }

        /// <summary>
        /// Verifica se o grupo está ok.
        /// </summary>
        /// <param name="group">O grupo de multisourcings.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            return m_itemDetalheSaidaPertenceCDDelegate(group.First());
        }
    }
}
