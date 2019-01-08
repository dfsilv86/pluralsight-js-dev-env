using System;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma LogMensagemReturnSheetVigente.
    /// </summary>
    public class LogMensagemReturnSheetVigente : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define IDLogMensagemReturnSheetVigente.
        /// </summary>
        public int IDLogMensagemReturnSheetVigente { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuario.
        /// </summary>
        public int IDUsuario { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }
    }
}