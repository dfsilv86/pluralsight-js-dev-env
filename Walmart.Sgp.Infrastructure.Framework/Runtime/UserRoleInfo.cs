using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Informações sobre o papel de um usuário.
    /// </summary>
    public class UserRoleInfo
    {
        /// <summary>
        /// Obtém ou define os menus que o usuário tem acesso.
        /// </summary>
        public IEnumerable<UserMenuInfo> GrantedMenus { get; set; }

        /// <summary>
        /// Obtém ou define as ações que o usuário tem acesso.
        /// </summary>
        public IEnumerable<UserActionInfo> GrantedActions { get; set; }
    }
}
