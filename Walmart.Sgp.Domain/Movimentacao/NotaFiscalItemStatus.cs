using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma NotaFiscalItemStatus.
    /// </summary>
    public class NotaFiscalItemStatus : EntityBase, IAggregateRoot
    {
        #region Constants
        /// <summary>
        /// O id do registro de conforme.
        /// </summary>
        /// //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IdConforme = 1;

        /// <summary>
        /// O id do registro de alterado.
        /// </summary>
        /// //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IdAlterado = 2;

        /// <summary>
        /// O id do registro de pendente.
        /// </summary>
        /// //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IdPendente = 3;

        /// <summary>
        /// O id do registro de revisado.
        /// </summary>
        /// //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IdRevisado = 4;
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDNotaFiscalItemStatus;
            }

            set
            {
                IDNotaFiscalItemStatus = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdNotaFiscalItemStatus.
        /// </summary>
        public int IDNotaFiscalItemStatus { get; set; }

        /// <summary>
        /// Obtém ou define DsNotaFiscalItemStatus.
        /// </summary>
        public string DsNotaFiscalItemStatus { get; set; }

        /// <summary>
        /// Obtém ou define SiglaNotaFiscalItemStatus.
        /// </summary>
        public string SiglaNotaFiscalItemStatus { get; set; }
        #endregion
    }
}