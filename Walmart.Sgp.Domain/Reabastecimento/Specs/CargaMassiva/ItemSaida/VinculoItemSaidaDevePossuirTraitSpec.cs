using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada
{
    /// <summary>
    /// Especificação que valida se o item de saida possui Trait.
    /// </summary>
    public class VinculoItemSaidaDevePossuirTraitSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, bool> m_itemSaidaPossuiTrait;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoItemSaidaDevePossuirTraitSpec"/>.
        /// </summary>
        /// <param name="itemSaidaPossuiTrait">O delegate que verifica se um item de saida possui Trait.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoItemSaidaDevePossuirTraitSpec(Func<long, long, long, bool> itemSaidaPossuiTrait, int cdSistema)
        {
            this.m_itemSaidaPossuiTrait = itemSaidaPossuiTrait;
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
            get { return Texts.OutputItemNoTrait; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_itemSaidaPossuiTrait(group.First().CdItemDetalheSaida, m_cdSistema, group.First().CdLoja);
        }
    }
}
