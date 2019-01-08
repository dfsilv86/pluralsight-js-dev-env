using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o item de entrada está inativo ou deletado.
    /// </summary>
    public class ItemEntradaDevePossuirTipoReabastecimentoSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, byte, ItemDetalhe> m_obterItemEntrada;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemEntradaDevePossuirTipoReabastecimentoSpec"/>.
        /// </summary>
        /// <param name="obterItemEntrada">O delegate que obtem o item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public ItemEntradaDevePossuirTipoReabastecimentoSpec(Func<long, byte, ItemDetalhe> obterItemEntrada, int cdSistema)
        {
            this.m_obterItemEntrada = obterItemEntrada;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => m.CdItemDetalheEntrada;
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InputItemMissingTipoReabastecimento; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var itemDetalhe = this.m_obterItemEntrada(group.First().CdItemDetalheEntrada, (byte)m_cdSistema);

            if (itemDetalhe != null && (null == itemDetalhe.VlTipoReabastecimento || itemDetalhe.VlTipoReabastecimento == ValorTipoReabastecimento.Nenhum))
            {
                return false;
            }

            return true;
        }
    }
}
