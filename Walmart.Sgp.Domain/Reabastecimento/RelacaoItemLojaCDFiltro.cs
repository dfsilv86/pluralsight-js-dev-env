using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Classe de filtro de RelacaoItemLojaCD
    /// </summary>
    public class RelacaoItemLojaCDFiltro
    {
        /// <summary>
        /// Codigo item saida
        /// </summary>
        public long? cdItemSaida { get; set; }

        /// <summary>
        /// UF estado
        /// </summary>
        public string dsEstado { get; set; }

        /// <summary>
        /// id da regiao de compra
        /// </summary>
        public int? idRegiaoCompra { get; set; }

        /// <summary>
        /// id da bandeira
        /// </summary>
        public int? idBandeira { get; set; }

        /// <summary>
        /// bool de pendende de vinculacao
        /// </summary>
        public bool blVinculado { get; set; }

        /// <summary>
        /// o codigo do fornecedor
        /// </summary>
        public int? idFornecedorParametro { get; set; }

        /// <summary>
        /// o codigo do sistema
        /// </summary>
        public int? cdSistema { get; set; }

        /// <summary>
        /// O codigo 9 digitos do fornecedor.
        /// </summary>
        public int? cdV9D { get; set; }
    }
}
