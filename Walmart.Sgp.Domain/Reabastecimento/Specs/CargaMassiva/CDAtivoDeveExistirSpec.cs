using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se os itens possuem CD valido.
    /// </summary>
    public class CDAtivoDeveExistirSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, bool> m_itensPossuemCDExistente;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CDAtivoDeveExistirSpec"/>.
        /// </summary>
        /// <param name="itensPossuemCDExistente">O delegate que verifica se os itens possuem CD valido.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public CDAtivoDeveExistirSpec(Func<long, long, bool> itensPossuemCDExistente, int cdSistema)
        {
            this.m_itensPossuemCDExistente = itensPossuemCDExistente;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdCD };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.CDNotFoundOrDisabled; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_itensPossuemCDExistente(group.First().CdCD, m_cdSistema);
        }
    }
}
