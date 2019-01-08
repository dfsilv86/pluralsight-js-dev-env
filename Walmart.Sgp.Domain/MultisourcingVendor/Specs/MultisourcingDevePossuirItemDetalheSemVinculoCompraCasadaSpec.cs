using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação de validação se o item detalhe de entrada está desvinculado à compra casada.
    /// </summary>
    public class MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpec : MultisourcingSpecBase<long>
    {
        private Func<long, bool> m_estaVinculadoCompraCasada;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpec"/>.
        /// </summary>
        /// <param name="estaVinculadoCompraCasada">O delegate que verifica se o item detalhe de entrada está vinculado à compra casada.</param>
        public MultisourcingDevePossuirItemDetalheSemVinculoCompraCasadaSpec(Func<long, bool> estaVinculadoCompraCasada)
        {
            m_estaVinculadoCompraCasada = estaVinculadoCompraCasada;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, long> Key
        {
            get { return (m) => m.CdItemDetalheEntrada; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InputItemLinkedCombinedPurchase; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<long, Multisourcing> group)
        {
            return !m_estaVinculadoCompraCasada(group.Key);
        }
    }
}