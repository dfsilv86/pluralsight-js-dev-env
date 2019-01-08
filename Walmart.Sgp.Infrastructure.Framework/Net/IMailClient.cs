using System.Net;
using System.Net.Mail;

namespace Walmart.Sgp.Infrastructure.Framework.Net
{
    /// <summary>
    /// Interface que define o client de email.
    /// </summary>
    public interface IMailClient
    {
        /// <summary>
        /// Obtém ou define Port.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Obtém ou define Host.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Obtém ou define Credentials.
        /// </summary>
        NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Obtém ou define DeliveryMethod.
        /// </summary>
        SmtpDeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o SSL está habilitado.
        /// </summary>
        bool EnableSsl { get; set; }

        /// <summary>
        /// Método que realiza o envio do email.
        /// </summary>
        void Send(MailMessage msg);
    }
}
