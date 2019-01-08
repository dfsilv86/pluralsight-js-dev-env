using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a RelacaoItemCD.
    /// </summary>
    public class RelacaoItemCDService : EntityDomainServiceBase<RelacaoItemCD, IRelacaoItemCDGateway>, IRelacaoItemCDService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacaoItemCDService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para RelacaoItemCD.</param>
        public RelacaoItemCDService(IRelacaoItemCDGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}