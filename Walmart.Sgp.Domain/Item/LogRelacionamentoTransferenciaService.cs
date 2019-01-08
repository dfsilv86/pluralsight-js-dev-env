using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Serviço de domínio relacionado a log relacionamento transferencia entre itens.
    /// </summary>
    public class LogRelacionamentoTransferenciaService : EntityDomainServiceBase<LogRelacionamentoTransferencia, ILogRelacionamentoTransferenciaGateway>, ILogRelacionamentoTransferenciaService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LogRelacionamentoTransferenciaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para log relacionamento transferencia.</param>
        public LogRelacionamentoTransferenciaService(
            ILogRelacionamentoTransferenciaGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}
