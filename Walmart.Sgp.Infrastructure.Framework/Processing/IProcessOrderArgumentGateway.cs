using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Define a interface de um table data gateway para ProcessOrderArgument.
    /// </summary>
    public interface IProcessOrderArgumentGateway : IDataGateway<ProcessOrderArgument>
    {
        /// <summary>
        /// Determina se existe um argumento cujo valor é o FileVaultTicket especificado.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <returns>True se existe um argumento com este arquivo, false caso contrário.</returns>
        bool HasFileVaultTicket(FileVaultTicket ticket);
    }
}
