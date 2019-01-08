namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa o filtro de itens de inventário.
    /// </summary>
    public class InventarioItemFiltro
    {
        /// <summary>
        /// Obtém ou define o id do inventario.
        /// </summary>
        public int IDInventario { get; set; }

        /// <summary>
        /// Obtém ou define o código antigo.
        /// </summary>
        public int? CdOldNumber { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do item.
        /// </summary>
        public string dsItem { get; set; }

        /// <summary>
        /// Obtém ou define o código plu.
        /// </summary>
        public long? cdPlu { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de filtro.
        /// </summary>
        public TipoFiltroItemInventario filtro { get; set; }
    }
}