using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Parâmetros para autorizar pedido (tela de pesquisa de sugestão de pedido)
    /// </summary>
    public class AutorizarPedidoRequest
    {
        /// <summary>
        /// Obtém ou define a data do pedido.
        /// </summary>
        public DateTime DtPedido { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o id do departamento.
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o código da estrutura mercadológica.
        /// </summary>
        public int CdSistema { get; set; }
    }
}