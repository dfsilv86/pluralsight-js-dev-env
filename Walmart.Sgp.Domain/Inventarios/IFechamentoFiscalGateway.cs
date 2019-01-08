using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um table data gateway para fechamento fiscal.
    /// </summary>
    public interface IFechamentoFiscalGateway : IDataGateway<FechamentoFiscal>
    {
        /// <summary>
        /// Obtém o ultimo fechamento fiscal da loja.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>
        /// O ultimo fechamento fiscal da loja.
        /// </returns>
        FechamentoFiscal ObterUltimo(int idLoja);
    }
}
