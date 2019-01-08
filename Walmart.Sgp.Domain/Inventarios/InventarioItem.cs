using System;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa um InventarioItem.
    /// </summary>
    public class InventarioItem : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDInventarioItem;
            }

            set
            {
                IDInventarioItem = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDInventarioItem.
        /// </summary>
        public int IDInventarioItem { get; set; }

        /// <summary>
        /// Obtém ou define IDInventario.
        /// </summary>
        public int IDInventario { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int? IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o item detalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define dhUltimaContagem.
        /// </summary>
        public DateTime? dhUltimaContagem { get; set; }

        /// <summary>
        /// Obtém ou define qtItem.
        /// </summary>
        public decimal qtItem { get; set; }

        /// <summary>
        /// Obtém ou define dhAlteracao.
        /// </summary>
        public DateTime? dhAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAlteracao.
        /// </summary>
        public int? cdUsuarioAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define dhRecalculo.
        /// </summary>
        public DateTime? dhRecalculo { get; set; }

        /// <summary>
        /// Obtém ou define blJustificado.
        /// </summary>
        public bool blJustificado { get; set; }

        /// <summary>
        /// Obtém ou define dsComentario.
        /// </summary>
        public string dsComentario { get; set; }

        /// <summary>
        /// Obtém ou define qtItemInicial.
        /// </summary>
        public decimal? qtItemInicial { get; set; }

        /// <summary>
        /// Obtém ou define blAteradoGA.
        /// </summary>
        public bool blAteradoGA { get; set; }

        /// <summary>
        /// Obtém ou define QtdAuditada.
        /// </summary>
        public decimal? QtdAuditada { get; set; }

        /// <summary>
        /// Obtém ou define AcaoTomada.
        /// </summary>
        public string AcaoTomada { get; set; }

        /// <summary>
        /// Obtém ou define AuditadoPor.
        /// </summary>
        public int? AuditadoPor { get; set; }

        /// <summary>
        /// Obtém ou define dtAuditoria.
        /// </summary>
        public DateTime? dtAuditoria { get; set; }

        /// <summary>
        /// Obtém ou define qtdInventariadaAuditada.
        /// </summary>
        public decimal? qtdInventariadaAuditada { get; set; }

        /// <summary>
        /// Obtém ou define vlTotalDifAuditado.
        /// </summary>
        public decimal? vlTotalDifAuditado { get; set; }

        /// <summary>
        /// Obtém ou define o estoque relacionado a esta instância.
        /// </summary>
        public Estoque Estoque { get; set; }
    }

}