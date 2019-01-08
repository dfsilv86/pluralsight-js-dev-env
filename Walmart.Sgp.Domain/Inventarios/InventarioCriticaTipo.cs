using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa uma InventarioCriticaTipo.
    /// </summary>
    public class InventarioCriticaTipo : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDInventarioCriticaTipo;
            }

            set
            {
                IDInventarioCriticaTipo = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDInventarioCriticaTipo.
        /// </summary>
        public int IDInventarioCriticaTipo { get; set; }

        /// <summary>
        /// Obtém ou define nmTipo.
        /// </summary>
        public string nmTipo { get; set; }

        /// <summary>
        /// Obtém ou define dsTipo.
        /// </summary>
        public string dsTipo { get; set; }

        /// <summary>
        /// Obtém ou define blImportaArquivo.
        /// </summary>
        public bool? blImportaArquivo { get; set; }

        /// <summary>
        /// Obtém ou define blPermiteAprovacao.
        /// </summary>
        public bool? blPermiteAprovacao { get; set; }

        /// <summary>
        /// Obtém ou define blVisualizaHO.
        /// </summary>
        public bool? blVisualizaHO { get; set; }

        /// <summary>
        /// Obtém ou define blVisualizaLoja.
        /// </summary>
        public bool? blVisualizaLoja { get; set; }

    }

}
