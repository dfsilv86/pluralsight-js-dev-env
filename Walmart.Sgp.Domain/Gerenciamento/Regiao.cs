using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma Regiao.
    /// </summary>
    [DebuggerDisplay("{nmRegiao}")]
    public class Regiao : EntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Regiao"/>
        /// </summary>
        public Regiao()
        {
            Distritos = new List<Distrito>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IDRegiao;
            }

            set
            {
                this.IDRegiao = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRegiao.
        /// </summary>
        public int IDRegiao { get; set; }

        /// <summary>
        /// Obtém ou define nmRegiao.
        /// </summary>
        public string nmRegiao { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int? IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define os distritos.
        /// </summary>
        public IEnumerable<Distrito> Distritos { get; set; }
        #endregion

        /// <summary>
        /// Obtém ou define Bandeira
        /// </summary>
        public Bandeira Bandeira { get; set; }
    }
}
