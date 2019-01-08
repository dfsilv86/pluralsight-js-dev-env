using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa um inventário não agendado.
    /// </summary>
    public class InventarioNaoAgendado
    {
        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }
    }
}
