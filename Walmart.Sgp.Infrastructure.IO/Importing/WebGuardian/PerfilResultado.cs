using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de perfil do WebGuardian.
    /// </summary>
    public partial class PerfilResultado : IWebGuardianResultado<PerfilTO>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public PerfilTO Dado
        {
            get { return Perfil; }
        }
    }
}
