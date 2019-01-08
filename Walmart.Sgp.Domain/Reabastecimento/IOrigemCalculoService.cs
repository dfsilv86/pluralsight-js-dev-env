using System;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um serviço de domínio para origem de cálculos.
    /// </summary>
    public interface IOrigemCalculoService
    {
        /// <summary>
        /// Obtém as disponibilidades das origens de cálculo (autómatico ou manual) para o dia informado.
        /// </summary>
        /// <param name="dia">O dia a ser consultado.</param>
        /// <returns>As disponibilidades.</returns>
        DisponibilidadeOrigemCalculo ObterDisponibilidade(DateTime dia);
    }
}
