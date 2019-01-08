using System;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma RelacaoItemCD.
    /// </summary>
    public class RelacaoItemCD : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)IDRelacaoItemCD;
            }

            set
            {
                IDRelacaoItemCD = value;
            }
        }

        /// <summary>
        /// Obtém ou define idRelacaoItemCD.
        /// </summary>
        public long IDRelacaoItemCD { get; set; }

        /// <summary>
        /// Obtém ou define idItemEntrada.
        /// </summary>
        public long idItemEntrada { get; set; }

        /// <summary>
        /// Obtém ou define idItemSaida.
        /// </summary>
        public long? idItemSaida { get; set; }

        /// <summary>
        /// Obtém ou define idCD.
        /// </summary>
        public int idCD { get; set; }

        /// <summary>
        /// Obtém ou define vlTipoReabastecimento.
        /// </summary>
        public int vlTipoReabastecimento { get; set; }

        /// <summary>
        /// Obtém ou define vlEstoqueSeguranca.
        /// </summary>
        public int? vlEstoqueSeguranca { get; set; }
    }
}