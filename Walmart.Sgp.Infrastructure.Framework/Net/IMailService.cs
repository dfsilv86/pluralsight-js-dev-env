using System.Collections.Generic;
using System.Net.Mail;

namespace Walmart.Sgp.Infrastructure.Framework.Net
{
    /// <summary>
    /// Define a interface de um serviço de envio de e-mails.
    /// </summary>
    public interface IMailService
    {
        #region Methods
        /// <summary>
        /// Envia um e-mail para todos os destinatários.
        /// </summary>
        /// <param name="from">O remetente (opcional, utiliza remetente padrão).</param>
        /// <param name="mailTo">Os destinatários.</param>
        /// <param name="subject">O assunto do e-mail (opcional).</param>
        /// <param name="body">O corpo do e-mail (opcional).</param>
        /// <param name="attachments">Os anexos (opcional).</param>
        void Send(MailAddress from, IEnumerable<MailAddress> mailTo, string subject, string body, IEnumerable<Attachment> attachments);

        /// <summary>
        /// Envia um e-mail para todos os destinatários utilizando o remetente padrão.
        /// </summary>
        /// <param name="mailTo">Os destinatários.</param>
        /// <param name="subject">O assunto do e-mail (opcional).</param>
        /// <param name="body">O corpo do e-mail (opcional).</param>
        /// <param name="attachments">Os anexos (opcional).</param>
        void Send(IEnumerable<MailAddress> mailTo, string subject, string body, IEnumerable<Attachment> attachments);
        #endregion
    }
}