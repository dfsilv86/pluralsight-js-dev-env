using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Classe que serve de modelo para geração de vinculos massivos.
    /// </summary>
    public class RelacaoItemLojaCDVinculo
    {
        /// <summary>
        /// Obtém ou define RowIndex
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Obtém ou define NotSatisfiedSpecException
        /// </summary>
        public IList<string> NotSatisfiedSpecReasons { get; set; }

        /// <summary>
        /// Obtém ou define CdItemDetalheSaida
        /// </summary>
        public long CdItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define CdItemDetalheEntrada
        /// </summary>
        public long CdItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define CdCD
        /// </summary>
        public long CdCD { get; set; }

        /// <summary>
        /// Obtém ou define CdLoja
        /// </summary>
        public long CdLoja { get; set; }

        /// <summary>
        /// Retornar true caso o Vinculo esteja válido.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return NotSatisfiedSpecReasons.Count == 0;
            }
        }
    }
}
