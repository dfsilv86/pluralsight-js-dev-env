using System;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de inventário agendamento.
    /// </summary>
    public class InventarioAgendamentoModel
    {
        /// <summary>
        /// Obtém ou define o DsBandeira.
        /// </summary>
        public string DsBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o IDBandeira.
        /// </summary>
        public int? IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o NmLoja.
        /// </summary>
        public string NmLoja { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int? IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o DsDepartamento.
        /// </summary>
        public string DsDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o NaoAgendados.
        /// </summary>
        public int NaoAgendados { get; set; }

        /// <summary>
        /// Obtém ou define o DtAgendamento.
        /// </summary>
        public DateTime? DtAgendamento { get; set; }
    }
}