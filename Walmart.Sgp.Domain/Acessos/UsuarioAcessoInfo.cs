using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representa informações de acesso de um usuário.
    /// </summary>
    public class UsuarioAcessoInfo
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o usuário.
        /// </summary>
        public Usuario Usuario { get; set; }

        /// <summary>
        /// Obtém ou define o papel.
        /// </summary>
        public Papel Papel { get; set; }    
        #endregion  
    }
}
