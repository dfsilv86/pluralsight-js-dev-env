using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa um filtro pra críticas de inventário
    /// </summary>
    public class InventarioCriticaFiltro
    {
        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int? IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }     

        /// <summary>
        /// Obtém ou define dhInclusao.
        /// </summary>
        public RangeValue<DateTime> DhInclusao { get; set; }      
    }

}
