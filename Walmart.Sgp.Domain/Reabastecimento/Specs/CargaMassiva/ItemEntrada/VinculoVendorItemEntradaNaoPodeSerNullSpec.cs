using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o vendor do item de entrada esta vinculado.
    /// </summary>
    public class VinculoVendorItemEntradaNaoPodeSerNullSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, bool> m_vendorItemEntradaPossuiVinculo;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoVendorItemEntradaNaoPodeSerNullSpec"/>.
        /// </summary>
        /// <param name="vendorItemEntradaPossuiVinculo">O delegate que verifica se o vendor de um item de entrada esta vinculado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoVendorItemEntradaNaoPodeSerNullSpec(Func<long, long, bool> vendorItemEntradaPossuiVinculo, int cdSistema)
        {
            this.m_vendorItemEntradaPossuiVinculo = vendorItemEntradaPossuiVinculo;
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
            get { return Texts.InputItemVendorIsNull; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_vendorItemEntradaPossuiVinculo(group.First().CdItemDetalheEntrada, m_cdSistema);
        }
    }
}
