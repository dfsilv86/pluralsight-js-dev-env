using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se as lojas são atendidas pelo CD.
    /// </summary>
    public class LojaDeveSerAtendidaPeloCDSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, bool> m_lojasSaoAtendidasPeloCD;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaDeveSerAtendidaPeloCDSpec"/>.
        /// </summary>
        /// <param name="lojasSaoAtendidasPeloCD">O delegate que verifica se as lojas são atendidas pelo cd.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public LojaDeveSerAtendidaPeloCDSpec(Func<long, long, long, bool> lojasSaoAtendidasPeloCD, int cdSistema)
        {
            this.m_lojasSaoAtendidasPeloCD = lojasSaoAtendidasPeloCD;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdCD, m.CdLoja };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.StoreUnattendedByCD; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_lojasSaoAtendidasPeloCD(group.First().CdLoja, group.First().CdCD, m_cdSistema);
        }
    }
}
