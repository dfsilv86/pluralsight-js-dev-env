using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um serviço de AlcadaDetalhe.
    /// </summary>
    public interface IAlcadaDetalheService : IDomainService<AlcadaDetalhe>
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
