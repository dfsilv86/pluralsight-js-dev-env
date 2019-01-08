using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma SugestaoReturnSheetRARequest.
    /// </summary>
    public class SugestaoReturnSheetRARequest
    {
        /// <summary>
        /// Obtém ou define DtInicioReturn.
        /// </summary>
        public DateTime? DtInicioReturn { get; set; }

        /// <summary>
        /// Obtém ou define DtFinalReturn.
        /// </summary>
        public DateTime? DtFinalReturn { get; set; }

        /// <summary>
        /// Obtém ou define dtSolicitacao.
        /// </summary>
        public long? CdV9D { get; set; }

        /// <summary>
        /// Obtém ou define Evento.
        /// </summary>
        public string Evento { get; set; }

        /// <summary>
        /// Obtém ou define CdItemDetalhe.
        /// </summary>
        public int? CdItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define CdDepartamento.
        /// </summary>
        public int? CdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define CdLoja.
        /// </summary>
        public int? CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define IdRegiaoCompra.
        /// </summary>
        public int? IdRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define BlExportado.
        /// </summary>
        public bool? BlExportado { get; set; }

        /// <summary>
        /// Obtém ou define BlAutorizado.
        /// </summary>
        public bool? BlAutorizado { get; set; }
    }
}
