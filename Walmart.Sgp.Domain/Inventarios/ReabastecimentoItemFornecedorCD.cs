using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa uma ReabastecimentoItemFornecedorCD.
    /// </summary>
    public class ReabastecimentoItemFornecedorCD : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IdReabastecimentoItemFornecedorCD;
            }

            set
            {
                IdReabastecimentoItemFornecedorCD = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdReabastecimentoItemFornecedorCD.
        /// </summary>
        public int IdReabastecimentoItemFornecedorCD { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public long IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IDCD.
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtém ou define TipoReabastecimento.
        /// </summary>
        public int TipoReabastecimento { get; set; }

        /// <summary>
        /// Obtém ou define EstoqueSeguranca.
        /// </summary>
        public int EstoqueSeguranca { get; set; }
    }
}
