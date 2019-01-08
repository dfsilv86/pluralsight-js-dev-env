using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Extensões para usuário.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// // Traduz o usuário para as informações necessárias para o usuário durante a execução.
        /// </summary>
        /// <param name="user">O usuário.</param>
        /// <param name="userService">O serviço de usuário.</param>
        /// <returns>O MemoryRuntimeUser</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static MemoryRuntimeUser ToMemoryRuntimeUser(this Usuario user, IUsuarioService userService)
        {
            ExceptionHelper.ThrowIfNull("user", user);
            ExceptionHelper.ThrowIfNull("userService", userService);

            userService.ObterPorId(user.Id);

            return new MemoryRuntimeUser()
            {
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                UserName = user.UserName,
            };
        }
    }
}
