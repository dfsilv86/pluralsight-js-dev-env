using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Informações necessárias para validar sugestões de pedido.
    /// </summary>
    public class ValidarSugestaoRequest
    {
        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o id do departamento.
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define a data do pedido.
        /// </summary>        
        public DateTime DtPedido { get; set; }

        /// <summary>
        /// Obtém ou define a sugestão.
        /// </summary>
        public SugestaoPedidoModel Sugestao { get; set; }
    }
}