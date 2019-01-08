using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Item lido de um arquivo de importação de inventário.
    /// </summary>
    public class ArquivoInventarioItem
    {
        /// <summary>
        /// Obtém ou define o código do departamento.
        /// </summary>
        public int? CdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o código do item.
        /// </summary>
        public long CdItem { get; set; }

        /// <summary>
        /// Obtém ou define o código upc.
        /// </summary>
        public long? CdUpc { get; set; }

        /// <summary>
        /// Obtém ou define o tamanho.
        /// </summary>
        public string Tamanho { get; set; }

        /// <summary>
        /// Obtém ou define o número do estoque.
        /// </summary>
        public string NumeroEstoque { get; set; }

        /// <summary>
        /// Obtém ou define o custo unitário.
        /// </summary>
        public decimal? CustoUnitario { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do item.
        /// </summary>
        public string DescricaoItem { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do tamanho do item.
        /// </summary>
        public string DescricaoTamanho { get; set; }

        /// <summary>
        /// Obtém ou define o preço unitário do item.
        /// </summary>
        public decimal? PrecoUnitarioVarejo { get; set; }

        /// <summary>
        /// Obtém ou define o usuário que realizou a contagem.
        /// </summary>
        public string Completo { get; set; }

        /// <summary>
        /// Obtém ou define a data em que a última contagem foi realizada.
        /// </summary>
        public DateTime UltimaContagem { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade contada.
        /// </summary>
        public decimal QtItem { get; set; }

        /// <summary>
        /// Obtém ou define o total aumentado.
        /// </summary>
        public decimal? TotalAumentadoContg { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade on hand.
        /// </summary>
        public decimal? QuantidadeOnHand { get; set; }

        /// <summary>
        /// Obtém ou define o total on hand.
        /// </summary>
        public decimal? TotalAumentadoOnHand { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade diferença.
        /// </summary>
        public decimal? QuantidadeDif { get; set; }

        /// <summary>
        /// Obtém ou define o total diferença.
        /// </summary>
        public decimal? TotalAumentadoDif { get; set; }

        /// <summary>
        /// Obtém ou define o erro de leitura, caso ocorra.
        /// </summary>
        public string Erro { get; set; }
    }
}
