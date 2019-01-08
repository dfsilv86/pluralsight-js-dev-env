using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a LogMensagemReturnSheetVigente.
    /// </summary>
    public class LogMensagemReturnSheetVigenteService : EntityDomainServiceBase<LogMensagemReturnSheetVigente, ILogMensagemReturnSheetVigenteGateway>, ILogMensagemReturnSheetVigenteService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LogMensagemReturnSheetVigenteService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para LogMensagemReturnSheetVigente.</param>
        public LogMensagemReturnSheetVigenteService(ILogMensagemReturnSheetVigenteGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}