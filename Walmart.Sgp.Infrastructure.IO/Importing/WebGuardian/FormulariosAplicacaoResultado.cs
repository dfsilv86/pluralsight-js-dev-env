using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de formulários do WebGuardian.
    /// </summary>
    public partial class FormulariosAplicacaoResultado : IWebGuardianResultado<FormularioTO[]>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public FormularioTO[] Dado
        {
            get
            {
                return Formularios;
            }
        }
    }
}
