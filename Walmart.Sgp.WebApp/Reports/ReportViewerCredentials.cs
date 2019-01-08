using System;
using System.Net;
using Microsoft.Reporting.WebForms;

namespace Walmart.Sgp.WebApp.Reports
{
    /// <summary>
    /// Classe para credencias do ReportViewer
    /// </summary>
    public class ReportViewerCredentials : IReportServerCredentials
    {
        #region Fields
        /// <summary>
        /// set userName
        /// </summary>
        private string userName;

        /// <summary>
        /// set passWord
        /// </summary>
        private string passWord;

        /// <summary>
        /// set domain
        /// </summary>
        private string domain;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReportViewerCredentials"/>.
        /// </summary>
        /// <param name="userName">O nome do usuário</param>
        /// <param name="password">A senha do usuário</param>
        /// <param name="domain">O dominio</param>
        public ReportViewerCredentials(string userName, string password, string domain)
        {
            this.userName = userName;
            this.passWord = password;
            this.domain = domain;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ImpersonationUser
        /// </summary>
        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        /// <summary>
        /// Obtém NetworkCredentials
        /// </summary>
        public System.Net.ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(userName, passWord, domain); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Recupera credenciais
        /// </summary>
        /// <param name="authCookie">O authCookie</param>
        /// <param name="userName">O userName</param>
        /// <param name="password">O password</param>
        /// <param name="authority">O authority</param>
        /// <returns>Retorna verdadeiro ou falso</returns>
        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            return false;
        }
        #endregion
    }
}