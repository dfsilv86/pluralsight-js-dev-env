using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma Extrato.
    /// </summary>
    public class ItemExtrato
    {
        /// <summary>
        /// Obtém ou define ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Obtém ou define IDMovimentacao.
        /// </summary>
        public long? IDMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define dtMovimentado.
        /// </summary>
        public DateTime? dtMovimentado { get; set; }

        /// <summary>
        /// Obtém ou define IDTipoMovimentacao.
        /// </summary>
        public int? IDTipoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define TipoMovimento.
        /// </summary>
        public TipoMovimento TipoMovimento { get; set; }

        /// <summary>
        /// Obtém ou define dsTipoMovimentacao.
        /// </summary>
        public string dsTipoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define cddsItem.
        /// </summary>
        public string cddsItem { get; set; }

        /// <summary>
        /// Obtém ou define Tipo.
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Obtém ou define qtdMovimentacao.
        /// </summary>
        public decimal? qtdMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoContabilAtual.
        /// </summary>
        public decimal? vlCustoContabilAtual { get; set; }

        /// <summary>
        /// Obtém ou define EstoqueTeorico.
        /// </summary>
        public decimal? EstoqueTeorico { get; set; }

        /// <summary>
        /// Obtém ou define IDInventario.
        /// </summary>
        public int? IDInventario { get; set; }

        /// <summary>
        /// Obtém ou define IDNotaFiscal.
        /// </summary>
        public long? IDNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define o custo total.
        /// </summary>
        public decimal? CustoTotal { get; set; }

        /// <summary>
        /// Obtém ou define o estoque teorico financeiro.
        /// </summary>
        public decimal? EstoqueTeoricoFinanceiro { get; set; }
    }
}
