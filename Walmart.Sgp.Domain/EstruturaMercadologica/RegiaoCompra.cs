using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma RegiaoCompra.
    /// </summary>
    public class RegiaoCompra : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdRegiaoCompra;
            }

            set
            {
                this.IdRegiaoCompra = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdRegiaoCompra.
        /// </summary>
        public int IdRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define dsRegiaoCompra.
        /// </summary>
        public string dsRegiaoCompra { get; set; }
        #endregion

    }

}