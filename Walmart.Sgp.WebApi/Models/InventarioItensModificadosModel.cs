using System;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de inventário itens modificados.
    /// </summary>
    public class InventarioItensModificadosModel
    {
        /// <summary>
        /// Obtém ou define a Loja.
        /// </summary>
        public string Loja { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o DeptoCateg.
        /// </summary>
        public string DeptoCateg { get; set; }

        /// <summary>
        /// Obtém ou define o IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define a DtInventario.
        /// </summary>
        public DateTime DtInventario { get; set; }
    }
}