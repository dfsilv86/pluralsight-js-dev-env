using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa um Motivo de Revisao de Custo.
    /// </summary>
    public class MotivoRevisaoCusto : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define IDMotivo.
        /// </summary>
        public int IDMotivo { get; set; }

        /// <summary>
        /// Obtém ou define dsMotivo.
        /// </summary>
        public string dsMotivo { get; set; }
        #endregion
    }
}
