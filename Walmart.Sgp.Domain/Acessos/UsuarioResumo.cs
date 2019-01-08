using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Informações básicas sobre um usuário.
    /// </summary>
    [DebuggerDisplay("{FullName} ({UserName})")]
    public class UsuarioResumo : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o nome de usuário (login).
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define o nome completo do usuário.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Obtém ou define o email.
        /// </summary>
        public string Email { get; set; }
    }
}
