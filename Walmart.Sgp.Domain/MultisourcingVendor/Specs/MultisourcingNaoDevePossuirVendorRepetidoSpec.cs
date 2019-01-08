using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação que valida se o idFornecedorParametro foi repetido.
    /// </summary>
    public class MultisourcingNaoDevePossuirVendorRepetidoSpec : MultisourcingSpecBase<object>
    {
        private byte m_cdSistema;

        private Func<long, byte, ItemDetalhe> m_obterPorCodigoESistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingNaoDevePossuirVendorRepetidoSpec"/>.
        /// </summary>
        /// <param name="obterPorCodigoESistema">O delegate que obtém o item por código e sistema.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public MultisourcingNaoDevePossuirVendorRepetidoSpec(Func<long, byte, ItemDetalhe> obterPorCodigoESistema, byte cdSistema)
        {
            m_obterPorCodigoESistema = obterPorCodigoESistema;
            m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, object> Key
        {
            get
            {
                return (m) => new { m.CdItemDetalheSaida, m.Vendor9Digitos };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.MultisourcingInvalidPercentualsSameVendor; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, Multisourcing> group)
        {
            var itens = BuscarItensComProjecao(group);

            var itensAgrupadosPorFornecedor = AgruparItensPorFornecedor(itens);

            if (itensAgrupadosPorFornecedor.Any(grupo => grupo.Count() > 1))
            {
                return false;
            }

            return true;
        }

        private static IEnumerable<IGrouping<int, ItemDetalhe>> AgruparItensPorFornecedor(List<ItemDetalhe> itens)
        {
            return itens.GroupBy(g => g.IdFornecedorParametro);
        }

        private List<ItemDetalhe> BuscarItensComProjecao(IGrouping<object, Multisourcing> group)
        {
            var itens = new List<ItemDetalhe>();
            foreach (var itemValidar in group)
            {
                var item = m_obterPorCodigoESistema(itemValidar.CdItemDetalheEntrada, m_cdSistema);
                itens.Add(item);
            }

            return itens;
        }
    }
}
