using System;
using System.Linq;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se o vendor do item de entrada está inativo ou deletado.
    /// </summary>
    public class FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, byte, ItemDetalhe> m_itemDetalheObterPorCodigoESistema;
        private readonly Func<int, ItemDetalhe> m_itemDetalheObterEstruturadoPorId;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec"/>.
        /// </summary>
        /// <param name="itemDetalheObterPorCodigoESistema">O delegate que busca o ItemDetalhe pelo código.</param>
        /// <param name="itemDetalheObterEstruturadoPorId">O delegate que busca o ItemDetalhe estruturado pelo id.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec(Func<long, byte, ItemDetalhe> itemDetalheObterPorCodigoESistema, Func<int, ItemDetalhe> itemDetalheObterEstruturadoPorId, int cdSistema)
        {
            this.m_itemDetalheObterPorCodigoESistema = itemDetalheObterPorCodigoESistema;
            m_itemDetalheObterEstruturadoPorId = itemDetalheObterEstruturadoPorId;
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
            get { return Texts.SupplierInputItemDisabledOrDeleted; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var itemDetalhe = m_itemDetalheObterPorCodigoESistema(group.First().CdItemDetalheEntrada, (byte)m_cdSistema);

            if (itemDetalhe == null)
            {
                return true;
            }

            var estruturado = m_itemDetalheObterEstruturadoPorId(itemDetalhe.Id);

            if (estruturado.Fornecedor.blAtivo == false || estruturado.Fornecedor.stFornecedor == FornecedorStatus.Inativo || estruturado.Fornecedor.stFornecedor == FornecedorStatus.Deletado)
            {
                return false;
            }

            return true;
        }
    }
}
