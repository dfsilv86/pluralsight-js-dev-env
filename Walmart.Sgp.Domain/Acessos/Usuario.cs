using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representa um usuário da aplicação.
    /// </summary>
    /// <remarks>
    /// UserName é o login do usuário; FullName é o nome (label) do usuário.
    /// </remarks>
    [DebuggerDisplay("{FullName} ({UserName})")]
    public class Usuario : EntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Usuario"/>
        /// </summary>
        public Usuario() 
            : this(0)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Usuario"/>.
        /// </summary>
        /// <param name="id">O id.</param>
        public Usuario(int id)
            : base(id)
        {            
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o id da aplicação.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int IdApplication
        {
            get
            {
                // TODO: remover quando a tabela CWIApplication for removida do banco.
                return 1;
            }
        }

        /// <summary>
        /// Obtém ou define o nome de usuário (login).
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define o valor para Password.
        /// </summary>
        public string Passwd { get; set; }

        /// <summary>
        /// Obtém ou define o valor para PasswordQuestion.
        /// </summary>
        public string PasswordQuestion { get; set; }

        /// <summary>
        /// Obtém ou define o valor para PasswordAnswer.
        /// </summary>
        public string PasswordAnswer { get; set; }

        /// <summary>
        /// Obtém ou define o nome completo do usuário.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Obtém ou define o valor para Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Obtém ou define o valor para Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se está aprovado.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Obtém ou define o valor para LastActivityDate.
        /// </summary>
        public DateTime? LastActivityDate { get; set; }

        /// <summary>
        /// Obtém ou define o valor para LastLoginDate.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Obtém ou define o valor para LastPasswordChangedDate.
        /// </summary>
        public DateTime? LastPasswordChangedDate { get; set; }

        /// <summary>
        /// Obtém ou define o valor para CreationDate.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se está bloqueado.
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Obtém ou define o valor para LastLockoutDate.
        /// </summary>
        public DateTime? LastLockoutDate { get; set; }

        /// <summary>
        /// Obtém ou define o valor para PasswordFormat.
        /// </summary>
        public int PasswdFormat { get; set; }

        /// <summary>
        /// Obtém ou define o valor para FailedPasswdCount.
        /// </summary>
        public int? FailedPasswdCount { get; set; }

        /// <summary>
        /// Obtém ou define o valor para FailedPasswordAttemptWindowStart.
        /// </summary>
        public DateTime? FailedPasswordAttemptWindowStart { get; set; }

        /// <summary>
        /// Obtém ou define o valor para FailedPasswordAnswerAttemptCount.
        /// </summary>
        public int? FailedPasswordAnswerAttemptCount { get; set; }

        /// <summary>
        /// Obtém ou define o valor para FailedPasswordAnswerAttemptWindowStart.
        /// </summary>
        public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
        #endregion
    }
}
