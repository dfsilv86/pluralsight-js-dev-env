using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa uma ReabastecimentoFornecedorCD.
    /// </summary>
    public class ReabastecimentoFornecedorCD : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IdReabastecimentoFornecedorCD;
            }

            set
            {
                IdReabastecimentoFornecedorCD = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdReabastecimentoFornecedorCD.
        /// </summary>
        public int IdReabastecimentoFornecedorCD { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedorParametro.
        /// </summary>
        public long IDFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define IDCD.
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtém ou define TipoReabastecimento.
        /// </summary>
        public int TipoReabastecimento { get; set; }
    }
}
