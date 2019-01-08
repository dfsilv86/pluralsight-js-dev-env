using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Fitro para auditoria.
    /// </summary>
    public class AuditFilter
    {
        /// <summary>
        /// Obtém ou define o id do usuário.
        /// </summary>
        public int? IdUsuario { get; set; }

        /// <summary>
        /// Obtém ou define o id da entidade.
        /// </summary>
        public int? IdEntidade { get; set; }

        /// <summary>
        /// Obtém ou define o intervalo de data.
        /// </summary>
        public RangeValue<DateTime> Intervalo { get; set; }
    }
}
