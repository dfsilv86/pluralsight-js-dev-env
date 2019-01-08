using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Filtro para carga de processo.
    /// </summary>
    public class ProcessoCargaFiltro
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int? IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o código da loja.
        /// </summary>
        public int? CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define a data.
        /// </summary>
        public DateTime Data { get; set; }
    }
}
