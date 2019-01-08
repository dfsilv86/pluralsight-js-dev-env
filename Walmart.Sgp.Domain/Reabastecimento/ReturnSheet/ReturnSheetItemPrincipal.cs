using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma ReturnSheetItemPrincipal.
    /// </summary>
    public class ReturnSheetItemPrincipal : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou defini Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IdReturnSheetItemPrincipal;
            }

            set
            {
                IdReturnSheetItemPrincipal = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdReturnSheetItemPrincipal.
        /// </summary>
        public int IdReturnSheetItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define IdReturnSheet.
        /// </summary>
        public int IdReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define IdItemDetalhe.
        /// </summary>
        public long IdItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalhe.
        /// </summary>
        public Walmart.Sgp.Domain.Item.ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define ReturnSheet.
        /// </summary>
        public ReturnSheet ReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define Lojas.
        /// </summary>
        public int Lojas { get; set; }
    }
}
