using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Filtro de parâmetro de loja/CD.
    /// </summary>
    public class LojaCdParametroFiltro
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
        /// Obtém ou define o nome da loja.
        /// </summary>
        public string NmLoja { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de reabastecimento.
        /// </summary>
        public TipoReabastecimento TpReabastecimento { get; set; }
    }
}
