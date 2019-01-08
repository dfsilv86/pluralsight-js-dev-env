using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa um processo.
    /// </summary>
    public class Processo : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IdProcesso;
            }

            set
            {
                IdProcesso = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdProcesso.
        /// </summary>
        public int IdProcesso { get; set; }

        /// <summary>
        /// Obtém ou define Descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define DiasProcessar.
        /// </summary>
        public int? DiasProcessar { get; set; }

        /// <summary>
        /// Obtém ou define IDGrupoEnvioEmail.
        /// </summary>
        public int? IDGrupoEnvioEmail { get; set; }
    }
}
