using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.Net
{
    /// <summary>
    /// Representa o serviço de envio de e-mails utilizando protocolo SMTP.
    /// </summary>
    public class SmtpService : IMailService
    {
        #region Fields
        private readonly SmtpConfiguration m_configuration;
        private readonly IMailClient m_client;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SmtpService" />
        /// </summary>
        /// <param name="configuration">A configuração do SMTP.</param>
        /// <param name="client">O client SMTP.</param>
        public SmtpService(SmtpConfiguration configuration, IMailClient client)
        {
            m_configuration = configuration;
            m_client = GetClient(configuration, client);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Envia um e-mail para todos os destinatários.
        /// </summary>
        /// <param name="from">O remetente (opcional, utiliza remetente padrão).</param>
        /// <param name="mailTo">Os destinatários.</param>
        /// <param name="subject">O assunto do e-mail (opcional).</param>
        /// <param name="body">O corpo do e-mail (opcional).</param>
        /// <param name="attachments">Os anexos (opcional).</param>
        public void Send(MailAddress from, IEnumerable<MailAddress> mailTo, string subject, string body, IEnumerable<Attachment> attachments)
        {
            ExceptionHelper.ThrowIfNull("to", mailTo);

            var msg = PopulateMessage(from, subject, body);

            foreach (var mail in mailTo)
            {
                msg.To.Add(mail);
            }

            PopulateAttachments(attachments, msg);

            m_client.Send(msg);
        }

        /// <summary>
        /// Envia um e-mail para todos os destinatários utilizando o remetente padrão.
        /// </summary>
        /// <param name="mailTo">Os destinatários.</param>
        /// <param name="subject">O assunto do e-mail (opcional).</param>
        /// <param name="body">O corpo do e-mail (opcional).</param>
        /// <param name="attachments">Os anexos (opcional).</param>
        public void Send(IEnumerable<MailAddress> mailTo, string subject, string body, IEnumerable<Attachment> attachments)
        {
            Send(null, mailTo, subject, body, attachments);
        }
        
        private static void PopulateAttachments(IEnumerable<Attachment> attachments, MailMessage msg)
        {
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    msg.Attachments.Add(attachment);
                }
            }
        }

        private static IMailClient GetClient(SmtpConfiguration configuration, IMailClient smtpClient)
        {
            smtpClient.Port = configuration.Port;
            smtpClient.Host = configuration.Server;

            if (UseCredentials(configuration))
            {
                smtpClient.Credentials = new NetworkCredential(configuration.User, configuration.Password, configuration.Domain);
            }

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;

            return smtpClient;
        }

        private static bool UseCredentials(SmtpConfiguration configuration)
        {
            return !string.IsNullOrWhiteSpace(configuration.User)
                && !string.IsNullOrWhiteSpace(configuration.Password)
                && !string.IsNullOrWhiteSpace(configuration.Domain);
        }

        private MailMessage PopulateMessage(MailAddress from, string subject, string body)
        {
            var msg = new MailMessage();
            msg.From = from ?? new MailAddress(m_configuration.FromAddress, m_configuration.FromDisplayName);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            return msg;
        }
        #endregion
    }
}