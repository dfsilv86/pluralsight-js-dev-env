using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Define a interface para um serviço de SSO (Single Sign-on).
    /// </summary>
    public interface ISsoService
    {
        /// <summary>
        /// Altera a senha do usuário.
        /// </summary>
        /// <param name="userName">O nome do usuário.</param>
        /// <param name="currentPassword">A senha atual.</param>
        /// <param name="newPassword">A nova senha.</param>
        void AlterarSenha(string userName, string currentPassword, string newPassword);

        /// <summary>
        /// Obtém a lista de ações seguras da aplicação.
        /// </summary>
        /// <param name="userName">O nome do usuário.</param>
        /// <param name="password">A senha.</param>
        /// <param name="idExternoAplicacao">O id externo da aplicação.</param>
        /// <returns>As ações.</returns>
        IEnumerable<UserActionInfo> ObterAcoesSegurasDaAplicacao(string userName, string password, int idExternoAplicacao);

        /// <summary>
        /// Obtém as informações do papel, como menus e ações concedidas.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoPapel">O id do papel no WebGuardian.</param>
        /// <returns>As informações do papel.</returns>
        UserRoleInfo ObterInformacoesDoPapel(string userName, string password, int idExternoPapel);

        /// <summary>
        /// Obtém os papéis do usuário na aplicação.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoAplicacao">O id da aplicação no WebGuardian.</param>
        /// <returns>Os papéis.</returns>
        IEnumerable<Papel> ObterPapeis(string userName, string password, int idExternoAplicacao);

        /// <summary>
        /// Obtém o papel por seu id no WebGuardian.
        /// </summary>
        /// <param name="idExternoPapel">O id do papel no WebGuardian.</param>
        /// <returns>O papel.</returns>
        Papel ObterPapel(int idExternoPapel);

        /// <summary>
        /// Obtém o usuário.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <returns>O usuário.</returns>
        Usuario ObterUsuario(string userName, string password);

        /// <summary>
        /// Obtém os usuários.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoAplicacao">O id da aplicação no WebGuardian.</param>
        /// <returns>Os usuários.</returns>
        IEnumerable<Usuario> ObterUsuarios(string userName, string password, int idExternoAplicacao);
    }
}
