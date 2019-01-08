using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um table data gateway para permissão de bandeira.
    /// </summary>
    public interface IPermissaoBandeiraGateway : IDataGateway<PermissaoBandeira>
    {
        /// <summary>
        /// Verifica se usuário possui permissâo à bandeira.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <returns>Retorna true caso possua permissão, false do contrário.</returns>
        bool UsuarioPossuiPermissaoBandeira(int idUsuario, int idBandeira);
    }
}