using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de usuários do WebGuardian.
    /// </summary>
    public partial class UsuariosAplicacaoResultado : IWebGuardianResultado<UsuarioTO[]>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public UsuarioTO[] Dado
        {
            get
            {
                return Usuarios;
            }
        }
    }
}
