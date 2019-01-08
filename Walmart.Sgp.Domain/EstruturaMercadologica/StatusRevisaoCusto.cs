using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa um status revisao custo
    /// </summary>
    public class StatusRevisaoCusto : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define IDStatusRevisaoCusto.
        /// </summary>
        public int IDStatusRevisaoCusto { get; set; }
        
        /// <summary>
        /// Obtém ou define dsStatus.
        /// </summary>
        public string dsStatus { get; set; }
        #endregion
    }
}
