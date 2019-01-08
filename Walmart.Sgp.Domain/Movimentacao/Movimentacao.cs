using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma movimentação.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class Movimentacao : EntityBase
    {
        /// <summary>
        /// Obtém ou define id..
        /// </summary>
        public override int Id
        {
            get
            {
                return IDMovimentacao;
            }

            set
            {
                IDMovimentacao = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDMovimentacao.
        /// </summary>
        public int IDMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define IDNotaFiscal.
        /// </summary>
        public int? IDNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDItem.
        /// </summary>
        public int IDItem { get; set; }

        /// <summary>
        /// Obtém ou define o item.
        /// </summary>
        public ItemDetalhe Item { get; set; }

        /// <summary>
        /// Obtém ou define IDTipoNotaFiscal.
        /// </summary>
        public int? IDTipoNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define IDTipoMovimentacao.
        /// </summary>
        public int? IDTipoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define qtdMovimentacao.
        /// </summary>
        public decimal qtdMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define IDMotivoMovimentacao.
        /// </summary>
        public int? IDMotivoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define o motivo da movimentação.
        /// </summary>
        public MotivoMovimentacao MotivoMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define dtCriacao.
        /// </summary>
        public DateTime dtCriacao { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioCriacao.
        /// </summary>
        public int? IDUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dtMovimentado.
        /// </summary>
        public DateTime? dtMovimentado { get; set; }

        /// <summary>
        /// Obtém ou define IdItemTransferencia.
        /// </summary>
        public int? IdItemTransferencia { get; set; }

        /// <summary>
        /// Obtém ou define o item transferência.
        /// </summary>
        public ItemDetalhe ItemTransferencia { get; set; }

        /// <summary>
        /// Obtém ou define IdItemOperacao.
        /// </summary>
        public int? IdItemOperacao { get; set; }

        /// <summary>
        /// Obtém ou define IdInventario.
        /// </summary>
        public int? IdInventario { get; set; }

        /// <summary>
        /// Obtém ou define IdNotaFiscalItem.
        /// </summary>
        public int? IdNotaFiscalItem { get; set; }

        /// <summary>
        /// Obtém ou define IdTipoProcesso.
        /// </summary>
        public int? IdTipoProcesso { get; set; }

    }

}
