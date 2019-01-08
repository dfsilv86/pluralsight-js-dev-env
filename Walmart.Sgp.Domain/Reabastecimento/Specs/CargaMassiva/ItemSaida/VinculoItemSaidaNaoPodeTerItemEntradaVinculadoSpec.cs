using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemSaida
{
    /// <summary>
    /// Especificação que valida se o item de saida e loja tem vinculo.
    /// </summary>
    //// TODO: este nome de spec não está errado?? originalmente returna true se itementrada = null ?!
    public class VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, IEnumerable<RelacaoItemLojaCD>> m_obterRelacaoPorVinculo;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpec"/>.
        /// </summary>
        /// <param name="obterRelacaoPorVinculo">O delegate que verifica se um item de saida é inativo ou deletado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpec(Func<long, long, long, IEnumerable<RelacaoItemLojaCD>> obterRelacaoPorVinculo, int cdSistema)
        {
            this.m_obterRelacaoPorVinculo = obterRelacaoPorVinculo;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdItemDetalheSaida, m.CdLoja };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.NoLinkForStoreAndItem; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var relacao = m_obterRelacaoPorVinculo(group.First().CdLoja, group.First().CdItemDetalheSaida, m_cdSistema);

            return relacao.Any(item => item.IdItemEntrada != null);
        }
    }
}
