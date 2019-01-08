using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Filtro de relacionamento transferencia
    /// </summary>
    public class RelacionamentoTransferenciaFiltro
    {
        /// <summary>
        /// Obtém ou define IdBandeira.
        /// </summary>
        public int IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IdLoja.
        /// </summary>
        public int? IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o IdDepartamento.
        /// </summary>
        public int? IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define CdItem.
        /// </summary>
        public long? CdItem { get; set; }

        /// <summary>
        /// Obtém ou define DsItem.
        /// </summary>
        public string DsItem { get; set; }
    }
}
