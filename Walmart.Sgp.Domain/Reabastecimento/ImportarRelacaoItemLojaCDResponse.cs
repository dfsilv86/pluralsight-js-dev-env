using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Reposta do processo de importação de vinculo/desvinculo RelacaoItemLojaCD.
    /// </summary>
    public class ImportarRelacaoItemLojaCDResponse
    {
        /// <summary>
        /// Obtém um valor que indica se a operação foi executada com sucesso.
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de retorno.
        /// </summary>
        public string Mensagem { get; set; }
    }
}
