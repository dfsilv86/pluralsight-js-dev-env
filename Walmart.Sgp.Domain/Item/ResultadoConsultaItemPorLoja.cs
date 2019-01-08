using System;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa o resultado da procedure PR_SelecionarItemEstoque_Page.
    /// </summary>
    /// <remarks>No DapperItemDetalheGateway, a operação já foi migrada da proc para uma query paginada.</remarks>
    public class ResultadoConsultaItemPorLoja
    {
        /// <summary>
        /// Obtém ou define o IDItemDetalhe.
        /// </summary>        
        public long IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o cdItem.
        /// </summary>        
        public int cdItem { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>        
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o Loja.
        /// </summary>        
        public string Loja { get; set; }

        /// <summary>
        /// Obtém ou define o tpUnidadeMedida.
        /// </summary>        
        public string tpUnidadeMedida { get; set; }

        /// <summary>
        /// Obtém ou define o qtEstoqueFisico.
        /// </summary>        
        public decimal qtEstoqueFisico { get; set; }

        /// <summary>
        /// Obtém ou define o vlCustoCompra.
        /// </summary>        
        public decimal vlCustoCompra { get; set; }

        /// <summary>
        /// Obtém ou define o qtEstoqueFinanceiro.
        /// </summary>        
        public decimal qtEstoqueFinanceiro { get; set; }

        /// <summary>
        /// Obtém ou define o QtdRows.
        /// </summary>        
        [Obsolete("A consulta foi migrada de uma proc para uma query paginada.")]
        public int QtdRows { get; set; }

        /// <summary>
        /// Obtém ou define o RowCount.
        /// </summary>        
        public int RowCount { get; set; } 
    }
}