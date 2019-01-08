using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Filtro de revisao de custo
    /// </summary>
    public class RevisaoCustoFiltro
    {
        /// <summary>
        /// Obtém ou define o id da Bandeira.
        /// </summary>
        public int IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id da Loja.
        /// </summary>
        public int? IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o id do Departamento.
        /// </summary>
        public int? IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o codigo do Item.
        /// </summary>
        public long? CdItem { get; set; }

        /// <summary>
        /// Obtém ou define a descricao do Item.
        /// </summary>
        public string DsItem { get; set; }

        /// <summary>
        /// Obtém ou define o Id do Status.
        /// </summary>
        public int? IdStatus { get; set; }
    }
}
