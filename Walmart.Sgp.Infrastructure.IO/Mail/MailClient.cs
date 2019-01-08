using System.Net;
using System.Net.Mail;
using Walmart.Sgp.Infrastructure.Framework.Net;

namespace Walmart.Sgp.Infrastructure.IO.Mail
{
    /// <summary>
    /// Classe que define o client de email.
    /// </summary>
    public class MailClient : IMailClient
    {
        /// <summary>
        /// Obtém ou define Port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Obtém ou define Host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Obtém ou define Credentials.
        /// </summary>
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Obtém ou define DeliveryMethod.
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o SSL está habilitado.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Método que realiza o envio do email.
        /// </summary>
        public void Send(MailMessage msg)
        {
            using (var client = new SmtpClient(this.Host, this.Port))
            {
                client.Host = this.Host;
                client.Port = this.Port;
                client.Credentials = this.Credentials;
                client.DeliveryMethod = this.DeliveryMethod;
                client.EnableSsl = this.EnableSsl;
                
                client.Send(msg);
            }
        }
    }
}
