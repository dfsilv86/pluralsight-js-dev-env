using System;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa o filtro de busca de inventários.
    /// </summary>
    public class InventarioFiltro
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int? IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id da categoria.
        /// </summary>
        public long? IdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        public int? IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o id do departamento.
        /// </summary>
        public int? IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o status do inventário.
        /// </summary>
        public InventarioStatus StInventario { get; set; }

        /// <summary>
        /// Obtém ou define a data do inventário.
        /// </summary>
        public DateTime? DhInventario { get; set; }
    }
}
