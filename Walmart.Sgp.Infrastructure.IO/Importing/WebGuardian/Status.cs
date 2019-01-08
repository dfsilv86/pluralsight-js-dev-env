using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado do webguardian que possui somente o status.
    /// </summary>
    public partial class Status : IWebGuardianResultado
    {
        /// <summary>
        /// Obtém o status da ação no WebGuardian.
        /// </summary>
        Status IWebGuardianResultado.Status
        {
            get { return this; }
        }
    }
}
