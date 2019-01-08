using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a se lista de itens do tipo ItemDetalheCD tem percentual 100%.
    /// </summary>
    public class ListaDeItensNaoPodeRepetirVendorSpec : SpecBase<IEnumerable<ItemDetalheCD>>
    {
        private int m_cdSistema;

        private Func<long, byte, ItemDetalhe> m_obterPorCodigoESistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ListaDeItensNaoPodeRepetirVendorSpec"/>.
        /// </summary>
        /// <param name="obterPorCodigoESistema">O delegate que obtém o item por código e sistema.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public ListaDeItensNaoPodeRepetirVendorSpec(Func<long, byte, ItemDetalhe> obterPorCodigoESistema, int cdSistema)
        {
            m_obterPorCodigoESistema = obterPorCodigoESistema;
            m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Verifica se a lista de itens informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A lista de ItemDetalheCD.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pela lista.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ItemDetalheCD> target)
        {
            var itensComValor = FiltrarItensComValor(target);
            var agrupados = BuscarItensAgrupados(itensComValor);

            if (agrupados.Any(grupo => grupo.Count() > 1))
            {
                return NotSatisfied(Texts.MultisourcingInvalidPercentualsSameVendor);
            }

            return Satisfied();
        }

        private static IEnumerable<ItemDetalheCD> FiltrarItensComValor(IEnumerable<ItemDetalheCD> target)
        {
            return target.Where(idcd => (idcd.vlPercentual.HasValue && idcd.vlPercentual.Value > 0));
        }

        private IEnumerable<IGrouping<int, ItemDetalhe>> BuscarItensAgrupados(IEnumerable<ItemDetalheCD> itensComValor)
        {
            var itens = new List<ItemDetalhe>();
            foreach (var itemCD in itensComValor)
            {
                itens.Add(m_obterPorCodigoESistema(itemCD.cdItem, (byte)m_cdSistema));
            }

            return itens.GroupBy(g => g.IdFornecedorParametro);
        }
    }
}
