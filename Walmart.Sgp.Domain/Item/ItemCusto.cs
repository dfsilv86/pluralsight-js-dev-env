using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa o custo de um item.
    /// </summary>
    public class ItemCusto
    {
        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o id do estoque.
        /// </summary>
        public int IDEstoque { get; set; }

        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o item.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define a data de recebimento.
        /// </summary>
        public DateTime DtRecebimento { get; set; }

        /// <summary>
        /// Obtém ou define o custo de compra.
        /// </summary>
        public decimal CustoCompra { get; set; }

        /// <summary>
        /// Obtém ou define o custo gerencial.
        /// </summary>
        public decimal CustoGerencial { get; set; }

        /// <summary>
        /// Obtém ou define o custo de cadastro.
        /// </summary>
        public decimal CustoCadastro { get; set; }

        /// <summary>
        /// Obtém ou define o custo contábil.
        /// </summary>
        public decimal CustoContabil { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se tem custo de cadastro.
        /// </summary>
        public bool BlCustoCadastro { get; set; }

        /// <summary>
        /// Obtém ou define o custo médio.
        /// </summary>
        public decimal VlCustoMedio { get; set; }

        /// <summary>
        /// Obtém ou define a data de recebimento da nota fiscal.
        /// </summary>
        public DateTime DtRecebimentoNota { get; set; }
    }
}