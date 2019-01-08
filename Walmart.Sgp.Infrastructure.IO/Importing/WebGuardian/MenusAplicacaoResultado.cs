using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de menus do WebGuardian.
    /// </summary>
    public partial class MenusAplicacaoResultado : IWebGuardianResultado<MenuTO[]>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public MenuTO[] Dado
        {
            get
            {
                return Menus;
            }
        }
    }
}
