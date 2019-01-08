using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma ReturnSheetItemLoja.
    /// </summary>
    public class ReturnSheetItemLoja : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IdReturnSheetItemLoja;
            }

            set
            {
                IdReturnSheetItemLoja = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdReturnSheetItemLoja.
        /// </summary>
        public int IdReturnSheetItemLoja { get; set; }

        /// <summary>
        /// Obtém ou define IdReturnSheetItemPrincipal.
        /// </summary>
        public int IdReturnSheetItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define IdItemDetalhe.
        /// </summary>
        public long IdItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IdLoja.
        /// </summary>
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define PrecoVenda.
        /// </summary>
        public decimal? PrecoVenda { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        #region Propriedades Calculadas
        /// <summary>
        /// Obtém ou define IdItemDetalheSaida.
        /// </summary>
        public long IdItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define IdItemDetalheEntrada.
        /// </summary>
        public long IdItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define cdLoja.
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define nmLoja.
        /// </summary>
        public string nmLoja { get; set; }

        /// <summary>
        /// Obtém ou define dsEstado.
        /// </summary>
        public string dsEstado { get; set; }

        /// <summary>
        /// Obtém ou define cdCD.
        /// </summary>
        public int cdCD { get; set; }

        /// <summary>
        /// Obtém ou define valido.
        /// </summary>
        public bool valido { get; set; }

        /// <summary>
        /// Obtém ou define selecionado.
        /// </summary>
        public bool selecionado { get; set; }
        #endregion

        /// <summary>
        /// Obtém ou define ItemPrincipal.
        /// </summary>
        public ReturnSheetItemPrincipal ItemPrincipal { get; set; }
    }
}
