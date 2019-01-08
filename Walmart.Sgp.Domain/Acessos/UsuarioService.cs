using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Serviço de domínio relacionado a usuário.
    /// </summary>
    public class UsuarioService : EntityDomainServiceBase<Usuario, IUsuarioGateway>, IUsuarioService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioService"/>.
        /// </summary>
        /// <param name="usuarioGateway">O table data gateway para usuário.</param>
        public UsuarioService(IUsuarioGateway usuarioGateway)
            : base(usuarioGateway)
        {            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o usuário pelo username.
        /// </summary>
        /// <param name="userName">O username do usuário desejado.</param>
        /// <returns>
        /// O usuário, se existir.
        /// </returns>
        public Usuario ObterPorUserName(string userName)
        {            
            return MainGateway.Find("UserName = @UserName", new { userName }).SingleOrDefault();
        }

        /// <summary>
        /// Pesquisa a lista de usuários pelo nome informado.
        /// </summary>
        /// <param name="userName">O nome de usuário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os usuários onde o nome de usuário ou login contém o nome informado.</returns>
        public IEnumerable<Usuario> Pesquisar(string userName, Paging paging)
        {
            return MainGateway.Find("UserName LIKE '%' +  @UserName + '%' OR FullName LIKE '%' +  @UserName + '%'", new { userName }, paging).RemoverSenha();
        }

        /// <summary>
        /// Obtém informações básicas do usuário pelo nome de usuário.
        /// </summary>
        /// <param name="userName">O nome de usuário (login).</param>
        /// <returns>O usuário.</returns>
        public UsuarioResumo ObterResumidoPorUserName(string userName)
        {
            Assert(new { userName }, new AllMustBeInformedSpec());

            return this.ObterPorUserName(userName).Resumir();
        }

        /// <summary>
        /// Obtém informações básicas do usuário pelo id.
        /// </summary>
        /// <param name="id">O id de usuário.</param>
        /// <returns>O usuário.</returns>
        public UsuarioResumo ObterResumidoPorId(int id)
        {
            Assert(new { id }, new AllMustBeInformedSpec());

            return this.ObterPorId(id).Resumir();
        }

        /// <summary>
        /// Pesquisa informações básicas sobre usuários.
        /// </summary>
        /// <param name="userName">Nome de usuário (login).</param>
        /// <param name="fullName">Nome completo do usuário.</param>
        /// <param name="email">Email do usuário.</param>
        /// <param name="cdUsuario">O id do usuário.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os usuários.</returns>
        public IEnumerable<UsuarioResumo> PesquisarResumidoPorUsuario(string userName, string fullName, string email, int? cdUsuario, Paging paging)
        {
            return this.MainGateway.Find<UsuarioResumo>(
                "UserName,FullName,Email",
                "(@UserName IS NULL OR UserName LIKE '%' + @UserName + '%') AND (@FullName IS NULL OR FullName LIKE '%' + @FullName + '%') AND (@Email IS NULL OR Email LIKE '%' + @Email + '%') AND (@CdUsuario IS NULL OR Id = @CdUsuario)", 
                new { cdUsuario, fullName, userName, email }, 
                paging);
        }

        #endregion
    }
}
