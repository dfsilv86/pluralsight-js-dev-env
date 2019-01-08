using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma Distrito.
    /// </summary>
    public class Distrito : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define IDDistrito.
        /// </summary>
        public int IDDistrito { get; set; }

        /// <summary>
        /// Obtém ou define nmDistrito.
        /// </summary>
        public string nmDistrito { get; set; }

        /// <summary>
        /// Obtém ou define IDRegiao.
        /// </summary>
        public int IDRegiao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioResponsavelDistrito.
        /// </summary>
        public int? cdUsuarioResponsavelDistrito { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? dhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? cdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? cdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IDDistrito;
            }

            set
            {
                this.IDDistrito = value;
            }
        }

        /// <summary>
        /// Obtém ou define a região.
        /// </summary>
        public Regiao Regiao { get; set; }
    }

}
