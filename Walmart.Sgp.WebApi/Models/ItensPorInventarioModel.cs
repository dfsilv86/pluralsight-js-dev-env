using System;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de itens por inventário.
    /// </summary>
    public class ItensPorInventarioModel
    {
        /// <summary>
        /// Obtém ou define a Loja.
        /// </summary>
        public string Loja { get; set; }

        /// <summary>
        /// Obtém ou define o IdLoja.
        /// </summary>
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o CdLoja.
        /// </summary>
        public int CdLoja { get; set; }
        
        /// <summary>
        /// Obtém ou define o StInventario.
        /// </summary>
        public int StInventario { get; set; }

        /// <summary>
        /// Obtém ou define a DtInventario.
        /// </summary>
        public DateTime DtInventario { get; set; }
    }
}