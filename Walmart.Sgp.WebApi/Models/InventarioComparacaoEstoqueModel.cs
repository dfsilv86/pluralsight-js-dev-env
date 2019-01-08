using System;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de inventário comparação estoque.
    /// </summary>
    public class InventarioComparacaoEstoqueModel
    {
        /// <summary>
        /// Obtém ou define a Loja.
        /// </summary>
        public string Loja { get; set; }

        /// <summary>
        /// Obtém ou define a DataInventario.
        /// </summary>
        public DateTime DataInventario { get; set; }

        /// <summary>
        /// Obtém ou define o StatusInventario.
        /// </summary>
        public int StatusInventario { get; set; }

        /// <summary>
        /// Obtém ou define o Departamento.
        /// </summary>
        public string Departamento { get; set; }

        /// <summary>
        /// Obtém ou define o IDInventario.
        /// </summary>
        public int? IDInventario { get; set; }
    }
}