using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa uma InventarioCritica.
    /// </summary>
    public class InventarioCritica : LojaSecaoContainer, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define IDInventarioCritica.
        /// </summary>
        public int IDInventarioCritica { get; set; }

        /// <summary>
        /// Obtém ou define IDInventario.
        /// </summary>
        public int? IDInventario { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int? IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define IDInventarioCriticaTipo.
        /// </summary>
        public int IDInventarioCriticaTipo { get; set; }

        /// <summary>
        /// Obtém ou define o tipo.
        /// </summary>
        public InventarioCriticaTipo Tipo { get; set; }

        /// <summary>
        /// Obtém ou define dsCritica.
        /// </summary>
        public string dsCritica { get; set; }

        /// <summary>
        /// Obtém ou define dhInclusao.
        /// </summary>
        public DateTime dhInclusao { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define dhInventario.
        /// </summary>
        public DateTime? dhInventario { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool? blAtivo { get; set; }

    }

}
