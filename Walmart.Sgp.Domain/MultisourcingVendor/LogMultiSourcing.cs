using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Representa uma LogMultiSourcing.
    /// </summary>
    public class LogMultiSourcing : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o Id do log.
        /// </summary>
        public int IdLogMultiSourcing { get; set; }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdLogMultiSourcing;
            }

            set
            {
                this.IdLogMultiSourcing = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdCd.
        /// </summary>
        public int IdCd { get; set; }

        /// <summary>
        /// Obtém ou define IdItemDetalheSaida.
        /// </summary>
        public long IdItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define IdItemDetalheEntrada.
        /// </summary>
        public long IdItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define IdFornecedorParametro.
        /// </summary>
        public long IdFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define Data.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Obtém ou define IdUsuario.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Obtém ou define PercAnterior.
        /// </summary>
        public decimal PercAnterior { get; set; }

        /// <summary>
        /// Obtém ou define PercPosterior.
        /// </summary>
        public decimal PercPosterior { get; set; }

        /// <summary>
        /// Obtém ou define TpOperacao.
        /// </summary>
        public string TpOperacao { get; set; }

        /// <summary>
        /// Obtém ou define Obervacao.
        /// </summary>
        public string Observacao { get; set; }
    }
}
