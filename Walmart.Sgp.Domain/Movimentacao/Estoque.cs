using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma Estoque.
    /// </summary>
    public class Estoque
    {
        /// <summary>
        /// Obtém ou define IDEstoque.
        /// </summary>
        public long IDEstoque { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define dhApuracao.
        /// </summary>
        public DateTime? dhApuracao { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoCompraAtual.
        /// </summary>
        public decimal? vlCustoCompraAtual { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoContabilAtual.
        /// </summary>
        public decimal? vlCustoContabilAtual { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoGerencialAtual.
        /// </summary>
        public decimal? vlCustoGerencialAtual { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoCadastroAtual.
        /// </summary>
        public decimal? vlCustoCadastroAtual { get; set; }

        /// <summary>
        /// Obtém ou define blCustoCadastro.
        /// </summary>
        public bool? blCustoCadastro { get; set; }

        /// <summary>
        /// Obtém ou define dtCustoCadastro.
        /// </summary>
        public DateTime? dtCustoCadastro { get; set; }

        /// <summary>
        /// Obtém ou define dtLiberacao.
        /// </summary>
        public DateTime? dtLiberacao { get; set; }

        /// <summary>
        /// Obtém ou define qtEstoqueFisico.
        /// </summary>
        public decimal qtEstoqueFisico { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioCriacao.
        /// </summary>
        public int IDUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioAtualizacao.
        /// </summary>
        public int? IDUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? dhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define dtRecebimento.
        /// </summary>
        public DateTime dtRecebimento { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoMedio.
        /// </summary>
        public decimal? vlCustoMedio { get; set; }

        /// <summary>
        /// Obtém ou define vlPrecoBruto.
        /// </summary>
        public decimal? vlPrecoBruto { get; set; }

        /// <summary>
        /// Obtém ou define vlPrecoNet.
        /// </summary>
        public decimal? vlPrecoNet { get; set; }

        /// <summary>
        /// Obtém ou define vlMargem.
        /// </summary>
        public decimal? vlMargem { get; set; }

        /// <summary>
        /// Obtém ou define IDMovimentacao.
        /// </summary>
        public long? IDMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define a movimentação.
        /// </summary>
        public Movimentacao Movimentacao { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoVenda.
        /// </summary>
        public decimal? vlCustoVenda { get; set; }

        /// <summary>
        /// Obtém ou define vlPrecoPromocional.
        /// </summary>
        public decimal? vlPrecoPromocional { get; set; }

        /// <summary>
        /// Obtém ou define IdTipoProcesso.
        /// </summary>
        public int? IdTipoProcesso { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o ItemDetalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de ajuste.
        /// </summary>
        public MotivoMovimentacao TipoAjuste { get; set; }

        /// <summary>
        /// Obtém o tipo de movimentação de estoque.
        /// </summary>
        public TipoMovimentacaoEstoque TipoMovimentacao
        {
            get
            {
                if (TipoAjuste == null)
                {
                    return null;
                }

                switch (TipoAjuste.Id)
                {
                    case MotivoMovimentacao.IDEntradaAcerto:
                        return TipoMovimentacaoEstoque.Entrada;

                    case MotivoMovimentacao.IDSaidaAcerto:
                        return TipoMovimentacaoEstoque.Saida;

                    default:
                        // Se surgir outro tipo de ajuste além de entrada ou saída será necessário efetuar o devido tratamento.
                        throw new InvalidOperationException(Texts.InvalidAdjustmentType);
                }
            }
        }

        /// <summary>
        /// Obtém ou define o motivo do ajuste.
        /// </summary>
        public MotivoMovimentacao MotivoAjuste { get; set; }
    }
}
