using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de pedido roteirizado.
    /// </summary>
    public class PedidoRoteirizadoModel
    {
        /// <summary>
        /// Obtém ou define o IDRoteiro.
        /// </summary>
        public int IDRoteiro { get; set; }

        /// <summary>
        /// Obtém ou define o DsRoteiro.
        /// </summary>
        public string DsRoteiro { get; set; }

        /// <summary>
        /// Obtém ou define o NmVendor.
        /// </summary>
        public string NmVendor { get; set; }

        /// <summary>
        /// Obtém ou define o CdVendor.
        /// </summary>
        public int CdVendor { get; set; }

        /// <summary>
        /// Obtém ou define o IDItemDetalhe.
        /// </summary>
        public int? IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o DtPedido.
        /// </summary>
        public DateTime DtPedido { get; set; }

        public string DataPedido
        {
            get
            {
                return this.DtPedido.ToString("MM/dd/yyyy");
            }
        }
    }

}