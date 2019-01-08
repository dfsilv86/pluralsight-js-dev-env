using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o vendor do item de entrada está inativo ou deletado.
    /// </summary>
    public class VinculoVendorItemSaidaNaoPodeSerInativoOuDeletadoSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, bool> m_vendorItemSaidaInativoOuDeletado;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoVendorItemSaidaNaoPodeSerInativoOuDeletadoSpec"/>.
        /// </summary>
        /// <param name="vendorItemSaidaInativoOuDeletado">O delegate que verifica se o vendor de um item de saida é inativo ou deletado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoVendorItemSaidaNaoPodeSerInativoOuDeletadoSpec(Func<long, long, bool> vendorItemSaidaInativoOuDeletado, int cdSistema)
        {
            this.m_vendorItemSaidaInativoOuDeletado = vendorItemSaidaInativoOuDeletado;
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
            get { return Texts.VendorOutputItemDisabledOrDeleted; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            if (m_vendorItemSaidaInativoOuDeletado(group.First().CdItemDetalheSaida, m_cdSistema))
            {
                return false;
            }

            return true;
        }
    }
}
