namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa o result set da procedure de consulta de itens (PR_SelecionarItem_Page).
    /// </summary>
    public class ResultadoConsultaItem
    {
        /// <summary>
        /// Obtém ou define o IDItemDetalhe.
        /// </summary>        
        public long IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o descricao.
        /// </summary>
        public string descricao { get; set; }

        /// <summary>
        /// Obtém ou define o bandeira.
        /// </summary>
        public string bandeira { get; set; }

        /// <summary>
        /// Obtém ou define o IDBandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o dsDepartamento.
        /// </summary>
        public string dsDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o cdItem.
        /// </summary>
        public int cdItem { get; set; }

        /// <summary>
        /// Obtém ou define o plu.
        /// </summary>
        public long? plu { get; set; }

        /// <summary>
        /// Obtém ou define o upc.
        /// </summary>
        public decimal? upc { get; set; }

        /// <summary>
        /// Obtém ou define o manipulado.
        /// </summary>
        public string manipulado { get; set; }

        /// <summary>
        /// Obtém ou define o vinculado.
        /// </summary>
        public string vinculado { get; set; }

        /// <summary>
        /// Obtém ou define o receituario.
        /// </summary>
        public string receituario { get; set; }

        /// <summary>
        /// Obtém ou define o tamanho.
        /// </summary>
        public string tamanho { get; set; }

        /// <summary>
        /// Obtém ou define o dsAreaCD.
        /// </summary>
        public string dsAreaCD { get; set; }

        /// <summary>
        /// Obtém ou define o dsRegiaoCompra.
        /// </summary>
        public string dsRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define o QtdRows.
        /// </summary>
        public int QtdRows { get; set; }

        /// <summary>
        /// Obtém ou define o RowCount.
        /// </summary>
        public int RowCount { get; set; }
    }
}