using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Informações necessárias para executar a importação automática de inventário.
    /// </summary>
    [Serializable]
    public class ImportarInventarioManualRequest
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        [ExposedParameter]
        public byte CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o código da bandeira.
        /// </summary>
        [ExposedParameter]
        public int IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        [ExposedParameter]
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define a data do inventário.
        /// </summary>
        [ExposedParameter]
        public DateTime DataInventario { get; set; }

        /// <summary>
        /// Obtém ou define os arquivos.
        /// </summary>
        [ExposedParameter(true)]
        public IEnumerable<FileVaultTicket> Arquivos { get; set; }
    }
}
