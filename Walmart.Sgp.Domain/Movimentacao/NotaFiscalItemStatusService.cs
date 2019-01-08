using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de domínio relacionado a status item da nota fiscal.
    /// </summary>
    public class NotaFiscalItemStatusService : EntityDomainServiceBase<NotaFiscalItemStatus, INotaFiscalItemStatusGateway>, INotaFiscalItemStatusService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotaFiscalItemStatusService"/>.
        /// </summary>
        /// <param name="notaFiscalItemStatusGateway">O table data gateway para status item da nota fiscal.</param>
        public NotaFiscalItemStatusService(INotaFiscalItemStatusGateway notaFiscalItemStatusGateway)
            : base(notaFiscalItemStatusGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém todos os status disponíveis.
        /// </summary>
        /// <returns>Os status.</returns>
        public override IEnumerable<NotaFiscalItemStatus> ObterTodos()
        {
            return MainGateway.FindAll(new Paging("DsNotaFiscalItemStatus"));
        }
        #endregion
    }
}
