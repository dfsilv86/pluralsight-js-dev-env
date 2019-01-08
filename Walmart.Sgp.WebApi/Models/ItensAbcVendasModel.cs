using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de abc vendas.
    /// </summary>
    public class ItensAbcVendasModel
    {
        /// <summary>
        /// Obtém ou define a Loja.
        /// </summary>
        public string Loja { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o DeptoCat.
        /// </summary>
        public string DeptoCat { get; set; }

        /// <summary>
        /// Obtém ou define o lblDeptoCat.
        /// </summary>
        public string LblDeptoCat { get; set; }

        /// <summary>
        /// Obtém ou define o IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o Periodo.
        /// </summary>
        public RangeValue<DateTime> Periodo { get; set; }
    }
}