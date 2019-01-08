using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o item de entrada possui cadastro compra casada como filho.
    /// </summary>
    public class VinculoItemEntradaNaoPodeSerFilhoCompraCasadaSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, int> m_obterCodItemPaiCompraCasadaPorFilho;

        private readonly int m_cdSistema;
        private int m_codItemPai;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoItemEntradaNaoPodeSerFilhoCompraCasadaSpec"/>.
        /// </summary>
        /// <param name="obterCodItemPaiCompraCasadaPorFilho">O delegate que obtem o cod. do item pai de uma compra casada pelo cod. de um item filho.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoItemEntradaNaoPodeSerFilhoCompraCasadaSpec(Func<long, long, int> obterCodItemPaiCompraCasadaPorFilho, int cdSistema)
        {
            this.m_obterCodItemPaiCompraCasadaPorFilho = obterCodItemPaiCompraCasadaPorFilho;
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
            get { return Texts.MarriedBuyItemChildUseParent.With(m_codItemPai); }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            m_codItemPai = m_obterCodItemPaiCompraCasadaPorFilho(group.First().CdItemDetalheEntrada, m_cdSistema);

            return m_codItemPai == 0;
        }
    }
}
