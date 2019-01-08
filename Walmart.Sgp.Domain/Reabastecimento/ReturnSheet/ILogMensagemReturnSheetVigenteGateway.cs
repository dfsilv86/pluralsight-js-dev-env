using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para LogMensagemReturnSheetVigente.
    /// </summary>
    public interface ILogMensagemReturnSheetVigenteGateway : IDataGateway<LogMensagemReturnSheetVigente>
    {
        /// <summary>
        /// Obtém um LogMensagemReturnSheetVigente pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade LogMensagemReturnSheetVigente.</returns>
        LogMensagemReturnSheetVigente ObterPorId(long id);
    }
}