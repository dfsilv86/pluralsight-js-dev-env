using System;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma RoteiroPedido.
    /// </summary>
    public class RoteiroPedido : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRoteiroPedido;
            }

            set
            {
                IDRoteiroPedido = value;
            }
        }

        /// <summary>
        /// Obtém ou define idRoteiroPedido.
        /// </summary>
        public int IDRoteiroPedido { get; set; }

        /// <summary>
        /// Obtém ou define idRoteiro.
        /// </summary>
        public int idRoteiro { get; set; }

        /// <summary>
        /// Obtém ou define idSugestaoPedido.
        /// </summary>
        public long idSugestaoPedido { get; set; }

        /// <summary>
        /// Obtém ou define blAutorizado.
        /// </summary>
        public bool blAutorizado { get; set; }

        /// <summary>
        /// Obtém ou define idUsuarioAutorizacao
        /// </summary>
        public int? idUsuarioAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAutorizacao
        /// </summary>
        public DateTime? dhAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define Usuario
        /// </summary>
        public Usuario Usuario { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalhe
        /// </summary>
        public Walmart.Sgp.Domain.Item.ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define QtdSolicitada
        /// </summary>
        public int QtdSolicitada { get; set; }

        /// <summary>
        /// Obtém ou define QtdRoteiroRA
        /// </summary>
        public int QtdRoteiroRA { get; set; }
    }
}
