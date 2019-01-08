using System;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para bandeira em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryParametroGateway : MemoryDataGateway<Parametro>, IParametroGateway
    {
        /// <summary>
        /// Obtém o parâmetro com seus relacionamentos.
        /// </summary>
        /// <returns>
        /// O parâmetro.
        /// </returns>
        public Parametro ObterEstruturado()
        {
            throw new NotImplementedException();
        }
    }
}