using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Filtro de Bandeira
    /// </summary>
    public class BandeiraFiltro
    {
        /// <summary>
        /// Obtém ou define o CdSistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define DsBandeira.
        /// </summary>
        public string DsBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id do formato.
        /// </summary>
        public int? IDFormato { get; set; }
    }
}
