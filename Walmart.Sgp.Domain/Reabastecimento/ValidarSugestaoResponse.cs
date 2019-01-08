using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Informações sobre a validação de uma sugestão de pedido.
    /// </summary>
    public class ValidarSugestaoResponse
    {
        /// <summary>
        /// Obtém ou define um valor que indica se a sugestão é válida.
        /// </summary>
        public bool SugestaoValida { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de validação.
        /// </summary>
        public string Mensagem { get; set; }
    }
}
