using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação de validação da existência de um CD.
    /// </summary>
    public class MultisourcingDevePossuirCDExistenteSpec : MultisourcingSpecBase<int>
    {
        private Func<int, int> m_obterCdPorCodigoDelegate;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingDevePossuirCDExistenteSpec"/>.
        /// </summary>
        /// <param name="obterCdPorCodigoDelegate">O delegate que obtém o CD pelo código.</param>
        public MultisourcingDevePossuirCDExistenteSpec(Func<int, int> obterCdPorCodigoDelegate)
        {
            m_obterCdPorCodigoDelegate = obterCdPorCodigoDelegate;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, int> Key
        {
            get { return (m) => m.CD; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.CDNotFound; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<int, Multisourcing> group)
        {
            var idCd = m_obterCdPorCodigoDelegate(group.Key);

            foreach (var item in group)
            {
                item.IDCD = idCd;
            }

            return 0 != idCd;
        }
    }
}