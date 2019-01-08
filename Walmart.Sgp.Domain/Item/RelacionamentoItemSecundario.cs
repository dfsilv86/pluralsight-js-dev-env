using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa uma RelacionamentoItemSecundario.
    /// </summary>
    public class RelacionamentoItemSecundario : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRelacionamentoItemSecundario;
            }

            set
            {
                IDRelacionamentoItemSecundario = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemSecundario.
        /// </summary>
        public int IDRelacionamentoItemSecundario { get; set; }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemPrincipal.
        /// </summary>
        public int IDRelacionamentoItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define psItem.
        /// </summary>
        public decimal psItem { get; set; }

        /// <summary>
        /// Obtém ou define tipo de item de entrada.
        /// </summary>
        public TipoItemEntrada TpItem { get; set; }

        /// <summary>
        /// Obtém ou define pcRendimentoDerivado.
        /// </summary>
        public float? pcRendimentoDerivado { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int? IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o item detalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define qtItemUn.
        /// </summary>
        public decimal? qtItemUn { get; set; }
    }
}
