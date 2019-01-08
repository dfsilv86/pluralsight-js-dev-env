using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Filtro de item.
    /// </summary>
    public class ItemDetalheFiltro
    {
        /// <summary>
        /// Obtém ou define o código do item.
        /// </summary>
        public long? CdItem { get; set; }

        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public byte CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o código do fornecedor.
        /// </summary>
        public int? IdFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define o código do plu.
        /// </summary>
        public long? CdPLU { get; set; }

        /// <summary>
        /// Obtém ou define a descricao do item.
        /// </summary>
        public string DsItem { get; set; }

        /// <summary>
        /// Obtém ou define o status do item.
        /// </summary>
        public string TpStatus { get; set; }

        /// <summary>
        /// Obtém ou define o código do fineline.
        /// </summary>
        public int? CdFineLine { get; set; }

        /// <summary>
        /// Obtém ou define o código da subcategoria.
        /// </summary>
        public int? CdSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o código da categoria.
        /// </summary>
        public int? CdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o código do departamento.
        /// </summary>
        public int? CdDepartamento { get; set; }

        /// <summary>
        /// Obtem ou define o id da regiao de compra.
        /// </summary>
        public int? IDRegiaoCompra { get; set; }
        
        /// <summary>
        /// Obtém ou define blPerecivel.
        /// </summary>
        public string blPerecivel { get; set; }
    }
}
