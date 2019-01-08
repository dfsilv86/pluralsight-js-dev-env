using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Representa uma Multisourcing.
    /// </summary>
    public class Multisourcing : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Multisourcing"/>.
        /// </summary>
        public Multisourcing()
        {
            NotSatisfiedSpecReasons = new List<string>(); 
        }

        /// <summary>
        /// Obtém ou define IDMultisourcing.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDMultisourcing;
            }

            set
            {
                IDMultisourcing = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDMultisourcing.
        /// </summary>
        public int IDMultisourcing { get; set; }

        /// <summary>
        /// Obtém ou define IDCD.
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemSecundario.
        /// </summary>
        public long IDRelacionamentoItemSecundario { get; set; }

        /// <summary>
        /// Obtém ou define vlPercentual.
        /// </summary>
        public decimal vlPercentual { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioInclusao.
        /// </summary>
        public int cdUsuarioInclusao { get; set; }

        /// <summary>
        /// Obtém ou define dtInclusao.
        /// </summary>
        public DateTime dtInclusao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAlteracao.
        /// </summary>
        public int cdUsuarioAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define dtAlteracao.
        /// </summary>
        public DateTime dtAlteracao { get; set; }

        #region Importação
        /// <summary>
        /// Obtém ou define NotSatisfiedSpecException
        /// </summary>
        public IList<string> NotSatisfiedSpecReasons { get; set; }

        /// <summary>
        /// Obtém ou define RowIndex
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Obtém ou define CdItemDetalheSaida.
        /// </summary>
        public long CdItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define CdItemDetalheEntrada.
        /// </summary>
        public long CdItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define Vendor9Digitos.
        /// </summary>
        public long Vendor9Digitos { get; set; }

        /// <summary>
        /// Obtém ou define CD.
        /// </summary>
        public int CD { get; set; }

        /// <summary>
        /// Retornar true caso o Multisourcing esteja válido.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return NotSatisfiedSpecReasons.Count == 0;
            }
        }

        /// <summary>
        /// Obtém ou define ItemDetalheSaida.
        /// </summary>
        public ItemDetalhe ItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalheEntrada.
        /// </summary>
        public ItemDetalhe ItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define Fornecedor.
        /// </summary>
        public Fornecedor Fornecedor { get; set; }
        #endregion
    }
}
