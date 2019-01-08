using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de RoteiroLoja.
    /// </summary>
    public interface IRoteiroLojaService : IDomainService<RoteiroLoja>
    {
        /// <summary>
        /// Obtém as lojas válidas para vínculo com o roteiro.
        /// </summary>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="dsEstado">O estado da loja.</param>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A lista contendo as lojas válidas para vínculo com o roteiro.</returns>
        IEnumerable<RoteiroLoja> ObterLojasValidas(long cdV9D, string dsEstado, int? idRoteiro, Paging paging);

        /// <summary>
        /// Obtém as lojas pelo roteiro.
        /// </summary>
        /// <param name="idRoteiro">O id.</param>
        /// <returns>A lista de lojas.</returns>
        IEnumerable<RoteiroLoja> ObterPorIdRoteiro(long idRoteiro);
    }
}