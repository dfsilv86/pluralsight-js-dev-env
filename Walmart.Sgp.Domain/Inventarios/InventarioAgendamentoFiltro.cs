using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Filtro para agendamento de inventário.
    /// </summary>
    public class InventarioAgendamentoFiltro
    {
        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o código da loja.
        /// </summary>
        public int? CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o código do departamento.
        /// </summary>
        public int? CdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define a data de agendamento.
        /// </summary>
        public DateTime? DtAgendamento { get; set; }
    }
}
