using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Dados básicos de bandeira.
    /// </summary>
    public class BandeiraResumo
    {
        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string DsBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o código de estrutura mercadológica.
        /// </summary>
        public byte? CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o id do formato.
        /// </summary>
        public int? IDFormato { get; set; }
    }
}
