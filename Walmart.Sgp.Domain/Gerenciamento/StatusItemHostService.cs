using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a status item host.
    /// </summary>
    public class StatusItemHostService : DomainServiceBase<IStatusItemHostGateway>, IStatusItemHostService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="StatusItemHostService" />.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para status item host.</param>
        public StatusItemHostService(IStatusItemHostGateway mainGateway)
                    : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém todos os status disponíveis no idioma informado.
        /// </summary>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os status.</returns>
        public IEnumerable<StatusItemHost> ObterPorCultura(string cultureCode)
        {
            return this.MainGateway.ObterPorCultura(cultureCode);
        }
        #endregion
    }
}
