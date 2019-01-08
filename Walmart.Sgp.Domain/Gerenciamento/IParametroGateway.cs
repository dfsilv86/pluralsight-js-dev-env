using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um table data gateway para parâmetro.
    /// </summary>
    public interface IParametroGateway : IDataGateway<Parametro>
    {
        /// <summary>
        /// Obtém o parâmetro com seus relacionamentos.
        /// </summary>
        /// <returns>
        /// O parâmetro.
        /// </returns>
        Parametro ObterEstruturado();
    }
}