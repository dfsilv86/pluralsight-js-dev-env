using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Filtro de Loja
    /// </summary>
    public class LojaFiltro
    {
        /// <summary>
        /// Obtém ou define o CdSistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o IdItemDetalheDestino.
        /// </summary>
        public long IdItemDetalheDestino { get; set; }

        /// <summary>
        /// Obtém ou define o IdItemDetalheOrigem.
        /// </summary>
        public long IdItemDetalheOrigem { get; set; }

        /// <summary>
        /// Obtém ou define o IdLoja.
        /// </summary>
        public int? IdLoja { get; set; }
    }
}
