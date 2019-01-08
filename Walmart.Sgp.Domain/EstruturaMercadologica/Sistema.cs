using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa um sistema.
    /// </summary>
    public class Sistema : EntityBase, IAggregateRoot
    {
        #region Properties        
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return cdSistema;
            }

            set
            {
                cdSistema = (byte)value;
            }
        }

        /// <summary>
        /// Obtém ou define the código de sistema.
        /// </summary>
        public byte cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o texto.
        /// </summary>
        public string Text { get; set; }
        #endregion
    }
}
