using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a status revisao custo.
    /// </summary>
    public class StatusRevisaoCustoService : EntityDomainServiceBase<StatusRevisaoCusto, IStatusRevisaoCustoGateway>, IStatusRevisaoCustoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="StatusRevisaoCustoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para status revisao custo.</param>
        public StatusRevisaoCustoService(IStatusRevisaoCustoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}
