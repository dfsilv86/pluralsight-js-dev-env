using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento.Roteirizacao
{
    /// <summary>
    /// Define um Pedido roteirizado
    /// </summary>
    public class PedidoRoteirizadoConsolidado
    {
        /// <summary>
        /// Obtém ou define idRoteiro
        /// </summary>
        public int idRoteiro { get; set; }

        /// <summary>
        /// Obtém ou define cdV9D
        /// </summary>
        public long? cdV9D { get; set; }

        /// <summary>
        /// Obtém ou define nmFornecedor
        /// </summary>
        public string nmFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define Descricao
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define blKgCx
        /// </summary>
        public bool? blKgCx { get; set; }

        /// <summary>
        /// Obtém ou define vlCargaMinima
        /// </summary>
        public int? vlCargaMinima { get; set; }

        /// <summary>
        /// Obtém ou define blAutorizado
        /// </summary>
        public bool? blAutorizado { get; set; }

        /// <summary>
        /// Obtém ou define DhAutorizacao
        /// </summary>
        public DateTime? DhAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAutorizacao
        /// </summary>
        public string UsuarioAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define TotalPedido
        /// </summary>
        public int? TotalPedido { get; set; }

        /// <summary>
        /// Obtém ou define chkAutorizar
        /// </summary>
        public bool chkAutorizar { get; set; }

        /// <summary>
        /// Obtém ou define DtPedido
        /// </summary>
        public DateTime? DtPedido { get; set; }

        /// <summary>
        /// Obtém ou define TotalPedidoRA
        /// </summary>
        public int? TotalPedidoRA { get; set; }
    }
}
