using System;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o item de saída é um item de transferencia.
    /// </summary>
    public class ItemSaidaNaoPodeSerTransferenciaSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, byte, ItemDetalhe> m_obterItem;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemSaidaNaoPodeSerTransferenciaSpec"/>.
        /// </summary>
        /// <param name="obterItem">O delegate que obtem o item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public ItemSaidaNaoPodeSerTransferenciaSpec(Func<long, byte, ItemDetalhe> obterItem, int cdSistema)
        {
            this.m_obterItem = obterItem;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => m.CdItemDetalheSaida;
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.OutputItemIsMTR; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var itemDetalhe = this.m_obterItem(group.First().CdItemDetalheSaida, (byte)m_cdSistema);
            if (itemDetalhe != null && itemDetalhe.BlItemTransferencia)
            {
                return false;
            }

            return true;
        }
    }
}
