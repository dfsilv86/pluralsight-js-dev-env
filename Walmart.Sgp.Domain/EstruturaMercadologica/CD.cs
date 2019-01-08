using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa um CD (Centro de Distribuição)..
    /// </summary>
    [DebuggerDisplay("{cdCD} - {nmNome}")]
    public class CD : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDCD;
            }

            set
            {
                IDCD = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDCD.
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtém ou define cdCD.
        /// </summary>
        public int cdCD { get; set; }

        /// <summary>
        /// Obtém ou define nmNome.
        /// </summary>
        public string nmNome { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define stCD.
        /// </summary>
        public string stCD { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }

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
        /// Obtém ou define cdSistema.
        /// </summary>
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define dsUF.
        /// </summary>
        public string dsUF { get; set; }

        /// <summary>
        /// Obtém ou define blConvertido.
        /// </summary>
        public bool? blConvertido { get; set; }

    }
}
