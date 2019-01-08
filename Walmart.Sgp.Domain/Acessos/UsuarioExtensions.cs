using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Extension methods para usuario.
    /// </summary>
    public static class UsuarioExtensions
    {
        /// <summary>
        /// Limpa o usuário, removendo a senha e outras informações que não devem ser retornadas pela api.
        /// </summary>
        /// <param name="usuario">O usuário.</param>
        /// <returns>O usuário limpo.</returns>
        public static Usuario RemoverSenha(this Usuario usuario)
        {
            // TODO: isto deve virar um filtro da webapi para remover propriedades do usuario durante a serialização
            if (null == usuario)
            {
                return null;
            }

            usuario.Passwd = null;
            usuario.PasswdFormat = 0;
            usuario.PasswordAnswer = null;
            usuario.PasswordQuestion = null;

            return usuario;
        }

        /// <summary>
        /// Limpa o usuário, removendo a senha e outras informações que não devem ser retornadas pela api.
        /// </summary>
        /// <param name="usuarios">Os usuários.</param>
        /// <returns>Os usuários limpos.</returns>
        public static IEnumerable<Usuario> RemoverSenha(this IEnumerable<Usuario> usuarios)
        {
            // TODO: isto deve virar um filtro da webapi para remover propriedades do usuario durante a serialização
            if (null == usuarios)
            {
                return null;
            }

            return usuarios.Select(RemoverSenha);
        }

        /// <summary>
        /// Resume o usuário, retornando apenas seu id, fullName e userName.
        /// </summary>
        /// <param name="usuario">O usuário.</param>
        /// <returns>O usuário resumido.</returns>
        public static UsuarioResumo Resumir(this Usuario usuario)
        {
            if (null == usuario)
            {
                return null;
            }

            return new UsuarioResumo { Id = usuario.Id, FullName = usuario.FullName, UserName = null != usuario.UserName ? usuario.UserName.Trim() : null, Email = usuario.Email };
        }
    }
}
