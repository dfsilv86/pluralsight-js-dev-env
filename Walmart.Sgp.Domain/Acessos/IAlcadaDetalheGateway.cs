using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um table data gateway para AlcadaDetalhe.
    /// </summary>
    public interface IAlcadaDetalheGateway : IDataGateway<AlcadaDetalhe>
    {
        /// <summary>
        /// Obter uma lista de AlcadaDetalhe por IdAlcada.
        /// </summary>
        /// <param name="idAlcada">O id da Alcada.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>A lista de AlcadaDetalhe.</returns>
        IEnumerable<AlcadaDetalhe> ObterPorIdAlcada(int idAlcada, Paging paging);
    }
}
