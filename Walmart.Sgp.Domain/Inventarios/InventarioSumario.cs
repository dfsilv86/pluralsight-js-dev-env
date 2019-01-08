namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Contém informações sumarizadas do inventário.
    /// </summary>
    public class InventarioSumario : Inventario
    {
        /// <summary>
        /// Obtém ou define o custo total.
        /// </summary>
        public decimal TotalCusto { get; set; }

        /// <summary>
        /// Obtém ou define o custo total de aprovação.
        /// </summary>
        public decimal TotalCustoAprovacao { get; set; }

        /// <summary>
        /// Obtém ou define o custo total de finalização.
        /// </summary>
        public decimal TotalCustoFinalizacao { get; set; }

        /// <summary>
        /// Obtém ou define o total de itens.
        /// </summary>
        public int TotalItens { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o inventário possui itens alterados.
        /// </summary>
        public bool PossuiItensAlterados { get; set; }
    }
}
