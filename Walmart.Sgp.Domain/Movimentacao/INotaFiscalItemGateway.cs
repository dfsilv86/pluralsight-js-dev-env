using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um table data gateway para item de nota fiscal.
    /// </summary>
    public interface INotaFiscalItemGateway : IDataGateway<NotaFiscalItem>
    {
        /// <summary>
        /// Salva o log de correção de custo.
        /// </summary>
        /// <param name="notaFiscalItem">O item de nota fiscal.</param>
        void SalvarLogCorrecaoCusto(NotaFiscalItem notaFiscalItem);

        /// <summary>
        /// Libera item de nota fiscal divergente.
        /// </summary>
        /// <param name="notaFiscalItem">O item de nota fiscal.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        void LiberarItemNotaFiscalDivergente(NotaFiscalItem notaFiscalItem, int idBandeira);
    }
}
