using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Representa um resultado do WebGuardian.
    /// </summary>
    public class WebGuardianResultado : IWebGuardianResultado
    {
        private readonly Status m_status;
        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianResultado"/>.
        /// </summary>
        /// <param name="status">O status.</param>
        public WebGuardianResultado(Status status)
        {
            m_status = status;
        }

        /// <summary>
        /// Obtém o status da operação no WebGuardian.
        /// </summary>
        public Status Status
        {
            get { return m_status; }
        }

        /// <summary>
        /// Operator implicit de conversão de status para WebGuardianResultado.
        /// </summary>
        /// <param name="status">O status.</param>
        /// <returns>A instância de WebGuardianResultado.</returns>
        public static implicit operator WebGuardianResultado(Status status)
        {
            return new WebGuardianResultado(status);
        }
    }
}
