using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma Formato.
    /// </summary>
    public class Formato : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDFormato;
            }

            set
            {
                IDFormato = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDFormato.
        /// </summary>
        public int IDFormato { get; set; }

        /// <summary>
        /// Obtém ou define dsFormato.
        /// </summary>
        public string dsFormato { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int cdSistema { get; set; }
        #endregion
    }
}
