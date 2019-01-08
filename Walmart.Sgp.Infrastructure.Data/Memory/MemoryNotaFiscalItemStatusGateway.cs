using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para nota fiscal item status em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryNotaFiscalItemStatusGateway : MemoryDataGateway<NotaFiscalItemStatus>, INotaFiscalItemStatusGateway
    {
    }
}