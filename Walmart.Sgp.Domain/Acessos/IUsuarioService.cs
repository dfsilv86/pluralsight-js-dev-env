using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um serviço de usuário.
    /// </summary>
    public interface IUsuarioService : IDomainService<Usuario>
    {
        /// <summary>
        /// Obtém o usuário pelo username.
        /// </summary>
        /// <param name="userName">O username do usuário desejado.</param>
        /// <returns>O usuário, se existir.</returns>
        /// <remarks>Usado pelo UsuarioImporter.ImportarUsuario() e AuthController.Logar*()</remarks>
        Usuario ObterPorUserName(string userName);

        /// <summary>
        /// Pesquisa a lista de usuários pelo nome informado.
        /// </summary>
        /// <param name="userName">O nome de usuário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os usuários onde o nome de usuário ou login contém o nome informado.</returns>        
        IEnumerable<Usuario> Pesquisar(string userName, Paging paging);

        /// <summary>
        /// Obtém informações básicas do usuário pelo nome de usuário.
        /// </summary>
        /// <param name="userName">O nome de usuário (login).</param>
        /// <returns>O usuário.</returns>
        UsuarioResumo ObterResumidoPorUserName(string userName);

        /// <summary>
        /// Obtém informações básicas do usuário pelo id.
        /// </summary>
        /// <param name="id">O id de usuário.</param>
        /// <returns>O usuário.</returns>
        UsuarioResumo ObterResumidoPorId(int id);

        /// <summary>
        /// Pesquisa informações básicas sobre usuários.
        /// </summary>
        /// <param name="userName">Nome de usuário (login).</param>
        /// <param name="fullName">Nome completo do usuário.</param>
        /// <param name="email">Email do usuário.</param>
        /// <param name="cdUsuario">O id do usuário.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os usuários.</returns>
        IEnumerable<UsuarioResumo> PesquisarResumidoPorUsuario(string userName, string fullName, string email, int? cdUsuario, Paging paging);
    }
}
