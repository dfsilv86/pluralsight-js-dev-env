using System;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa a disponibilidade de origens de cálculo em determinado dia.
    /// </summary>
    public class DisponibilidadeOrigemCalculo
    {
        /// <summary>
        /// Obtém ou define o dia da disponibilidade;
        /// </summary>
        public DateTime Dia { get; set; }

        /// <summary>
        /// Obtém ou define se a origem de cálculo manual está disponível.
        /// </summary>
        public bool ManualDisponivel { get; set; }

        /// <summary>
        /// Obtém ou define se a origem de cálculo automático está disponível.
        /// </summary>
       public bool AutomaticoDisponivel { get; set; }
    }
}
