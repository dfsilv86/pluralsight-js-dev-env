using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemSaida
{
    /// <summary>
    /// Especificação que valida se a loja e o CD possuem cadastro para o item controle de estoque.
    /// </summary>
    public class VinculoLojaCDPossuemCadastroItemControleEstoqueSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, long, bool> m_lojaCDPossuemCadastroItemControleEstoque;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoLojaCDPossuemCadastroItemControleEstoqueSpec"/>.
        /// </summary>
        /// <param name="lojaCDPossuemCadastroItemControleEstoque">O delegate que verifica se a loja e o CD possuem cadastro para o item controle de estoque.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoLojaCDPossuemCadastroItemControleEstoqueSpec(Func<long, long, long, long, bool> lojaCDPossuemCadastroItemControleEstoque, int cdSistema)
        {
            this.m_lojaCDPossuemCadastroItemControleEstoque = lojaCDPossuemCadastroItemControleEstoque;
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
            get { return Texts.StoreAndDCNotRegisteredForTheItem; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_lojaCDPossuemCadastroItemControleEstoque(group.First().CdItemDetalheSaida, group.First().CdCD, group.First().CdLoja, m_cdSistema);
        }
    }
}
