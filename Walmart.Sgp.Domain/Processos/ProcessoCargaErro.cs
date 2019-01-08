using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa um erro na carga de um processo.
    /// </summary>
    [DebuggerDisplay("{Descricao}")]
    public class ProcessoCargaErro
    {
        #region Properties
        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string Descricao { get; set; }
        #endregion        
    }
}
