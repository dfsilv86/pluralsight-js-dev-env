using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para item de nota fiscal em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryNotaFiscalItemGateway : MemoryDataGateway<NotaFiscalItem>, INotaFiscalItemGateway
    {
        /// <summary>
        /// Salva o log de correção de custo.
        /// </summary>
        /// <param name="notaFiscalItem">O item de nota fiscal.</param>
        public void SalvarLogCorrecaoCusto(NotaFiscalItem notaFiscalItem)
        {
        }

        /// <summary>
        /// Libera item de nota fiscal divergente.
        /// </summary>
        /// <param name="notaFiscalItem">O item de nota fiscal.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        public void LiberarItemNotaFiscalDivergente(NotaFiscalItem notaFiscalItem, int idBandeira)
        {
        }
    }
}