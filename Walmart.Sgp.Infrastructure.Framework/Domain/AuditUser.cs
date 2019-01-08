using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Representa um usuário na auditoria.
    /// </summary>
    public class AuditUser : EntityBase
    {
        /// <summary>
        /// Obtém ou define o nome de usuário.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define o nome completo.
        /// </summary>
        public string FullName { get; set; }
    }
}
