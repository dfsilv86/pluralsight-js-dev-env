using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representa um papel do usuário.
    /// </summary>
    public class Papel : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id externo do papel no WebGuardian.
        /// </summary>
        public int IdExterno { get; set; }

        /// <summary>
        /// Obtém ou define o nome.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o perfil é administrador.
        /// </summary>
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o perfil é GA
        /// </summary>
        public bool? IsGa { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o perfil é HO
        /// </summary>
        public bool? IsHo { get; set; }

        /// <summary>
        /// Obtém o id da aplicação.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int IdApplication
        {
            get
            {
                // TODO: remover quando a tabela CWIApplication for removida do banco.
                return 1;
            }
        }
    }
}
