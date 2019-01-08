using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um serviço de regiao.
    /// </summary>
    public interface IRegiaoService : IDomainService<Regiao>
    {
        /// <summary>
        /// Obtém as regiões cadastradas para uma bandeira.
        /// </summary>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <returns>As regiões</returns>
        IEnumerable<Regiao> ObterPorBandeira(int idBandeira);
    }
}
