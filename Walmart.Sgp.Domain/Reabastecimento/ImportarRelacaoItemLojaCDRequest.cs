using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Requisição de importação de vinculo/desvinculo RelacaoItemLojaCD.
    /// </summary>
    [Serializable]
    public class ImportarRelacaoItemLojaCDRequest
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
        [ExposedParameter(true)]
        public IEnumerable<FileVaultTicket> Arquivos { get; set; }
    }
}
