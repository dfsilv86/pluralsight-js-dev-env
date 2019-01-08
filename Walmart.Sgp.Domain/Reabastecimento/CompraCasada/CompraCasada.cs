using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento.CompraCasada
{
    /// <summary>
    /// Representa uma CompraCasada.
    /// </summary>
    public class CompraCasada : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDCompraCasada;
            }

            set
            {
                IDCompraCasada = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDCompraCasada.
        /// </summary>
        public int IDCompraCasada { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalheSaida.
        /// </summary>
        public long IDItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedorParametro.
        /// </summary>
        public long IDFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalheEntrada.
        /// </summary>
        public long? IDItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define blItemPai.
        /// </summary>
        public bool? blItemPai { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }
    }
}
