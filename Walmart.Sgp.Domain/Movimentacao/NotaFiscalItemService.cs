using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de cadastro de item de nota fiscal.
    /// </summary>
    public class NotaFiscalItemService : EntityDomainServiceBase<NotaFiscalItem, INotaFiscalItemGateway>, INotaFiscalItemService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotaFiscalItemService"/>.
        /// </summary>
        /// <param name="mainGateway">O gateway.</param>
        public NotaFiscalItemService(INotaFiscalItemGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}
