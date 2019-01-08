using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de domínio relacionado a movimentação.
    /// </summary>
    public class MovimentacaoService : DomainServiceBase<IMovimentacaoGateway>, IMovimentacaoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MovimentacaoService"/>.
        /// </summary>
        /// <param name="movimentacaoGateway">O table data gateway para movimentação.</param>
        public MovimentacaoService(IMovimentacaoGateway movimentacaoGateway)
            : base(movimentacaoGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Seleciona o extrato de movimentação de produto.
        /// </summary>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dataInicio">A data de inicio.</param>
        /// <param name="dataFim">A data de fim.</param>
        /// <param name="idItemDetalhe">O id de item.</param>
        /// <param name="tipoMovimento">O tipo de movimento.</param>
        /// <param name="idInventario">O id de inventario.</param>
        /// <remarks>
        ///  O parâmetro 'tipoMovimento' serve para trazer de forma separada os tipos de movimentações que são ajuste de inventário (I)
        ///  e o que são movimentações normais (N) (Venda, Quebra, etc.).
        ///  Se for necessário trazer todos os tipos de movimentações, passe esse parâmetro com o valor branco.
        /// </remarks>
        /// <returns>Os items do extato.</returns>
        public IEnumerable<ItemExtrato> RelExtratoProdutoMovimentacao(int idLoja, DateTime dataInicio, DateTime dataFim, long idItemDetalhe, string tipoMovimento, int? idInventario)
        {
            Assert(new { Store = idLoja, StartDate = dataInicio, EndDate = dataFim, ItemCode = idItemDetalhe }, new AllMustBeInformedSpec());

            return this.MainGateway.SelExtratoProdutoMovimentacao(idLoja, dataInicio, dataFim, idItemDetalhe, tipoMovimento, idInventario);
        }

        /// <summary>
        /// Obtém a movimentação pelo id.
        /// </summary>
        /// <param name="idMovimentacao">O id da movimentação.</param>
        /// <returns>A movimentação.</returns>
        public Movimentacao ObterEstruturadoPorId(int idMovimentacao)
        {
            return MainGateway.ObterEstruturadoPorId(idMovimentacao);
        }

        /// <summary>
        /// Obtém as datas de quebra para o item na loja informada.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item desejado.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>As data de quebra.</returns>
        public IEnumerable<DateTime> ObterDatasDeQuebra(int idItemDetalhe, int idLoja)
        {
            return MainGateway.ObterDatasDeQuebra(idItemDetalhe, idLoja);
        }
        #endregion
    }
}
