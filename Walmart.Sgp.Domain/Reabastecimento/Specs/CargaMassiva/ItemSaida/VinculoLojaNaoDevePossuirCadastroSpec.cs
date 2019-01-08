using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemSaida
{
    /// <summary>
    /// Especificação que valida se o item de saida ja possui cadastro.
    /// </summary>
    public class VinculoLojaNaoDevePossuirCadastroSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, long, bool> m_itemSaidaPossuiCadastro;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoLojaNaoDevePossuirCadastroSpec"/>.
        /// </summary>
        /// <param name="itemSaidaPossuiCadastro">O delegate que verifica se um item de saida ja possui cadastro.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoLojaNaoDevePossuirCadastroSpec(Func<long, long, long, long, bool> itemSaidaPossuiCadastro, int cdSistema)
        {
            this.m_itemSaidaPossuiCadastro = itemSaidaPossuiCadastro;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdItemDetalheSaida, m.CdLoja, m.CdCD };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.StoreAlreadyHaveSetupForItem; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return !m_itemSaidaPossuiCadastro(group.First().CdItemDetalheSaida, group.First().CdCD, group.First().CdLoja, m_cdSistema);
        }
    }
}
