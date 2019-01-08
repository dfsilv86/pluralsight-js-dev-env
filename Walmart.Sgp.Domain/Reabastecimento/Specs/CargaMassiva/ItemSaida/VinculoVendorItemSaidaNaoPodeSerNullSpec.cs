using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o vendor do item de saida esta vinculado.
    /// </summary>
    public class VinculoVendorItemSaidaNaoPodeSerNullSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, bool> m_vendorItemSaidaPossuiVinculo;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoVendorItemSaidaNaoPodeSerNullSpec"/>.
        /// </summary>
        /// <param name="vendorItemSaidaPossuiVinculo">O delegate que verifica se o vendor de um item de saida esta vinculado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoVendorItemSaidaNaoPodeSerNullSpec(Func<long, long, bool> vendorItemSaidaPossuiVinculo, int cdSistema)
        {
            this.m_vendorItemSaidaPossuiVinculo = vendorItemSaidaPossuiVinculo;
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
            get { return Texts.OutputItemVendorIsNull; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_vendorItemSaidaPossuiVinculo(group.First().CdItemDetalheSaida, m_cdSistema);
        }
    }
}
