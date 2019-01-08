using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada
{
    /// <summary>
    /// Especificação referente a se os itens filhos de uma compra casada são validos.
    /// </summary>
    public class TraitItensFilhoDevemSerIguaisAoPaiSpec : SpecBase<IEnumerable<ItemDetalhe>>
    {
        private readonly Func<int, int, Paging, IEnumerable<Loja>> m_obterTraitsPorItem;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TraitItensFilhoDevemSerIguaisAoPaiSpec"/>
        /// </summary>
        /// <param name="obterTraitsPorItem">Metodo para buscar traits.</param>
        public TraitItensFilhoDevemSerIguaisAoPaiSpec(Func<int, int, Paging, IEnumerable<Loja>> obterTraitsPorItem)
        {
            m_obterTraitsPorItem = obterTraitsPorItem;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ItemDetalhe> target)
        {
            var itensSelecionados = target.Where(i => (i.PaiCompraCasada.HasValue && i.PaiCompraCasada.Value) || (i.FilhoCompraCasada.HasValue && i.FilhoCompraCasada.Value));
            var traits = PopulaTraits(itensSelecionados);

            var amostra = traits.FirstOrDefault();
            if (traits.Any(t => !t.SequenceEqual(amostra)))
            {
                return NotSatisfied(Texts.TraitMustBeEquals);
            }

            return Satisfied();
        }

        private List<int[]> PopulaTraits(IEnumerable<ItemDetalhe> itensSelecionados)
        {
            var traits = new List<int[]>();

            foreach (var item in itensSelecionados)
            {
                var idsLojas = m_obterTraitsPorItem(item.IDItemDetalhe, item.CdSistema, null).Select(s => s.cdLoja).OrderBy(o => o).ToArray();
                traits.Add(idsLojas);
            }

            return traits;
        }
    }
}
