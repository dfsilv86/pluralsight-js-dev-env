using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Valores utilizados como filtro para pesquisa de sugestao de pedido.
    /// </summary>
    public class SugestaoPedidoFiltro
    {
        /// <summary>
        /// Obtém ou define o id do usuário.
        /// </summary>
        public int IDUsuario { get; set; }

        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int? IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o código da estrutura mercadológica.
        /// </summary>
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de permissão.
        /// </summary>
        public int? TipoPermissao { get; set; }

        /// <summary>
        /// Obtém ou define o código da loja.
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define a data do pedido.
        /// </summary>
        public DateTime dtPedido { get; set; }

        /// <summary>
        /// Obtém ou define o código do item.
        /// </summary>
        public int? cdItem { get; set; }

        /// <summary>
        /// Obtém ou define o código do fineline.
        /// </summary>
        public int? cdFineLine { get; set; }

        /// <summary>
        /// Obtém ou define o código da subcategoria.
        /// </summary>
        public int? cdSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o código da categoria.
        /// </summary>
        public int? cdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o código do departamento.
        /// </summary>
        public int cdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o código da divisão.
        /// </summary>
        public int? cdDivisao { get; set; }

        /// <summary>
        /// Obtém ou define a origem do cálculo da sugestão.
        /// </summary>
        public string cdOrigemCalculo { get; set; }

        /// <summary>
        /// Obtém ou define o código de 9 dígitos do vendor.
        /// </summary>
        public int? cdVendor { get; set; }
    }
}
