using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada
{
    /// <summary>
    /// Especificação que valida se o item de entrada possui Trait.
    /// </summary>
    public class VinculoItemEntradaDevePossuirTraitSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, bool> m_itemEntradaPossuiTrait;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoItemEntradaDevePossuirTraitSpec"/>.
        /// </summary>
        /// <param name="itemEntradaPossuiTrait">O delegate que verifica se um item de entrada possui Trait.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoItemEntradaDevePossuirTraitSpec(Func<long, long, long, bool> itemEntradaPossuiTrait, int cdSistema)
        {
            this.m_itemEntradaPossuiTrait = itemEntradaPossuiTrait;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdItemDetalheEntrada, m.CdLoja };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InputItemNoTrait; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_itemEntradaPossuiTrait(group.First().CdItemDetalheEntrada, m_cdSistema, group.First().CdLoja);
        }
    }
}
