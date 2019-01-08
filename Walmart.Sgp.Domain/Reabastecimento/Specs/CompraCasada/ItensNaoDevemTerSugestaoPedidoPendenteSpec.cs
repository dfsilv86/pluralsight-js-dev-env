using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada
{
    /// <summary>
    /// Especificação referente a se os itens filhos de uma compra casada são validos.
    /// </summary>
    public class ItensNaoDevemTerSugestaoPedidoPendenteSpec : SpecBase<IEnumerable<ItemDetalhe>>
    {
        private readonly Func<int, int, int, DateTime, bool> m_buscarSugestaoPedido;
        private readonly Func<int, int, int, DateTime, bool> m_buscarSugestaoPedidoCD;
        private readonly int m_idFornecedorParametro;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItensNaoDevemTerSugestaoPedidoPendenteSpec"/>
        /// </summary>
        /// <param name="idFornecedorParametro">O id fornecedor paramatro.</param>
        /// <param name="buscarSugestaoPedido">Função para buscar sugestao pedido para um item.</param>
        /// <param name="buscarSugestaoPedidoCD">Função para buscar sugestao pedido para um item em Sugestao CD.</param>
        public ItensNaoDevemTerSugestaoPedidoPendenteSpec(int idFornecedorParametro, Func<int, int, int, DateTime, bool> buscarSugestaoPedido, Func<int, int, int, DateTime, bool> buscarSugestaoPedidoCD)
        {
            m_buscarSugestaoPedido = buscarSugestaoPedido;
            m_buscarSugestaoPedidoCD = buscarSugestaoPedidoCD;
            m_idFornecedorParametro = idFornecedorParametro;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<ItemDetalhe> target)
        {
            var idItemDetalheSaida = target.FirstOrDefault().ItemSaida.IDItemDetalhe;
            var cdSistema = target.FirstOrDefault().CdSistema;
            var dt = DateTime.Now;

            if (m_buscarSugestaoPedido(idItemDetalheSaida, m_idFornecedorParametro, cdSistema, dt) || m_buscarSugestaoPedidoCD(idItemDetalheSaida, m_idFornecedorParametro, cdSistema, dt))
            {
                return NotSatisfied(Texts.ErrorOnActionOrderPending);
            }

            return Satisfied();
        }
    }
}
