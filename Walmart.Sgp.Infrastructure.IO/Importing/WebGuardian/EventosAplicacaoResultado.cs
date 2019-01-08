using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de eventos do WebGuardian.
    /// </summary>
    public partial class EventosAplicacaoResultado : IWebGuardianResultado<EventoTO[]>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public EventoTO[] Dado
        {
            get
            {
                return Eventos;
            }
        }
    }
}
