using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a motivo revisao custo.
    /// </summary>
    public class MotivoRevisaoCustoService : EntityDomainServiceBase<MotivoRevisaoCusto, IMotivoRevisaoCustoGateway>, IMotivoRevisaoCustoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MotivoRevisaoCustoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para motivo revisao custo.</param>
        public MotivoRevisaoCustoService(IMotivoRevisaoCustoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}
