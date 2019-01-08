using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa um item de inventario com dados sumarizados.
    /// </summary>
    public class InventarioItemSumario : InventarioItem
    {
        /// <summary>
        /// Obtém ou define o custo total.
        /// </summary>
        public decimal? vlCustoTotal { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade do item no inventário anterior.
        /// </summary>
        public decimal? qtItemInventarioAnterior { get; set; }
    }
}
