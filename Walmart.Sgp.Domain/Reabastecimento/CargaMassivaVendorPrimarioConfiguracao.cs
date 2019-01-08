using System.Collections.Generic;
using System.Net.Mail;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa a configuração de uma importação de vinculo/desvinculo.
    /// </summary>
    public class CargaMassivaVendorPrimarioConfiguracao
    {
        /// <summary>
        /// Lista de e-mails que serão notificados após finalização da importação.
        /// </summary>
        public IEnumerable<MailAddress> EmailsRetornoImportacao { get; set; }

        /// <summary>
        /// Obtém ou define Desvinculando.
        /// </summary>
        public bool Desvinculando { get; set; }
    }
}
