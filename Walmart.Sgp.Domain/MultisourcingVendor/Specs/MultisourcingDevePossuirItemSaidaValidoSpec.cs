using System;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Item de Saída Inválido (Item de Saída deletado/inativo).
    /// </summary>
    public class MultisourcingDevePossuirItemSaidaValidoSpec : MultisourcingSpecBase<long>
    {
        /// <summary>
        /// O delegate que obtém o item detalhe por código e sistema.
        /// </summary>
        private Func<Multisourcing, ItemDetalhe> m_obterItemDetalhePorCodigoESistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingDevePossuirItemSaidaValidoSpec"/>.
        /// </summary>
        /// <param name="obterItemDetalhePorCodigoESistema">O delegate que obtém o item detalhe por código e sistema.</param>
        public MultisourcingDevePossuirItemSaidaValidoSpec(Func<Multisourcing, ItemDetalhe> obterItemDetalhePorCodigoESistema)
        {
            m_obterItemDetalhePorCodigoESistema = obterItemDetalhePorCodigoESistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, long> Key
        {
            get { return m => m.CdItemDetalheSaida; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InvalidOutputItem; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<long, Multisourcing> group)
        {
            var multisourcing = group.First();
            var itemDetalhe = m_obterItemDetalhePorCodigoESistema(multisourcing);

            foreach (var item in group)
            {
                item.ItemDetalheSaida = itemDetalhe;
            }

            return multisourcing.ItemDetalheSaida != null && multisourcing.ItemDetalheSaida.TpStatus == TipoStatusItem.Ativo;
        }
    }
}
