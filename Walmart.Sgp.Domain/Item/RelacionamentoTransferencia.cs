using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa um RelacionamentoTransferencia.
    /// </summary>
    public class RelacionamentoTransferencia : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRelacionamentoTransferencia;
            }

            set
            {
                IDRelacionamentoTransferencia = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRelacionamentoTransferencia.
        /// </summary>
        public int IDRelacionamentoTransferencia { get; set; }
        
        /// <summary>
        /// Obtém ou define IDItemDetalheOrigem.
        /// </summary>
        public long IDItemDetalheOrigem { get; set; }
        
        /// <summary>
        /// Obtém ou define IDItemDetalheDestino.
        /// </summary>
        public long IDItemDetalheDestino { get; set; }
        
        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }
        
        /// <summary>
        /// Obtém ou define dtCriacao.
        /// </summary>
        public DateTime dtCriacao { get; set; }
        
        /// <summary>
        /// Obtém ou define IDUsuario.
        /// </summary>
        public int IDUsuario { get; set; }
        
        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool? blAtivo { get; set; }
        
        /// <summary>
        /// Obtém ou define dtInativo.
        /// </summary>
        public DateTime? dtInativo { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalheOrigem.
        /// </summary>
        public ItemDetalhe ItemDetalheOrigem { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalheDestino.
        /// </summary>
        public ItemDetalhe ItemDetalheDestino { get; set; }

        /// <summary>
        /// Obtém ou define Loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define Usuario.
        /// </summary>
        public Usuario Usuario { get; set; }
        #endregion
    }
}
