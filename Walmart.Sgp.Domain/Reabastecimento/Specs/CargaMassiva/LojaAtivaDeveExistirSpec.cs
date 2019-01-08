using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se os itens possuem loja valida.
    /// </summary>
    public class LojaAtivaDeveExistirSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, bool> m_itensPossuemLojaExistente;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaAtivaDeveExistirSpec"/>.
        /// </summary>
        /// <param name="itensPossuemLojaExistente">O delegate que verifica se os itens possuem loja valida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public LojaAtivaDeveExistirSpec(Func<long, long, bool> itensPossuemLojaExistente, int cdSistema)
        {
            this.m_itensPossuemLojaExistente = itensPossuemLojaExistente;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdLoja };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.StoreNotFoundOrDisabled; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            return m_itensPossuemLojaExistente(group.First().CdLoja, m_cdSistema);
        }
    }
}
