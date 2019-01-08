using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um table data gateway para permissão de loja.
    /// </summary>
    public interface IPermissaoLojaGateway : IDataGateway<PermissaoLoja>
    {
        /// <summary>
        /// Verifica se usuário possui permissâo à loja.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Retorna true caso possua permissão, false do contrário.</returns>
        bool UsuarioPossuiPermissaoLoja(int idUsuario, int idLoja);
    }
}