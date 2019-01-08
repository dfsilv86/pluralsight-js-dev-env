using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma NotaFiscalItem.
    /// </summary>
    public class NotaFiscalItem : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDNotaFiscalItem;
            }

            set
            {
                IDNotaFiscalItem = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDNotaFiscalItem.
        /// </summary>
        public int IDNotaFiscalItem { get; set; }

        /// <summary>
        /// Obtém ou define IDNotaFiscal.
        /// </summary>
        public int IDNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define qtItem.
        /// </summary>
        public decimal qtItem { get; set; }

        /// <summary>
        /// Obtém ou define vlMercadoria.
        /// </summary>
        public decimal vlMercadoria { get; set; }

        /// <summary>
        /// Obtém ou define vlCusto.
        /// </summary>
        public decimal vlCusto { get; set; }

        /// <summary>
        /// Obtém vlTotal.
        /// </summary>
        public decimal vlTotal 
        { 
            get
            {
                return qtItem * vlCusto;
            }
        }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? dhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define blDivergente.
        /// </summary>
        public bool blDivergente { get; set; }

        /// <summary>
        /// Obtém ou define tpStatus.
        /// </summary>
        public string tpStatus { get; set; }

        /// <summary>
        /// Obtém ou define dtLiberacao.
        /// </summary>
        public DateTime? dtLiberacao { get; set; }

        /// <summary>
        /// Obtém ou define blLiberado.
        /// </summary>
        public bool? blLiberado { get; set; }

        /// <summary>
        /// Obtém ou define qtItemAnterior.
        /// </summary>
        public decimal? qtItemAnterior { get; set; }

        /// <summary>
        /// Obtém ou define nrLinha.
        /// </summary>
        public short? nrLinha { get; set; }

        /// <summary>
        /// Obtém ou define VariacaoUltimoCusto.
        /// </summary>
        public decimal? VariacaoUltimoCusto { get; set; }

        /// <summary>
        /// Obtém ou define IdNotaFiscalItemStatus.
        /// </summary>
        public short? IdNotaFiscalItemStatus { get; set; }

        /// <summary>
        /// Obtém ou define o item.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define NotaFiscalItemStatus.
        /// </summary>
        public NotaFiscalItemStatus Status { get; set; }
    }
}
