using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se os itens possuem relacionamento.
    /// </summary>
    public class VinculoDevePossuirRelacionamentoSGPSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, bool> m_itensPossuemRelacionamento;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoDevePossuirRelacionamentoSGPSpec"/>.
        /// </summary>
        /// <param name="itensPossuemRelacionamento">O delegate que verifica se os itens possuem relacionamento.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoDevePossuirRelacionamentoSGPSpec(Func<long, long, long, bool> itensPossuemRelacionamento, int cdSistema)
        {
            this.m_itensPossuemRelacionamento = itensPossuemRelacionamento;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdItemDetalheEntrada, m.CdItemDetalheSaida };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.SgpRelationshipNotFound; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_itensPossuemRelacionamento(group.First().CdItemDetalheEntrada, group.First().CdItemDetalheSaida, m_cdSistema);
        }
    }
}
