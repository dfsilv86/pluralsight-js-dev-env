using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um table data gateway para status item de nota fiscal.
    /// </summary>
    public interface INotaFiscalItemStatusGateway : IDataGateway<NotaFiscalItemStatus>
    {
    }
}
