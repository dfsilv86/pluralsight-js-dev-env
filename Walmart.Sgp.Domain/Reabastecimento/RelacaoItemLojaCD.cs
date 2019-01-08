using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma RelacaoItemLojaCD.
    /// </summary>
    public class RelacaoItemLojaCD : EntityBase, IAggregateRoot, IStampContainer
    {
        /// <summary>
        /// Obt�m ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRelacaoItemLojaCD;
            }

            set
            {
                IDRelacaoItemLojaCD = value;
            }
        }

        /// <summary>
        /// Obt�m ou define IDRelacaoItemLojaCD.
        /// </summary>
        public int IDRelacaoItemLojaCD { get; set; }

        /// <summary>
        /// Obt�m ou define IDItem.
        /// </summary>
        public long IDItem { get; set; }

        /// <summary>
        /// Obt�m ou define IDLojaCDParametro.
        /// </summary>
        public long IDLojaCDParametro { get; set; }

        /// <summary>
        /// Obt�m ou define blAtivo.
        /// </summary>
        public bool? blAtivo { get; set; }

        /// <summary>
        /// Obt�m ou define cdSistema.
        /// </summary>
        public byte cdSistema { get; set; }

        /// <summary>
        /// Obt�m ou define dhCriacao.
        /// </summary>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obt�m ou define dhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obt�m ou define cdUsuarioCriacao.
        /// </summary>
        public int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obt�m ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? CdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obt�m ou define IdItemEntrada.
        /// </summary>
        public long? IdItemEntrada { get; set; }

        /// <summary>
        /// Obtem ou define o tipo do reab
        /// </summary>
        public int? VlTipoReabastecimento { get; set; }

        /// <summary>
        /// Obtem ou define o cod de cross ref
        /// </summary>
        public int? CdCrossRef { get; set; }

        /// <summary>
        /// Obt�m ou define Item.
        /// </summary>
        public ItemDetalhe Item { get; set; }

        /// <summary>
        /// Obt�m ou define ItemEntrada.
        /// </summary>
        public ItemDetalhe ItemEntrada { get; set; }

        /// <summary>
        /// Obt�m ou define LojaCdParametro.
        /// </summary>
        public LojaCdParametro LojaCdParametro { get; set; }

    }
}
