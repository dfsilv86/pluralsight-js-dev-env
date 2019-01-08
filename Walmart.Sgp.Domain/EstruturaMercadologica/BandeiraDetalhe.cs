using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma BandeiraDetalhe.
    /// </summary>
    [DebuggerDisplay("{Id}: IDDepartamento: {IDDepartamento} | IDCategoria: {IDCategoria}")]
    public class BandeiraDetalhe : LojaSecaoContainer
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDBandeiraDetalhe;
            }

            set
            {
                IDBandeiraDetalhe = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDBandeiraDetalhe.
        /// </summary>
        public int IDBandeiraDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }       

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }            
    }

}
