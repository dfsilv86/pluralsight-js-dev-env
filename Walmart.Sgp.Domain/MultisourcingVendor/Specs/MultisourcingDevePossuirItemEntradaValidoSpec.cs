using System;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Item de Entrada Inválido (Item de Entrada deletado/inativo).
    /// </summary>
    public class MultisourcingDevePossuirItemEntradaValidoSpec : MultisourcingSpecBase<long>
    {
        /// <summary>
        /// O delegate que obtém o item detalhe por código e sistema.
        /// </summary>
        private Func<Multisourcing, ItemDetalhe> m_obterItemDetalhePorCodigoESistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingDevePossuirItemEntradaValidoSpec"/>.
        /// </summary>
        /// <param name="obterItemDetalhePorCodigoESistema">O delegate que obtém o item detalhe por código e sistema.</param>
        public MultisourcingDevePossuirItemEntradaValidoSpec(Func<Multisourcing, ItemDetalhe> obterItemDetalhePorCodigoESistema)
        {
            m_obterItemDetalhePorCodigoESistema = obterItemDetalhePorCodigoESistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, long> Key
        {
            get { return m => m.CdItemDetalheEntrada; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InvalidInputItem; }
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
                item.ItemDetalheEntrada = itemDetalhe;
            }

            return multisourcing.ItemDetalheEntrada != null && multisourcing.ItemDetalheEntrada.TpStatus == TipoStatusItem.Ativo;
        }
    }
}
