using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de uma estratégia de auditoria/log.
    /// </summary>
    public interface IAuditStrategy
    {
        /// <summary>
        /// Usou o gateway para inserir as instâncias especificadas.
        /// </summary>
        /// <param name="instances">As instâncias.</param>
        void DidInsert(params object[] instances);

        /// <summary>
        /// Usou o gateway para alterar as instâncias especificadas.
        /// </summary>
        /// <param name="instances">As instâncias.</param>
        /// <remarks>Isso não indica que a instância teve algum valor realmente modificado, apenas que o gateway foi usado para executar um UPDATE no banco.</remarks>
        void DidUpdate(params object[] instances);

        /// <summary>
        /// Irá usar o gateway para excluir as instâncias especificadas.
        /// </summary>
        /// <param name="instances">As instâncias.</param>
        void WillDelete(params object[] instances);
    }
}
