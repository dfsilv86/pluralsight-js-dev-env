using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Opções do Single Sign On.
    /// </summary>
    public class SsoOptions
    {
        /// <summary>
        /// Obtém ou define o nome do usuário.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define a senha do usuário.
        /// </summary>
        public string UserPassword { get; set; }
   
        /// <summary>
        /// Obtém ou define o código da aplicação.
        /// </summary>
        public int ApplicationCode { get; set; }

        /// <summary>
        /// Obtém ou define o código do papel.
        /// </summary>
        public int? ProfileCode { get; set; }

        /// <summary>
        /// Obtém ou define o domínio do e-mail.
        /// </summary>
        public string EmailDomain { get; set; }
    }
}
