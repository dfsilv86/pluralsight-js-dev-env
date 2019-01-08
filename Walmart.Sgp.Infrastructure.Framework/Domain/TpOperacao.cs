using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define Tipos de operação
    /// </summary>
    public enum TpOperacao
    {
        /// <summary>
        /// Valor para Inclusão
        /// </summary>
        Inclusao = 'I',

        /// <summary>
        /// Valor para Alteração
        /// </summary>
        Alteracao = 'A',

        /// <summary>
        /// Valor para Exclusão
        /// </summary>
        Exclusao = 'E'
    }

}
