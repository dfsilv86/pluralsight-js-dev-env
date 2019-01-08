using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Status do Item no Host.
    /// </summary>
    public class StatusItemHost
    {
        /// <summary>
        /// Obtém ou define o código do status.
        /// </summary>
        public string tpStatus { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string Text { get; set; }
    }
}
