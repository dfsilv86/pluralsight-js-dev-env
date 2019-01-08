using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a região administrativa.
    /// </summary>
    public class RegiaoAdministrativaService : EntityDomainServiceBase<RegiaoAdministrativa, IRegiaoAdministrativaGateway>, IRegiaoAdministrativaService
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RegiaoAdministrativaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para região administrativa.</param>
        public RegiaoAdministrativaService(IRegiaoAdministrativaGateway mainGateway)
            : base(mainGateway)
        {
        }

        #endregion
    }
}
