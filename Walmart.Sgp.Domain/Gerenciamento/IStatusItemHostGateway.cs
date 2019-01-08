using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um table data gateway para status item host.
    /// </summary>
    public interface IStatusItemHostGateway
    {
        /// <summary>
        /// Obtém todos os status disponíveis no idioma informado.
        /// </summary>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os status.</returns>
        IEnumerable<StatusItemHost> ObterPorCultura(string cultureCode);
    }
}
