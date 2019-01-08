using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se um item é Staple, CD convertido, item prime e faz parte de uma XRef.
    /// </summary>
    public class VinculoItemXrefDeveSerPrimeSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, long, RelacaoItemLojaCDXrefItemPrime> m_verificaItemStaple;

        private readonly int m_cdSistema;
        private long? m_cdItemPrime = null;
        private long? m_cdXref = null;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoItemXrefDeveSerPrimeSpec"/>.
        /// </summary>
        /// <param name="verificaItemStaple">O delegate que verifica se um item é Staple.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoItemXrefDeveSerPrimeSpec(Func<long, long, long, long, RelacaoItemLojaCDXrefItemPrime> verificaItemStaple, int cdSistema)
        {
            this.m_verificaItemStaple = verificaItemStaple;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdItemDetalheEntrada, m.CdCD, m.CdLoja };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get
            {
                if (m_cdItemPrime == null && m_cdXref == null)
                {
                    return string.Empty;
                }

                return m_cdItemPrime.HasValue ? Texts.XrefIsntItemPrime.With(m_cdItemPrime.Value) : Texts.XrefWithoutPrimaryItem.With(m_cdXref.Value);
            }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var cdItem = group.First().CdItemDetalheEntrada;
            var cdCD = group.First().CdCD;
            var cdLoja = group.First().CdLoja;

            var result = m_verificaItemStaple(cdItem, cdCD, cdLoja, m_cdSistema);
            if (result == null || result.CdItem == result.CdItemPrime)
            {
                return true;
            }

            m_cdItemPrime = result.CdItemPrime;
            m_cdXref = result.CdCrossRef;

            return false;
        }
    }
}
