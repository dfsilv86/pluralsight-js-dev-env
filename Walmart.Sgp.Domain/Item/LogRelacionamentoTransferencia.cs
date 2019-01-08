using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa um LogRelacionamentoTransferencia.
    /// </summary>
    public class LogRelacionamentoTransferencia : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDLogRelacionamentoTransferencia;
            }

            set
            {
                IDLogRelacionamentoTransferencia = value;
            }
        }

        /// <summary>
        /// Obtém ou define o IDLogRelacionamentoTransferencia.
        /// </summary>
        public int IDLogRelacionamentoTransferencia { get; set; }

        /// <summary>
        /// Obtém ou define o IDItemDetalheOrigem.
        /// </summary>
        public long IDItemDetalheOrigem { get; set; }

        /// <summary>
        /// Obtém ou define o IDItemDetalheDestino.
        /// </summary>
        public long IDItemDetalheDestino { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o dtCriacao.
        /// </summary>
        public DateTime dtCriacao { get; set; }

        /// <summary>
        /// Obtém ou define o IDUsuario.
        /// </summary>
        public int IDUsuario { get; set; }

        /// <summary>
        /// Obtém ou define o tpOperacao.
        /// </summary>
        public string tpOperacao { get; set; }
        #endregion
    }
}
