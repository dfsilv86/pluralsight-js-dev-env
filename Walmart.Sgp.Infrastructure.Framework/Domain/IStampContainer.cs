using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de um container de carimbo de criação e alteração.
    /// </summary>
    public interface IStampContainer : IEntity
    {
        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        int? CdUsuarioAtualizacao { get; set; }
    }
}
