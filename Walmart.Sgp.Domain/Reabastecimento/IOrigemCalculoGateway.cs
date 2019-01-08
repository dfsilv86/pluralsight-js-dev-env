using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para um repositório de origem de cálculo.
    /// </summary>
    public interface IOrigemCalculoGateway
    {
        /// <summary>
        /// Obtém as disponibilidades das origens de cálculo (autómatico ou manual) para o dia informado.
        /// </summary>
        /// <param name="dia">O dia a ser consultado.</param>
        /// <returns>As disponibilidades.</returns>
        DisponibilidadeOrigemCalculo ObterDisponibilidade(DateTime dia);
    }
}
