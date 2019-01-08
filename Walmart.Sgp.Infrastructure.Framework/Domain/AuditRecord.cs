using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Registro de log de auditoria.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade logada.</typeparam>
    [DebuggerDisplay("Log (atual? {blAtual}) id {IdAuditRecord} oper {CdAuditKind.Description} por user {IdAuditUser} em {DhAuditStamp} - {Entity}")]
    public class AuditRecord<TEntity> : EntityBase
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AuditRecord{TEntity}"/>.
        /// </summary>
        /// <param name="audited">A entidade logada.</param>
        /// <param name="auditKind">Tipo de log.</param>
        public AuditRecord(TEntity audited, AuditKind auditKind)
        {
            ExceptionHelper.ThrowIfNull("auditKind", auditKind);

            this.Entity = audited;

            this.IdAuditUser = RuntimeContext.Current.User.Id;

            this.DhAuditStamp = DateTime.Now;

            this.CdAuditKind = auditKind.Value;
        }

        /// <summary>
        /// Evita que uma instância default da classe <see cref="AuditRecord{TEntity}" /> seja criada.
        /// </summary>
        private AuditRecord()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém ou define o id do registro de log.
        /// </summary>
        public int IdAuditRecord { get; set; }

        /// <summary>
        /// Obtém um valor que indica se este registro representa os dados presentes atualmente na tabela da entidade.
        /// </summary>
        /// <remarks>Caso false, representa os dados presentes na tabela de log, mesmo que estes sejam iguais ao que está na tabela da entidade.</remarks>
        public bool BlAtual { get; private set; }

        /// <summary>
        /// Obtém ou define o id do registro de log.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdAuditRecord;
            }

            set
            {
                this.IdAuditRecord = value;
            }
        }

        /// <summary>
        /// Obtém o id do usuário que efetuou a operação.
        /// </summary>
        public int IdAuditUser { get; private set; }

        /// <summary>
        /// Obtém ou define o usuário que efetuou a operaççao.
        /// </summary>
        public AuditUser AuditUser { get; set; }

        /// <summary>
        /// Obtém a data e hora em que a operação foi efetuada.
        /// </summary>
        public DateTime DhAuditStamp { get; private set; }

        /// <summary>
        /// Obtém o tipo de operação efetuada.
        /// </summary>
        public AuditKind CdAuditKind { get; private set; }

        /// <summary>
        /// Obtém ou define os dados da entidade.
        /// </summary>
        public TEntity Entity { get; set; }
        #endregion
    }
}
