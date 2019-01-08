using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Requisição de importação multisourcing.
    /// </summary>
    public class ImportarMultisourcingRequest
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public byte CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o id do usuário.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Obtém ou define os arquivos.
        /// </summary>
        public IEnumerable<FileVaultTicket> Arquivos { get; set; }
    }
}
