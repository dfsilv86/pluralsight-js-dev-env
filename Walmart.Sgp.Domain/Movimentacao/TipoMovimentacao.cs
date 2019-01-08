using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma tipo de movimentação.
    /// </summary>
    public class TipoMovimentacao : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDTipoMovimentacao;
            }

            set
            {
                IDTipoMovimentacao = value;
            }
        }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public int IDTipoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string dsTipoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define se deve apurar custo médio.
        /// </summary>
        public bool? bitApurarCustoMedio { get; set; }

        /// <summary>
        /// Obtém ou define a ordem.
        /// </summary>
        public int? Ordem { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de movimento.
        /// </summary>
        public TipoMovimento TipoMovimento { get; set; }
    }
}
