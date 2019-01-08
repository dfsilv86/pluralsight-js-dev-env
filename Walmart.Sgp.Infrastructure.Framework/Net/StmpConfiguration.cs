namespace Walmart.Sgp.Infrastructure.Framework.Net
{
    /// <summary>
    /// Configuração do serviço de envio de e-mails SMTP.
    /// </summary>
    public class SmtpConfiguration
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o endereço do servidor SMTP.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Obtém ou define a porta do servidor SMTP.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Obtém ou define o usuário.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Obtém ou define a senha.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Obtém ou define o domínio do usuário.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Obtém ou define o remetente padrão.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Obtém ou define o nome de exibição do remetente padrão.
        /// </summary>
        public string FromDisplayName { get; set; } 
        #endregion
    }
}