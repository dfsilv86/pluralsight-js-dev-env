using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Item de Entrada não relacionado (Item de entrada não é relacionado ao item de saída informado).
    /// </summary>
    public class ItemEntradaPertenceItemSaidaSpec : MultisourcingSpecBase<object>
    {
        /// <summary>
        /// O delegate que verifica se o item de entrada pertence ao item de saída.
        /// </summary>
        private Func<Multisourcing, long> m_itemEntradaPertenceItemSaida;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemEntradaPertenceItemSaidaSpec"/>.
        /// </summary>
        /// <param name="itemEntradaPertenceItemSaida">O delegate que verifica se o item de entrada pertence ao item de saída.</param>
        public ItemEntradaPertenceItemSaidaSpec(Func<Multisourcing, long> itemEntradaPertenceItemSaida)
        {
            m_itemEntradaPertenceItemSaida = itemEntradaPertenceItemSaida;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get { return m => new { m.CdItemDetalheEntrada, m.CdItemDetalheSaida }; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InputItemNotRelated; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            var multisourcing = group.First();
            var idRelacionamentoItemSecundario = m_itemEntradaPertenceItemSaida(multisourcing);

            foreach (var item in group)
            {
                item.IDRelacionamentoItemSecundario = idRelacionamentoItemSecundario;
            }

            return multisourcing.IDRelacionamentoItemSecundario != default(long);
        }
    }
}
