using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma Região Administrativa.
    /// </summary>
    public class RegiaoAdministrativa : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdRegiaoAdministrativa;
            }

            set
            {
                this.IdRegiaoAdministrativa = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdRegiaoAdministrativa.
        /// </summary>
        public int IdRegiaoAdministrativa { get; set; }

        /// <summary>
        /// Obtém ou define dsRegiaoAdministrativa.
        /// </summary>
        public string dsRegiaoAdministrativa { get; set; }
        #endregion

    }
}
